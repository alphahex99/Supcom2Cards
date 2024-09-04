using ModdingUtils.MonoBehaviours;
using Sonigon;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class HardenEffect : CounterReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public float counterValue = 0f;

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
            gunStatModifier.attackSpeed_mult = Harden.ATTACKSPEED_MULT;
            gunStatModifier.projectileSpeed_mult = Harden.PROJECTILESPEED_MULT;
        }

        public override void OnApply()
        {

        }

        public override void Reset()
        {

        }

        public override void OnStart()
        {
            block.BlockAction += OnBlock;
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public override void OnOnDestroy()
        {
            block.BlockAction -= OnBlock;
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        private void OnBlock(BlockTrigger.BlockTriggerType trigger)
        {
            if (trigger == BlockTrigger.BlockTriggerType.Default ||
                trigger == BlockTrigger.BlockTriggerType.Echo ||
                trigger == BlockTrigger.BlockTriggerType.ShieldCharge)
            {
                counterValue += Harden.DURATION * CardAmount;

                SoundManager.Instance.Play(player.data.block.soundBlockStatusEffect, block.transform);
            }
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