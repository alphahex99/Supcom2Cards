using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class QuantumSpongeEffect : ReversibleEffect
    {
        public bool ChromeShielded = false;

        private void Recharge()
        {
            if (ChromeShielded)
            {
                ChromeShielded = false;
                return;
            }
            if (block.IsOnCD() && block.counter < QuantumSponge.RECHARGE * block.Cooldown())
            {
                block.counter = QuantumSponge.RECHARGE * block.Cooldown();
            }
        }

        public override void OnStart()
        {
            player.data.stats.WasDealtDamageAction += OnDamage;
        }

        public override void OnOnDestroy()
        {
            player.data.stats.WasDealtDamageAction -= OnDamage;
        }

        private void OnDamage(Vector2 damage, bool selfDamage)
        {
            // small delay required for cards like Chrome Shield
            Supcom2.instance.ExecuteAfterFrames(1, Recharge);
        }
    }
}
