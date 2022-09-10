using System;
using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class DarkenoidEffect : ReversibleEffect
    {
        public int HowMany = 0;

        private float xMax = 0;

        public override void OnStart()
        {
            applyImmediately = false;

            double radians = Math.PI / 360f * Darkenoid.DEGREES;

            xMax = (float)Math.Cos(radians - 0.5f * Math.PI);

            gun.AddAttackAction(AttackAction);
        }

        public override void OnOnDestroy()
        {

        }

        private void AttackAction()
        {
            Vector2 aim = player.data.aimDirection;

            gunStatModifier.RemoveGunStatModifier(gun);
            if (aim.y < 0 && aim.x < xMax && aim.x > -xMax)
            {
                gunStatModifier.bulletDamageMultiplier_mult = Darkenoid.DAMAGE_BUFF * HowMany;
                gunStatModifier.projectileSize_add = 10f;
            }
            else
            {
                gunStatModifier.bulletDamageMultiplier_mult = Darkenoid.DAMAGE_DEBUFF / HowMany;
                gunStatModifier.projectileSize_add = 0f;
            }
            gunStatModifier.ApplyGunStatModifier(gun);;
        }
    }
}
