using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class ChromeShieldEffect : ReversibleEffect
    {
        public override void OnStart()
        {
            player.data.stats.WasDealtDamageAction += OnDamage;
        }

        public override void OnOnDestroy()
        {
            player.data.stats.WasDealtDamageAction -= OnDamage;
        }

        private void OnDamage(Vector2 damage, bool selfDamage)
        {
            if (!block.IsOnCD())
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
            }
        }
    }
}
