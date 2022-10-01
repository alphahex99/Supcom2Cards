using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using System;

namespace Supcom2Cards.MonoBehaviours
{
    public class VeterancyEffect : CounterReversibleEffect
    {
        public int HowMany = 0;

        private bool active = true;
        private float kills = 0;
        private float rank = 0;

        private float hpAfterBoost = 0;

        private void PlayerDied(Player p, int idk)
        {
            if (active && p.teamID != player.teamID && p.data.lastSourceOfDamage == player)
            {
                // for some reason this gets run twice when somebody dies so add 0.5 instead of 1
                kills += 0.5f;
            }
        }

        public override void OnStart()
        {
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public override void OnOnDestroy()
        {
            active = false;
            //PlayerManager.instance.InvokeMethod("RemovePlayerDiedAction", (Action)PlayerDied);
        }

        public override CounterStatus UpdateCounter()
        {
            if (active && kills > 0 && rank <= Veterancy.MAX_KILLS * HowMany)
            {
                rank = Math.Min(rank + kills, Veterancy.MAX_KILLS * HowMany);
                kills = 0;
                return CounterStatus.Apply;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            // heal (later) to adjust for new max health
            hpAfterBoost = player.data.health * (1 + rank * Veterancy.ADD_MULT_PER_KILL);

            // update multipliers
            gunStatModifier.damage_mult = 1 + rank * Veterancy.ADD_MULT_PER_KILL;
            characterDataModifier.maxHealth_mult = 1 + rank * Veterancy.ADD_MULT_PER_KILL;
        }

        public override void Reset()
        {
        }

        public override void OnApply()
        {
            // heal to adjust for new max health
            player.data.health = hpAfterBoost;
        }
    }
}
