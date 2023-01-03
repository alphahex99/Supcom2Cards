#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using UnboundLib;
using ModsPlus;
using UnityEngine;

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

        private CustomHealthBar fuelBar;

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
            GameObject obj = new GameObject("JumpJets Fuel Bar");
            obj.transform.SetParent(parent);
            fuelBar = obj.AddComponent<CustomHealthBar>();
            fuelBar.transform.localPosition = Vector3.down * 0.25f;
            fuelBar.transform.localScale = Vector3.one;
            fuelBar.SetColor(Color.gray);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!player.data.dead)
            {
                fuelBar.SetValues(currentjumps - 1, jumps - 1);
            }
        }

        public override void OnOnDestroy()
        {
            base.OnOnDestroy();

            Destroy(fuelBar.gameObject);
        }
    }
}
