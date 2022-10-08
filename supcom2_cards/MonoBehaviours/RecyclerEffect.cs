#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class RecyclerEffect : MonoBehaviour
    {
        public Player player;

        private float counter = 1;

        public void FixedUpdate()
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                // damage enemies
                float stolenHP = 0;
                foreach (Player p in PlayerManager.instance.players)
                {
                    if (!p.data.dead && p.teamID != player.teamID)
                    {
                        float dmg = p.data.maxHealth * Recycler.DPS_HP_PERCENT / 100f;
                        p.TakeDamage(dmg, player);
                        stolenHP += dmg;
                    }
                }
                // heal owner
                player.data.healthHandler.Heal(stolenHP);

                // reset counter
                counter = 1;
            }
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
        }
    }
}