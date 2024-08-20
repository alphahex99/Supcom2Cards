using ModdingUtils.MonoBehaviours;
using Sonigon;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class IncreaseSpreadEffect : CounterReversibleEffect
    {
        private float counter = 0;
        private bool modifiersActive = false;

        private int cardAmount = 1;

        public void Activate(int cardAmount)
        {
            this.cardAmount = cardAmount;

            counter = Cards.RadarJammer.RJ_SECONDS * cardAmount;

            SoundManager.Instance.Play(player.data.playerSounds.soundCharacterDamageScreenEdge, player.transform);
        }

        public override CounterStatus UpdateCounter()
        {
            counter -= TimeHandler.deltaTime;

            if (!modifiersActive && counter > 0)
            {
                return CounterStatus.Apply;
            }
            else if (counter <= 0)
            {
                Reset();
                return CounterStatus.Remove;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            gunStatModifier.spread_add = RadarJammer.BULLET_SPREAD * cardAmount;
        }

        public override void OnApply()
        {
            modifiersActive = true;
        }
        public override void OnRemove()
        {
            modifiersActive = false;
        }
        public override void Reset()
        {
            counter = 0;
            modifiersActive = false;
        }

        public override void OnStart()
        {
            applyImmediately = false;
            SetLivesToEffect(int.MaxValue);
        }
        public override void OnOnDestroy()
        {

        }
    }
}
