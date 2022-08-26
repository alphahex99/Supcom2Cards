using ModdingUtils.MonoBehaviours;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class MagnetronEffect : ReversibleEffect
    {
        public int HowMany = 0;

        private bool active = false;
        private float counter = 0;
        private float force_mult = 0;

        private List<Player> enemies = new List<Player>();

        public void Activate(float force, float seconds)
        {
            enemies = PlayerManager.instance.players.Where(p => p.teamID != player.teamID).ToList();

            counter += HowMany * seconds;
            this.force_mult = 1000f * force;

            active = true;
        }

        public override void OnUpdate()
        {
            if (active)
            {
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    counter = 0;
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

                Vector3 force = force_mult / distance_squared * dir;

                // nerf vertical component since players can only strafe horizontally
                enemy.data.healthHandler.CallTakeForce(new Vector2(force.x, 0.1f * force.y), forceIgnoreMass: true, ignoreBlock: true);

                // check to damage enemy
                if (distance <= 2.5f)
                {
                    enemy.data.healthHandler.CallTakeDamage(0.5f * Vector2.up, enemy.transform.position, damagingPlayer: player);
                    player.data.healthHandler.Heal(1f);
                }
            }
        }
    }
}