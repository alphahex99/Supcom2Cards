using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class RecyclerEffect : ReversibleEffect
    {
        private float counter = 1;
        public override void OnUpdate()
        {
            if (!player.data.dead)
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
                            p.data.healthHandler.TakeDamage(Vector2.up * dmg, p.data.transform.position, ignoreBlock:true);
                            stolenHP += dmg;
                        }
                    }
                    // heal owner
                    player.data.healthHandler.Heal(stolenHP);

                    // reset counter
                    counter = 1;
                }
            }
        }
    }
}