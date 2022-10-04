#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using System;
using System.Collections.Generic;
using System.Linq;
using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class RadarJammerEffect : MonoBehaviour
    {
        // use Activate() after adding RadarJammerEnemyEffect component; when RadarJammerEnemyEffect is destroyed it removes it's player from playersJammed automatically
        public readonly List<Player> playersJammed = new List<Player> ();

        private bool applied = false;

        private Player player;

        public void Update()
        {
            if (!applied)
            {
                IEnumerable<Player> players = PlayerManager.instance.players.Where(p => p.teamID != player.teamID);
                foreach (Player p in players.Except(playersJammed))
                {
                    p.gameObject.AddComponent<RadarJammed>();
                    playersJammed.Add(p);
                }
                applied = true;
            }
        }

        public void Start()
        {
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);

            player = GetComponent<Player>();
        }

        public void OnDestroy()
        {
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        private void PlayerDied(Player p, int idk)
        {
            // BEWARE: PlayerDied gets run twice for some reason
            if (p == player)
            {
                // owner died, remove RadarJammed effect from everyone
                foreach (Player jammedPlayer in playersJammed)
                {
                    Destroy(jammedPlayer.gameObject.GetComponent<RadarJammed>());
                }
                playersJammed.Clear();
                applied = false;
            }
        }
    }

    public class RadarJammed : ReversibleEffect
    {
        public override void OnStart()
        {
            SetLivesToEffect(int.MaxValue);

            gunStatModifier.spread_add = RadarJammer.BULLET_SPREAD;

            gun.AddAttackAction(AttackAction);
        }

        public override void OnOnEnable()
        {
            ApplyModifiers();
        }

        public override void OnOnDestroy()
        {
            gun.InvokeMethod("RemoveAttackAction", (Action)AttackAction);
        }

        public override void OnOnDisable()
        {
            ClearModifiers();
        }

        private void AttackAction()
        {
            ClearModifiers();

            float rand = (float)Supcom2.RNG.NextDouble();
            gunStatModifier.projectileSpeed_mult += (rand * 2 - 1) * RadarJammer.BULLET_SPEED_RAND;

            UnityEngine.Debug.Log(rand);

            ApplyModifiers();

            Supcom2.instance.ExecuteAfterFrames(1, RemoveRandomizedBulletSpeed);
        }

        private void RemoveRandomizedBulletSpeed()
        {
            ClearModifiers();

            gunStatModifier.projectileSpeed_mult = 1;

            ApplyModifiers();
        }
    }
}