using System;
using ModdingUtils.MonoBehaviours;
using UnboundLib;

namespace Supcom2Cards.MonoBehaviours
{
    public class StackedCannonsEffect : ReversibleEffect
    {
        public int HowMany = 0;

        public void AttackAction()
        {
            gun.numberOfProjectiles = gunAmmo.maxAmmo * HowMany;
        }

        public override void OnStart()
        {
            gun.AddAttackAction(AttackAction);
        }
        public override void OnOnDestroy()
        {
            gun.InvokeMethod("RemoveAttackAction", (Action)AttackAction);
        }
    }
}
