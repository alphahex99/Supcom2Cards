using System;
using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class RogueNanitesEffect : ReversibleEffect
    {
        private Action<BlockTrigger.BlockTriggerType>? blockAction;

        public override void OnStart()
        {
            blockAction = GetBlockAction(player);

            block.BlockAction += blockAction;
        }

        public override void OnOnDestroy()
        {
            if (blockAction != null)
            {
                block.BlockAction -= blockAction;
            }
        }

        private Action<BlockTrigger.BlockTriggerType> GetBlockAction(Player player)
        {
            return delegate (BlockTrigger.BlockTriggerType trigger)
            {
                if (trigger == BlockTrigger.BlockTriggerType.Default ||
                    trigger == BlockTrigger.BlockTriggerType.Echo ||
                    trigger == BlockTrigger.BlockTriggerType.ShieldCharge)
                {
                    player.data.healthHandler.Heal(25f);
                }
            };
        }
    }
}
