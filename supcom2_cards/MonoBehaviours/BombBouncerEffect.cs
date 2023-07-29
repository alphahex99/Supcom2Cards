#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModsPlus;
using UnityEngine;
using Supcom2Cards.Cards;
using UnboundLib;

namespace Supcom2Cards.MonoBehaviours
{
    public class BombBouncerEffect : MonoBehaviour, ISingletonEffect
    {
        private int _cardAmount = 0;
        public int CardAmount
        {
            get => _cardAmount;

            set
            {
                _cardAmount = value;

                UpdateExplosion();
            }
        }

        private bool _full = false;
        private float _charge = 0;
        public float Charge
        {
            get => _charge;

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

        public float ChargeMax => player.data.maxHealth * BombBouncer.DMG_REQUIRED_OF_MAX_HP;

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

            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);

            // custom charge bar
            Transform parent = player.GetComponentInChildren<PlayerWobblePosition>().transform;
            GameObject obj = new GameObject("Bomb Bouncer Charge Bar");
            obj.transform.SetParent(parent);
            chargeBar = obj.AddComponent<CustomHealthBar>();
            chargeBar.transform.localPosition = Vector3.up * 0.25f;
            chargeBar.transform.localScale = Vector3.one;

            if (CardAmount == 1)
            {
                UpdateExplosion();
            }
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
            if (Charge >= ChargeMax && Explosion != null)
            {
                // reset charge
                Charge = 0f;

                // spawn explosion at player location
                GameObject ex = Instantiate(Explosion.effect, player.data.transform.position, Quaternion.identity);

                // make the explosion THICC
                ex.transform.localScale *= 2f;

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

        private void UpdateExplosion()
        {
            (GameObject AddToProjectile, GameObject effect, Explosion explosion) = Supcom2.LoadExplosion("explosionBombBuncer");

            explosion.damage = BombBouncer.EXPLOSION_DMG * CardAmount;

            Explosion = new ObjectsToSpawn
            {
                AddToProjectile = AddToProjectile,
                direction = ObjectsToSpawn.Direction.forward,
                effect = effect,
                normalOffset = 0.1f,
                scaleFromDamage = 0.5f,
                scaleStackM = 0.7f,
                scaleStacks = true,
                spawnAsChild = false,
                spawnOn = ObjectsToSpawn.SpawnOn.all,
                stacks = 0,
                stickToAllTargets = false,
                stickToBigTargets = false,
                zeroZ = false
            };

            // set this player as owner of the explosion
            effect.GetOrAddComponent<SpawnedAttack>().spawner = player;
        }
    }
}
