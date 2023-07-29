#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModsPlus;
using UnityEngine;
using Supcom2Cards.Cards;
using UnboundLib;

namespace Supcom2Cards.MonoBehaviours
{
    public class BombBouncerEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; }

        private float _charge = 0;
        public float Charge
        {
            get => _charge;

            set
            {
                _charge = value;

                if (_charge >= ChargeMax)
                {
                    _charge = ChargeMax;
                }

                chargeBar.SetColor(_charge < ChargeMax ? BombBouncer.COLOR_UNCHARGED : BombBouncer.COLOR_CHARGED);
            }
        }

        public float ChargeMax => player.data.maxHealth * CardAmount;

        public ObjectsToSpawn? Explosion;
        public Explosion ExplosionData;

        public Player player;
        public Block block;

        private CustomHealthBar chargeBar;

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();

            block.BlockAction += OnBlock;
            player.data.stats.WasDealtDamageAction += OnDamage;

            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);

            // custom charge bar
            Transform parent = player.GetComponentInChildren<PlayerWobblePosition>().transform;
            GameObject obj = new GameObject("Bomb Bouncer Charge Bar");
            obj.transform.SetParent(parent);
            chargeBar = obj.AddComponent<CustomHealthBar>();
            chargeBar.transform.localPosition = Vector3.up * 0.25f;
            chargeBar.transform.localScale = Vector3.one;
            chargeBar.SetColor(BombBouncer.COLOR_UNCHARGED);
        }

        public void Update()
        {
            if (!player.data.dead)
            {
                chargeBar.SetValues(Charge, ChargeMax);
            }
        }

        private void OnBlock(BlockTrigger.BlockTriggerType trigger)
        {
            if (Charge <= 0f || Explosion == null)
            {
                return;
            }
            
            // update explosion damage
            Explosion.effect.GetComponent<Explosion>().damage = Charge;

            // reset charge
            Charge = 0f;

            // spawn explosion at player location
            GameObject ex = Instantiate(Explosion.effect, player.data.transform.position, Quaternion.identity);

            // make the explosion THICC
            ex.transform.localScale *= 3f;

            // delete explosion after 2s
            Destroy(ex, 2);
        }

        private void OnDamage(Vector2 damage, bool selfDamage)
        {
            Charge += damage.magnitude;
        }

        public void OnDestroy()
        {
            block.BlockAction -= OnBlock;
            player.data.stats.WasDealtDamageAction -= OnDamage;

            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        private void PlayerDied(Player p, int idk)
        {
            if (p == player)
            {
                // owner died, reset charge
                Charge = 0f;
            }
        }
    }
}
