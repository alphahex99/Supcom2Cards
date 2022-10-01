using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using System;

namespace Supcom2Cards.MonoBehaviours
{
    public class VeterancyEffect : CounterReversibleEffect
    {
        public int HowMany = 0;

        // for some reason PlayerDied gets run twice when somebody dies so the boost needs to be half and max rank doubled
        private const float addMultPerKill = Veterancy.ADD_MULT_PER_KILL / 2;
        private const int maxKills = Veterancy.MAX_KILLS * 2;
        private int killsX2 = 0;
        private int rankX2 = 0;

        private bool active = true;
        private float hpAfterBoost = 0;

        private void PlayerDied(Player p, int idk)
        {
            if (active && p.teamID != player.teamID && p.data.lastSourceOfDamage == player)
            {
                killsX2++;
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
            if (active && killsX2 > 0 && rankX2 <= maxKills * HowMany)
            {
                rankX2 += killsX2;
                killsX2 = 0;
                return CounterStatus.Apply;
            }
            return CounterStatus.Wait;
        }

        public override void UpdateEffects()
        {
            // heal (later) to adjust for new max health
            hpAfterBoost = player.data.health * (1 + rankX2 * addMultPerKill);

            // update multipliers
            gunStatModifier.damage_mult = 1 + rankX2 * addMultPerKill;
            characterDataModifier.maxHealth_mult = 1 + rankX2 * addMultPerKill;
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
