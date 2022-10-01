using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class VeterancyEffect : ReversibleEffect
    {
        public int HowMany = 0;

        // for some reason PlayerDied gets run twice when somebody dies so kills are doubled
        private int killsX2 = 0;
        private int rank = 0;

        private bool active = true;

        private float GetMult() => (1 + rank * Veterancy.ADD_MULT_PER_KILL);

        private void PlayerDied(Player p, int idk)
        {
            if (active && p.teamID != player.teamID && p.data.lastSourceOfDamage == player)
            {
                killsX2++;
            }
        }

        public override void OnUpdate()
        {
            if (killsX2 >= 2 && rank <= Veterancy.MAX_KILLS * HowMany)
            {
                rank++;
                killsX2 -= 2;

                UpdateBuffs();
            }
        }

        private void UpdateBuffs()
        {
            // update buffs
            ClearModifiers();
            gunStatModifier.damage_mult = GetMult();
            characterDataModifier.maxHealth_mult = GetMult();
            ApplyModifiers();

            // heal to adjust for new max health
            player.data.health *= GetMult();
        }

        public override void OnStart()
        {
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public override void OnOnDestroy()
        {
            active = false;
            // TODO: remove player died action properly
        }
    }
}
