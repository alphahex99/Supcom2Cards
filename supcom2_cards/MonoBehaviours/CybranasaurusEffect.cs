#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Sonigon;
using Supcom2Cards.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    //TODO: add visuals, maybe explosion?
    public class CybranasaurusEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; }

        public Player player;

        private static readonly float divisor = 1 / (Cybranasaurus.DISTANCE_MAX - Cybranasaurus.DISTANCE_MIN);

        private static SoundEvent sound;

        public float yMax = -1000f;

        private List<Laser> lasers = new List<Laser>(Cybranasaurus.CHARGE_EDGES);
        private Polygon p = new Polygon();

        private readonly float JUMP_MAX_INV = 1f / Cybranasaurus.JUMP_MAX;

        private float spin = 0f;

        public void FixedUpdate()
        {
            if (CardAmount < 1)
            {
                return;
            }
            lasers.ForEach(l => l.DrawHidden());
            if (!player.Simulated())
            {
                yMax = -1000f;
                return;
            }

            float y = player.transform.position.y;

            yMax = y > yMax ? y : yMax;

            spin += TimeHandler.fixedDeltaTime * Cybranasaurus.CHARGE_RPM;
            if (spin > 60f)
            {
                spin -= 60f;
            }
            Draw();
        }

        private void Draw()
        {
            float height = yMax - player.transform.position.y;
            height = Mathf.Clamp(height, 0, Cybranasaurus.JUMP_MAX);

            if (height < Cybranasaurus.JUMP_MIN)
            {
                return;
            }

            Color color = Color.yellow;
            if (height > Cybranasaurus.JUMP_MAX * 0.95f)
            {
                // fully charged
                color = Color.red;
            }
            lasers.ForEach(l => l.Color = color);

            float size = height * JUMP_MAX_INV;
            p.Size = size * Cybranasaurus.CHARGE_SIZE;

            float dphi = spin * 0.10471975511965977461542144610932f; // dphi = spin * 2pi / 60
            p.Draw(player.transform.position.x, player.transform.position.y, lasers, size * dphi);
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();

            player.data.TouchGroundAction += OnTouchGround;
            player.data.TouchWallAction += OnTouchWall;
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);

            sound = player.data.playerSounds.soundCharacterLandBig;

            lasers.SetListCount(Cybranasaurus.CHARGE_EDGES);

            lasers.ForEach(l => l.Width = 0.3f);

            p.Edges = Cybranasaurus.CHARGE_EDGES;
        }

        public void OnTouchGround(float sinceGrounded, Vector3 pos, Vector3 groundNormal, Transform groundTransform)
        {
            Damage();

            // reset
            yMax = player.transform.position.y;
        }

        public void OnTouchWall(float sinceWallGrab, Vector3 pos, Vector3 normal)
        {
            Damage();

            // reset
            yMax = player.transform.position.y;
        }

        private void Damage()
        {
            float height = yMax - player.transform.position.y;
            height = Mathf.Clamp(height, 0, Cybranasaurus.JUMP_MAX);

            if (height < Cybranasaurus.JUMP_MIN)
            {
                return;
            }

            float jumpMult = height * JUMP_MAX_INV;

            float jumpDmg = player.data.health * Cybranasaurus.HP_DMG_MULT * jumpMult;

            // play sound
            //SoundManager.Instance.Play(sound, transform, new SoundParameterIntensity(jumpDmg * 100f));

            // damage
            if (player.data.view.IsMine)
            {
                IEnumerable<Player> enemies = PlayerManager.instance.players.Where(
                    enemy => !enemy.data.dead && enemy.teamID != player.teamID && enemy.Simulated()
                );

                foreach (Player enemy in enemies)
                {
                    float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
                    if (distance > Cybranasaurus.DISTANCE_MAX)
                    {
                        // too far
                        continue;
                    }

                    if (distance > Cybranasaurus.DISTANCE_MIN)
                    {
                        // quadratic distance falloff
                        jumpDmg *= 1f - Mathf.Pow((distance - Cybranasaurus.DISTANCE_MIN) * divisor, 2);
                    }

                    enemy.data.healthHandler.CallTakeDamage(Vector2.up * jumpDmg, enemy.data.transform.position, damagingPlayer: player);
                }
            }
        }

        public void OnDestroy()
        {
            player.data.TouchGroundAction -= OnTouchGround;
            player.data.TouchWallAction -= OnTouchWall;
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
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
