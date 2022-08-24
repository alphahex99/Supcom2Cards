using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class RadarJammerEnemyEffect : ReversibleEffect
    {
        private bool active = false;
        private RadarJammerOwnerEffect? owner = null;

        public void Activate(RadarJammerOwnerEffect radarJammerOwner)
        {
            this.owner = radarJammerOwner;
            active = true;
        }

        public override void OnUpdate()
        {
            if (active && (owner == null || owner.player.data.dead))
            {
                if (owner != null)
                {
                    owner.playersJammed.Remove(player);
                }
                this.Destroy();
            }
        }

        public override void OnStart()
        {
            SetLivesToEffect(int.MaxValue);

            //gunStatModifier.projectileSpeed_mult = -1f;
            gunStatModifier.spread_add = RadarJammer.BULLET_SPREAD;
            gunStatModifier.projectileSpeed_mult = RadarJammer.BULLET_SPEED_MULT;

            base.OnStart();
        }
    }
}