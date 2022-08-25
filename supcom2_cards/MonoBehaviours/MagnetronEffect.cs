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
        private float force = 0;

        private List<Player> enemies = new List<Player>();

        public void Activate(float force, float seconds)
        {
            enemies = PlayerManager.instance.players.Where(p => p.teamID != player.teamID).ToList();

            counter += seconds * HowMany;
            this.force = force;

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
            foreach(Player p in enemies)
            {
                Vector3 dir = p.transform.position - player.transform.position;
                float distance_squared = dir.magnitude * dir.magnitude;
                dir.Normalize();

                Vector2 forceV = dir * force / distance_squared;

                // manually Clamp because Vector2.ClampMagnitude() doesn't do anything >:(
                float max = force > 0 ? 30f : 20f;
                if (forceV.magnitude > max)
                {
                    float coef = max / forceV.magnitude;
                    forceV.x *= coef;
                    forceV.y *= coef;
                }
                p.data.healthHandler.CallTakeForce(forceV, ignoreBlock: true);
            }
        }
    }
}