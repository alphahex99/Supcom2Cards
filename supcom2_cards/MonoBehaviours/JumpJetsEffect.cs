#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using UnboundLib;
using ModsPlus;
using UnityEngine;
using Sonigon;

namespace Supcom2Cards.MonoBehaviours
{
    public class JumpJetsEffect : InAirJumpEffect, ISingletonEffect
    {
        private int _cardAmount = 0;
        public int CardAmount
        {
            get => _cardAmount;

            set
            {
                _cardAmount = value;

                AddJumps(10);
            }
        }

        private float jumps => (float)this.GetFieldValue("jumps");
        private float currentJumps
        {
            get => (float)this.GetFieldValue("currentjumps");
            set => this.SetFieldValue("currentjumps", value);
        } 

        private CustomHealthBar fuelBar;
        private GeneralInput input;

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

            input = player.GetComponent<GeneralInput>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!player.data.dead)
            {
                fuelBar.SetValues(currentJumps - 1, jumps - 1);
            }

            // sound
            if (input.jumpIsPressed && !player.data.isGrounded && !player.data.isWallGrab && currentJumps > 0f)
            {
                SoundManager.Instance.Play(player.data.healthHandler.soundDamagePassive, player.transform);
                //SoundManager.Instance.Play(player.data.healthHandler.soundDamageLifeSteal, player.transform); farting.mp3
            }
        }

        public override void OnOnDestroy()
        {
            base.OnOnDestroy();

            Destroy(fuelBar.gameObject);
        }

        public void Refuel()
        {
            currentJumps = jumps;
        }
    }
}
