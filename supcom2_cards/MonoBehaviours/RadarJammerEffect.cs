﻿using System.Collections.Generic;
using System.Linq;
using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;

namespace Supcom2Cards.MonoBehaviours
{
    public class RadarJammerEffect : ReversibleEffect
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
                    p.gameObject.AddComponent<RadarJammed>().Activate(this);
                    playersJammed.Add(p);
                }
            }
            else
            {
                // remove Radar Jammer from everyone
                foreach (Player p in playersJammed)
                {
                    Destroy(p.gameObject.GetComponent<RadarJammed>());
                }
            }
        }
    }

    public class RadarJammed : ReversibleEffect
    {
        private bool active = false;
        private RadarJammerEffect? owner = null;

        public void Activate(RadarJammerEffect radarJammerOwner)
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

            gunStatModifier.spread_add = RadarJammer.BULLET_SPREAD;
            gunStatModifier.projectileSpeed_mult = RadarJammer.BULLET_SPEED_MULT;
        }
    }
}