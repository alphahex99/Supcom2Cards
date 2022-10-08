#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Supcom2Cards.Cards;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class QuantumSpongeEffect : MonoBehaviour
    {
        public bool ChromeShielded = false;

        public Player player;
        public Block block;

        private void Recharge()
        {
            if (ChromeShielded)
            {
                ChromeShielded = false;
                return;
            }
            if (block.IsOnCD() && block.counter < QuantumSponge.RECHARGE * block.Cooldown())
            {
                block.counter = QuantumSponge.RECHARGE * block.Cooldown();
            }
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();

            player.data.stats.WasDealtDamageAction += OnDamage;
        }

        public void OnDestroy()
        {
            player.data.stats.WasDealtDamageAction -= OnDamage;
        }

        private void OnDamage(Vector2 damage, bool selfDamage)
        {
            // small delay required for cards like Chrome Shield
            Supcom2.Instance.ExecuteAfterFrames(1, Recharge);
        }
    }
}
