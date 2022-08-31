using System;
using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class ChromeShieldEffect : ReversibleEffect
    {
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
            float dmg = damage.magnitude;
            if (!block.IsOnCD() && dmg > 0.05f * player.data.maxHealth)
            {
                player.data.healthHandler.Heal(dmg);
                block.TryBlock();
            }
        }
    }
}
