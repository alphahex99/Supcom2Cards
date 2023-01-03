#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModsPlus;
using UnityEngine;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class BombBouncerEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        private bool _full = false;
        private float _charge = 0;
        public float Charge
        {
            get
            {
                return _charge;
            }
            set
            {
                _charge = value;

                // update charge bar if full status changed
                float chargeMax = ChargeMax;
                if (_full)
                {
                    // don't &&
                    if (_charge < chargeMax)
                    {
                        chargeBar.SetColor(Color.yellow);
                        _full = false;
                    }
                }
                else if (_charge >= chargeMax)
                {
                    chargeBar.SetColor(Color.red);
                    _full = true;
                }
            }
        }

        public float ChargeMax => player.data.maxHealth * BombBouncer.DMG_REQUIRED_OF_MAX_HP / CardAmount;

        public ObjectsToSpawn? Explosion;

        public Player player;
        public Block block;

        private CustomHealthBar chargeBar;

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();

            block.BlockAction += OnBlock;
            player.data.stats.WasDealtDamageAction += OnDamage;

            // custom charge bar
            Transform parent = player.GetComponentInChildren<PlayerWobblePosition>().transform;
            GameObject obj = new GameObject("Bomb Bouncer Charge Bar");
            obj.transform.SetParent(parent);
            chargeBar = obj.AddComponent<CustomHealthBar>();
            chargeBar.transform.localPosition = Vector3.up * 0.25f;
            chargeBar.transform.localScale = Vector3.one;
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
            if (Charge >= ChargeMax)
            {
                // reset charge
                Charge = 0f;

                // spawn explosion at player location
                GameObject ex = Instantiate(Explosion.effect, player.data.transform.position, Quaternion.identity);

                // make the explosion THICC
                ex.transform.localScale *= 2.5f;

                // delete explosion after 2s
                Destroy(ex, 2);
            }
        }

        private void OnDamage(Vector2 damage, bool selfDamage)
        {
            Charge += damage.magnitude;
        }

        public void OnDestroy()
        {
            block.BlockAction -= OnBlock;
            player.data.stats.WasDealtDamageAction -= OnDamage;
        }
    }
}
