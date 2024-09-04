#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;
using UnboundLib.Utils;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    // TODO: rewrite with decrementing counterValue instead of comparing times
    // TODO: add visuals, maybe recolored ToxicCloud?
    public class MagnetronEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player player;
        public Block block;

        private bool active = false;
        private float currentForce = 0;
        private float timeStarted = 0;

        private Player[] enemies;

        public void Activate(float force)
        {
            currentForce = force;

            if (!active)
            {
                enemies = PlayerManager.instance.players.Where(p => p.teamID != player.teamID).ToArray();
                timeStarted = Time.time;

                active = true;
            }
        }

        public void FixedUpdate()
        {
            if (CardAmount < 1 || !player.Simulated())
            {
                return;
            }

            if (active && Time.time - timeStarted > Magnetron.DURATION * CardAmount)
            {
                active = false;
            }
            if (!active)
            {
                return;
            }

            foreach (Player enemy in enemies)
            {
                Vector3 dir = enemy.transform.position - player.transform.position;
                float distance = dir.magnitude;
                dir.Normalize();

                float distance_squared = distance * distance;
                if (distance_squared < 20f)
                {
                    distance_squared = 20f;
                }

                //TODO: wtf? why?
                if (distance_squared > float.MaxValue)
                {
                    distance_squared = float.MaxValue;
                }

                Vector3 force = currentForce / distance_squared * dir * TimeHandler.fixedDeltaTime;

                // nerf vertical component since players can only strafe horizontally
                enemy.data.healthHandler.TakeForce(new Vector2(force.x, 0.1f * force.y), forceIgnoreMass: true);

                // check to damage enemy
                Vector2 ownerPos = new Vector2(player.transform.position.x, player.transform.position.y);
                if (distance <= 3.0f && PlayerManager.instance.CanSeePlayer(ownerPos, enemy).canSee)
                {
                    enemy.TakeDamage(Magnetron.DPS * CardAmount * TimeHandler.fixedDeltaTime);
                    player.data.healthHandler.Heal(Magnetron.HPS * CardAmount * TimeHandler.fixedDeltaTime);
                }
            }
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();

            block.BlockAction += OnBlock;
        }
        public void OnDestroy()
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