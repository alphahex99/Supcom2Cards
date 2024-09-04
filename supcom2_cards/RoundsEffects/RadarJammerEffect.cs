#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using ModdingUtils.RoundsEffects;
using Sonigon;
using Supcom2Cards.Cards;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards.RoundsEffects
{
    public class RadarJammerEffect : HitEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public override void DealtDamage(Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            var effect = damagedPlayer.gameObject.GetOrAddComponent<RadarJammerEffectEnemy>();
            effect.counterValue = RadarJammer.DURATION * CardAmount;
            effect.spread_add = RadarJammer.BULLET_SPREAD * CardAmount;

            SoundManager.Instance.Play(
                damagedPlayer.data.playerSounds.soundCharacterDamageScreenEdge,
                damagedPlayer.transform
            );
        }
    }

    public class RadarJammerEffectEnemy : CounterReversibleEffect
    {
        public float counterValue = 0f;

        public float spread_add = RadarJammer.BULLET_SPREAD;

        public override CounterStatus UpdateCounter()
        {
            counterValue -= TimeHandler.deltaTime;
            if (counterValue > 0f)
            {
                return CounterStatus.Apply;
            }
            counterValue = 0f;
            return CounterStatus.Remove;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.spread_add = spread_add;
        }

        public override void OnApply()
        {

        }

        public override void Reset()
        {

        }

        public override void OnStart()
        {
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public override void OnOnDestroy()
        {
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        private void PlayerDied(Player p, int idk)
        {
            if (p == player)
            {
                counterValue = 0f;
                ClearModifiers();
            }
        }
    }
}