using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class RogueNanitesEffect : ReversibleEffect
    {
        public override void OnStart()
        {
            block.BlockAction += OnBlock;
        }

        public override void OnOnDestroy()
        {
            block.BlockAction -= OnBlock;
        }

        private void OnBlock(BlockTrigger.BlockTriggerType trigger)
        {
            if (trigger == BlockTrigger.BlockTriggerType.Default ||
                trigger == BlockTrigger.BlockTriggerType.Echo ||
                trigger == BlockTrigger.BlockTriggerType.ShieldCharge)
            {
                player.data.healthHandler.Heal(25f);
            }
        }
    }
}
