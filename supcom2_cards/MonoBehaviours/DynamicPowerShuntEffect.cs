#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Supcom2Cards.Cards;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class DynamicPowerShuntEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player player;
        public Block block;

        private bool active = false;

        private const float DURATION_MULT = 1 / DynamicPowerShunt.CD_MULT;

        private Vector3 lastPosition = new Vector3(0, 0, 0);

        public void Update()
        {
            if (active)
            {
                // reverse this frame's increment
                block.counter -= TimeHandler.deltaTime;

                // increment faster
                block.counter += TimeHandler.deltaTime * CardAmount * DURATION_MULT;
            }
        }

        public void FixedUpdate()
        {
            //active = Vector3.Distance(player.transform.position, lastPosition) < 1f;

            float dx = player.transform.position.x - lastPosition.x;
            dx = dx > 0 ? dx : -dx;
            float dy = player.transform.position.y - lastPosition.y;
            dy = dy > 0 ? dy : -dy;

            active = dx * dx + dy * dy < DynamicPowerShunt.MAX_SPEED_POW;
            lastPosition = player.transform.position;
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();
        }
    }
}
