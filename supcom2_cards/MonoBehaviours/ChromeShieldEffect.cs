#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using Sonigon;

namespace Supcom2Cards.MonoBehaviours
{
    public class ChromeShieldEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player player;
        public Block block;

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
            if (!block.IsOnCD() && player.data.silenceTime < 0.01f && HalfBakedEffect.TryBlock(player))
            {
                // instantly heal back the damage before the game realizes the player died
                // don't use healthHandler.Heal() to avoid overhealing if player has healing boosts
                player.data.health += damage.magnitude;

                // alert quantum sponge to not activate
                QuantumSpongeEffect sponge = player.gameObject.GetComponent<QuantumSpongeEffect>();
                if (sponge != null)
                {
                    sponge.ChromeShielded = true;
                }

                block.TryBlock();
                blocked();
            }
        }

        private void blocked()
        {
            SoundManager.Instance.Play(block.soundBlockBlocked, block.transform);

            // TODO: Reflect projectile instead of eating it
        }
    }
}
