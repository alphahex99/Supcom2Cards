using System;
using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class ChromeShieldEffect : ReversibleEffect
    {
        private Action<Vector2, bool>? wasDealtDamageAction;

        public override void OnStart()
        {
            wasDealtDamageAction = GetDamagedAction(player, block);

            player.data.stats.WasDealtDamageAction += wasDealtDamageAction;
        }

        public override void OnOnDestroy()
        {
            if (wasDealtDamageAction != null)
            {
                player.data.stats.WasDealtDamageAction -= wasDealtDamageAction;
            }
        }

        private Action<Vector2, bool> GetDamagedAction(Player player, Block block)
        {
            return delegate (Vector2 damage, bool idk)
            {
                float dmg = damage.magnitude;
                if (!block.IsOnCD() && dmg > 0.05f * player.data.maxHealth)
                {
                    player.data.healthHandler.Heal(dmg);
                    block.TryBlock();
                }
            };
        }
    }
}
