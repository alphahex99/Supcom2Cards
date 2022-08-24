using System.Collections.Generic;
using System.Linq;
using ModdingUtils.MonoBehaviours;

namespace Supcom2Cards.MonoBehaviours
{
    public class RadarJammerOwnerEffect : ReversibleEffect
    {
        // use Activate() after adding RadarJammerEnemyEffect component; when RadarJammerEnemyEffect is destroyed it removes it's player from playersJammed automatically
        public readonly List<Player> playersJammed = new List<Player> ();

        public override void OnUpdate()
        {
            if (!player.data.dead)
            {
                // apply Radar Jammer to every enemy who doesn't already have it
                IEnumerable<Player> players = PlayerManager.instance.players.Where(p => p.teamID != player.teamID);
                foreach (Player p in players.Except(playersJammed))
                {
                    p.gameObject.AddComponent<RadarJammerEnemyEffect>().Activate(this);
                    playersJammed.Add(p);
                }
            }
            else
            {
                // remove Radar Jammer from everyone
                foreach (Player p in playersJammed)
                {
                    RadarJammerEnemyEffect mono = p.gameObject.GetComponent<RadarJammerEnemyEffect>();
                    mono.Destroy();
                }
            }
        }
    }
}