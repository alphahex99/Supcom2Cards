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

        private float GetMult() => (1 + rank * Veterancy.ADD_MULT_PER_KILL);

        private void PlayerDied(Player p, int idk)
        {
            if (p.teamID != player.teamID && p.data.lastSourceOfDamage == player && rank < Veterancy.MAX_KILLS * HowMany)
            {
                killsX2++;
            }
        }

        public override void OnUpdate()
        {
            if (killsX2 >= 2)
            {
                rank++;
                killsX2 -= 2;

                // update buffs
                ClearModifiers();
                gunStatModifier.damage_mult = GetMult();
                characterDataModifier.maxHealth_mult = GetMult();
                ApplyModifiers();

                // heal to adjust for new max health
                player.data.health *= GetMult();
            }
        }

        public override void OnStart()
        {
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public override void OnOnDestroy()
        {
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }
    }
}
