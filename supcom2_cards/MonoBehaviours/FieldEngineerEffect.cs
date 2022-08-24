using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class FieldEngineerEffect : ReversibleEffect
    {
        private readonly float hps;
        private float counter = 1;


        public FieldEngineerEffect(float heal_per_second)
        {
            hps = heal_per_second;
        }

        public override void OnUpdate()
        {
            if (!player.data.dead)
            {
                counter -= Time.deltaTime;
                if (counter <= 0)
                {
                    // heal owner
                    player.data.healthHandler.Heal(hps);

                    // reset counter
                    counter = 1;
                }
            }
        }
    }
}