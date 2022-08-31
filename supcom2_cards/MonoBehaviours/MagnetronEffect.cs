using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class MagnetronEffect : ReversibleEffect
    {
        public int HowMany = 0;

        private bool active = false;
        private float force1k = 0;
        private float timeStarted = 0;

        private List<Player> enemies = new List<Player>();

        private float damagePerTick = 0;
        private float healingPerTick = 0;

        public void Activate(float force)
        {
            // OnFixedUpdate() updates 143 times per second from testing (probably)
            if (damagePerTick == 0)
            {
                damagePerTick = Magnetron.DPS / 143;
            }
            if (healingPerTick == 0)
            {
                healingPerTick = Magnetron.HPS / 143;
            }

            force1k = 1000f * force;

            if (!active)
            {
                enemies = PlayerManager.instance.players.Where(p => p.teamID != player.teamID).ToList();
                timeStarted = Time.time;

                active = true;
            }
        }

        public override void OnFixedUpdate()
        {
            if (active)
            {
                if (Time.time - timeStarted > Magnetron.MG_SECONDS * HowMany)
                {
                    active = false;
                }
                else if (!player.data.dead)
                {
                    OnTick();
                }
            }
        }

        private void OnTick()
        {
            foreach (Player enemy in enemies)
            {
                Vector3 dir = enemy.transform.position - player.transform.position;
                float distance = dir.magnitude;
                dir.Normalize();

                float distance_squared = Mathf.Clamp(distance * distance, 20f, float.MaxValue);

                Vector3 force = force1k / distance_squared * dir;

                // nerf vertical component since players can only strafe horizontally
                enemy.data.healthHandler.CallTakeForce(new Vector2(force.x, 0.1f * force.y), forceIgnoreMass: true, ignoreBlock: true);

                // check to damage enemy
                if (distance <= 2f)
                {
                    enemy.data.healthHandler.CallTakeDamage(Vector2.up * damagePerTick * HowMany, enemy.transform.position, damagingPlayer: player);
                    player.data.healthHandler.Heal(healingPerTick);
                }
            }
        }

        public override void OnStart()
        {
            block.BlockAction += OnBlock;

            base.OnStart();
        }
        public override void OnOnDestroy()
        {
            block.BlockAction -= OnBlock;
        }

        private void OnBlock(BlockTrigger.BlockTriggerType trigger)
        {
            if (trigger == BlockTrigger.BlockTriggerType.Default ||
                trigger == BlockTrigger.BlockTriggerType.Echo ||
                trigger == BlockTrigger.BlockTriggerType.ShieldCharge)
            {
                Activate(player.data.aimDirection.y > 0 ? Magnetron.FORCE_PUSH : -Magnetron.FORCE_PULL);
            }
        }
    }
}