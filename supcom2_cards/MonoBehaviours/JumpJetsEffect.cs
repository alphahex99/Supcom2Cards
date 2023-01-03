using ModdingUtils.MonoBehaviours;
using UnboundLib;
using ModsPlus;

namespace Supcom2Cards.MonoBehaviours
{
    public class JumpJetsEffect : InAirJumpEffect, ISingletonEffect
    {
        private int _cardAmount = 0;
        public int CardAmount
        {
            get
            {
                return _cardAmount;
            }

            set
            {
                _cardAmount = value;

                AddJumps(10);
            }
        }

        private float jumps => (float)this.GetFieldValue("jumps");
        private float currentjumps => (float)this.GetFieldValue("currentjumps");

        private CustomHealthBar? fuel;

        public override void OnStart()
        {
            base.OnStart();

            SetJumpMult(0.25f);
            SetContinuousTrigger(true);
            SetResetOnWallGrab(true);
            SetCostPerJump(1);
            SetInterval(0.025f);

            //fuel = player.gameObject.AddComponent<CustomHealthBar>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fuel != null)
            {
                fuel.SetValues(currentjumps, jumps);
            }
        }
    }
}
