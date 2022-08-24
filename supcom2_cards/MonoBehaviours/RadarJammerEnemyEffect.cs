using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class RadarJammerEnemyEffect : ReversibleEffect
    {
        private bool active = false;
        private RadarJammerOwnerEffect? owner = null;

        public void Activate(RadarJammerOwnerEffect owner)
        {
            this.owner = owner;
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
            gunStatModifier.spread_add = 0.125f;

            base.OnStart();
        }
    }
}