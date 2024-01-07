#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using ModdingUtils.RoundsEffects;
using Sonigon;

namespace Supcom2Cards.RoundsEffects
{
    public class QuantumForceBlastEffect : WasHitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player Owner;

        public override void WasDealtDamage(Vector2 damage, bool selfDamage)
        {
            SoundManager.Instance.Play(Owner.data.playerSounds.soundCharacterDamageScreenEdge, transform);
        }
    }
}
