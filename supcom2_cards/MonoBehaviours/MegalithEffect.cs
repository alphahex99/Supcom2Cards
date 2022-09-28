#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Supcom2Cards.MonoBehaviours
{
    public class MegalithEffect : ReversibleEffect
    {
        public int HowMany = 0;

        private float counter = 1;
        private const float UPDATE_PERIOD = 1 / Megalith.UPS; // time between targetting updates in milliseconds

        private IEnumerable<Player> visiblePlayers;

        public override void OnStart()
        {
            visiblePlayers = PlayerManager.instance.players.Where(p => PlayerManager.instance.CanSeePlayer(player.data.transform.position, p).canSee);
        }

        public override void OnUpdate()
        {
            if (!player.data.dead)
            {
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    // dps with card stats and UPS adjusted
                    float dps = Supcom2.GetGunDPS(gun, gunAmmo) * Megalith.DPS_MULT * UPDATE_PERIOD;

                    //UnityEngine.Debug.Log($"dps = {dps}");

                    foreach (Player p in visiblePlayers)
                    {
                        if (!p.data.dead && p.teamID != player.teamID)
                        {
                            p.data.healthHandler.TakeDamage(Vector2.up * dps, p.data.transform.position);
                        }
                    }

                    // reset counter
                    counter = UPDATE_PERIOD;
                }
            }
        }
    }
}