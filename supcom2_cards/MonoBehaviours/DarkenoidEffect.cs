#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Sonigon;
using Supcom2Cards.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Supcom2Cards.MonoBehaviours
{
    //TODO: players with ridiculous HP might have weird hitboxes that dont get hit by the laser because of Darkenoid.DPS_WIDTH
    // use CharacterData.mainCol (CircleCollider2D)?
    public class DarkenoidEffect : MonoBehaviour, ISingletonEffect
    {
        private int _cardAmount = 0;
        public int CardAmount
        {
            get { return _cardAmount; }
            set
            {
                _cardAmount = value;

                int count = Darkenoid.BEAM_COUNT * _cardAmount;

                lasers.SetListCount(count);

                lasers.ForEach(l => l.Width = Darkenoid.BEAM_WIDTH);
                lasers.SetTeamColor(gameObject, 2.5f);
            }
        }

        public Player player;
        public Gun gun;

        public static SoundEvent sound;

        private IEnumerable<Player> enemies;

        private readonly float beamDmg = Darkenoid.DPS / Darkenoid.BEAM_COUNT;

        private readonly List<Laser> lasers = new List<Laser>(Darkenoid.BEAM_COUNT);

        private int layerMask = LayerMask.GetMask("Default", "IgnorePlayer");

        // avoid CallTakeDamage 5x if there are 5 beams (Decay lag) -> store dmg instead and call 1x
        private Dictionary<Player, float> playersDMG = new Dictionary<Player, float>();

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            gun = player.Gun();

            sound = player.data.playerSounds.soundCharacterJumpEnsnare;

            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);

            enemies = PlayerManager.instance.players.Where(
                p => !p.data.dead && p.teamID != player.teamID
            );
        }

        public void OnDestroy()
        {
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        public void FixedUpdate()
        {
            if (CardAmount < 1)
            {
                return;
            }
            lasers.ForEach(l => l.DrawHidden());
            playersDMG.Clear();

            if (!player.Simulated())
            {
                return;
            }

            float pX = player.transform.position.x;
            float pY = player.transform.position.y;

            // left edge of the beam
            float xMin = pX - (lasers.Count - 1) * Darkenoid.BEAM_GAP;

            // loop through all parts of the beam
            for (int i = 0; i < lasers.Count; i++)
            {
                float bX = xMin + i * Darkenoid.BEAM_GAP * 2f;

                // draw
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(bX, pY), Vector2.down, 1000, layerMask);
                float hitY = (hit.collider == null) ? -1000 : hit.point.y;
                lasers[i].Draw(bX, pY, bX, hitY);

                // damage
                CalculateDamage(bX, pY, hitY, beamDmg * TimeHandler.fixedDeltaTime * gun.damage);
            }

            FinalizeDamage(player);
        }

        private void CalculateDamage(float bX, float pY, float hitY, float dmg)
        {
            foreach (Player enemy in enemies)
            {
                if (enemy.data.dead)
                {
                    continue;
                }

                float xMin = bX - Darkenoid.DPS_WIDTH;
                float xMax = bX + Darkenoid.DPS_WIDTH;

                float eX = enemy.transform.position.x;
                float eY = enemy.transform.position.y;

                // check horizontal position
                if (eX < xMin || eX > xMax)
                {
                    continue;
                }

                // check vertical position
                if (eY > pY || eY < hitY)
                {
                    continue;
                }

                // enemy is inside this laser, damage them
                if (player.data.view.IsMine)
                {
                    if (!playersDMG.ContainsKey(enemy))
                    {
                        playersDMG.Add(enemy, dmg);
                    }
                    else
                    {
                        playersDMG[enemy] = dmg;
                    }
                }
                SoundManager.Instance.Play(sound, enemy.transform);
            }
        }
        private void FinalizeDamage(Player damagingPlayer)
        {
            if (!player.data.view.IsMine)
            {
                return;
            }
            foreach (KeyValuePair<Player, float> playerDMG in playersDMG)
            {
                Player player = playerDMG.Key;
                float dmg = playerDMG.Value;
                if (dmg > 0f)
                {
                    player.data.healthHandler.CallTakeDamage(dmg * Vector2.up, player.data.transform.position, damagingPlayer: damagingPlayer);
                }
            }
        }

        private void PlayerDied(Player p, int idk)
        {
            if (p == player)
            {
                // owner died, hide drones
                lasers.ForEach(l => l.DrawHidden());
            }
        }
    }
}
