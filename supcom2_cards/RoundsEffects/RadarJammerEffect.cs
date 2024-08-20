#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.RoundsEffects;
using Supcom2Cards.MonoBehaviours;
using UnityEngine;

namespace Supcom2Cards.RoundsEffects
{
    public class RadarJammerEffect : HitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            if (this == null)
            {
                return;
            }

            IncreaseSpreadEffect effect = damagedPlayer.gameObject.GetComponent<IncreaseSpreadEffect>();
            if (effect == null)
            {
                effect = damagedPlayer.gameObject.AddComponent<IncreaseSpreadEffect>();
            }

            effect.Activate(CardAmount);
        }
    }
}
