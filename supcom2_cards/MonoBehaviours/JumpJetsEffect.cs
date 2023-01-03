using ModdingUtils.MonoBehaviours;
using UnboundLib;
using ModsPlus;
using UnityEngine;
using UnityEngine.UI;

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

            // custom fuel bar
            Transform parent = player.GetComponentInChildren<PlayerWobblePosition>().transform;
            GameObject obj = new GameObject("Jetpack Bar");
            obj.transform.SetParent(parent);
            fuel = obj.AddComponent<CustomHealthBar>();
            fuel.SetValues(data.jumps - 1, data.jumps - 1);

            // move fuel bar above normal health bar
            fuel.transform.localPosition = Vector3.down * 0.25f;
            fuel.transform.localScale = Vector3.one;

            fuel.SetColor(Color.gray);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fuel != null)
            {
                fuel.SetValues(currentjumps - 1, jumps - 1);
            }
        }

        public override void OnOnDestroy()
        {
            base.OnOnDestroy();

            if (fuel != null)
            {
                Destroy(fuel.gameObject);
            }
        }
    }
}
