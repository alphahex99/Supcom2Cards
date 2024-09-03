﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
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

                foreach (Laser laser in lasers)
                {
                    laser.Color = Color.cyan;
                    laser.Width = Darkenoid.BEAM_WIDTH;
                }

                float magicNumberIPulledOutOfMyAss_dontAsk = 5f;

                beamDmg = Vector2.up * Darkenoid.DPS / Darkenoid.BEAM_COUNT * magicNumberIPulledOutOfMyAss_dontAsk;
            }
        }

        public Player player;

        private IEnumerable<Player> enemies;

        private float counter = 0f;
        private const float DT = 1f / Darkenoid.UPS;

        Vector2 beamDmg = Vector2.up;

        private readonly List<Laser> lasers = new List<Laser>(Darkenoid.BEAM_COUNT);

        private int layerMask = LayerMask.GetMask("Default", "IgnorePlayer");

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();

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

            float pX = player.transform.position.x;
            float pY = player.transform.position.y;

            // left edge of the beam
            float xMin = pX - (lasers.Count - 1) * Darkenoid.BEAM_GAP;

            // loop through all parts of the beam
            for (int i = 0; i < lasers.Count; i++)
            {
                float bX = xMin + i * Darkenoid.BEAM_GAP;

                // draw
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(bX, pY), Vector2.down, 1000, layerMask);
                float hitY = (hit.collider == null) ? -1000 : hit.point.y;
                lasers[i].Draw(bX, pY, bX, hitY);

                // damage
                if (counter < 0f)
                {
                    counter = DT;
                }
                Damage(bX, pY, hitY, beamDmg * TimeHandler.deltaTime);
            }
        }

        private void Damage(float bX, float pY, float hitY, Vector2 dmg)
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
                enemy.data.healthHandler.TakeDamage(dmg, enemy.data.transform.position, damagingPlayer: player);
            }
        }

        private void PlayerDied(Player p, int idk)
        {
            if (p == player)
            {
                // owner died, hide drones
                foreach (Laser laser in lasers)
                {
                    laser.DrawHidden();
                }
            }
        }
    }
}
