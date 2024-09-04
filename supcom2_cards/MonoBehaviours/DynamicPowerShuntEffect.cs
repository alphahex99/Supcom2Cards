#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using Sonigon;
using Supcom2Cards.Cards;
using System;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class DynamicPowerShuntEffect : MonoBehaviour, ISingletonEffect
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

                double c = 1 / Math.Pow(DynamicPowerShunt.CD_MULT_STILL, _cardAmount);
                counterMult = (float)c;
            }
        }

        public Player player;
        public Block block;

        private bool standingStill = false;
        private float counterMult;

        private Vector3 lastPosition = new Vector3(0, 0, 0);
        private float delay = DynamicPowerShunt.STAND_DELAY;

        public void FixedUpdate()
        {
            if (standingStill)
            {
                delay -= Time.fixedDeltaTime;
                if (delay <= 0)
                {
                    block.counter += Time.deltaTime * (counterMult - 1f);
                }
            }
            else
            {
                delay = DynamicPowerShunt.STAND_DELAY;
            }

            standingStill = player.StandingStill(ref lastPosition);
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();
        }
    }
}
