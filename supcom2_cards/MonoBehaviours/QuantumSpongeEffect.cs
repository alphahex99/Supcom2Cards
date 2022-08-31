using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class QuantumSpongeEffect : ReversibleEffect
    {
        public void Recharge()
        {
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
            // small delay required to first block and then recharge for cards like Chrome Shield
            Supcom2.instance.ExecuteAfterFrames(1, Recharge);
        }
    }
}
