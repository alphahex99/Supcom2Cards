﻿#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class MagnetronEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player player;
        public Block block;

        private bool active = false;
        private float force1k = 0;
        private float timeStarted = 0;

        private List<Player> enemies = new List<Player>();

        private float damagePerTick = 0;
        private float healingPerTick = 0;

        public void Activate(float force)
        {
            force1k = 1000f * force;

            if (!active)
            {
                enemies = PlayerManager.instance.players.Where(p => p.teamID != player.teamID).ToList();
                timeStarted = Time.time;

                active = true;
            }
        }

        public void FixedUpdate()
        {
            if (active)
            {
                if (Time.time - timeStarted > Magnetron.MG_SECONDS * CardAmount)
                {
                    active = false;
                }
                else
                {
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
                        if (distance_squared > float.MaxValue)
                        {
                            distance_squared = float.MaxValue;
                        }

                        Vector3 force = force1k / distance_squared * dir;

                        // nerf vertical component since players can only strafe horizontally
                        enemy.data.healthHandler.TakeForce(new Vector2(force.x, 0.1f * force.y), forceIgnoreMass: true, ignoreBlock: true);

                        // check to damage enemy
                        Vector2 ownerPos = new Vector2(player.transform.position.x, player.transform.position.y);
                        if (distance <= 3.0f && PlayerManager.instance.CanSeePlayer(ownerPos, enemy).canSee)
                        {
                            enemy.TakeDamage(damagePerTick * CardAmount);
                            player.data.healthHandler.Heal(healingPerTick);
                        }
                    }
                }
            }
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();

            // TODO: FixedUpdate() is called 50 FPS, why 143?
            if (damagePerTick == 0)
            {
                damagePerTick = Magnetron.DPS / 143;
            }
            if (healingPerTick == 0)
            {
                healingPerTick = Magnetron.HPS / 143;
            }

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