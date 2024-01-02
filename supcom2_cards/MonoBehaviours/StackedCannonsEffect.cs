#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class StackedCannonsEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player player;
        public Gun gun;
        public GunAmmo gunAmmo;
        public Block block;

        public void AttackAction()
        {
            gun.numberOfProjectiles = gunAmmo.maxAmmo * CardAmount;
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            gun = player.data.weaponHandler.gun;
            block = player.GetComponent<Block>();
            gunAmmo = gun.GetComponentInChildren<GunAmmo>();

            gun.AddAttackAction(AttackAction);
        }
        public void OnDestroy()
        {
            gun.InvokeMethod("RemoveAttackAction", (Action)AttackAction);
        }
    }
}
