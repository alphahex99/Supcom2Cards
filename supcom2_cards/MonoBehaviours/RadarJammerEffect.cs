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
    public class RadarJammerEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player player;

        // use Activate() after adding RadarJammerEnemyEffect component; when RadarJammerEnemyEffect is destroyed it removes it's player from playersJammed automatically
        public readonly List<Player> playersJammed = new List<Player> ();

        private bool applied = false;

        public void Update()
        {
            if (!applied)
            {
                IEnumerable<Player> players = PlayerManager.instance.players.Where(p => p.teamID != player.teamID);
                foreach (Player p in players.Except(playersJammed))
                {
                    RadarJammed jammedEffect = p.gameObject.AddComponent<RadarJammed>();
                    jammedEffect.CardAmount = CardAmount;

                    playersJammed.Add(p);
                }
                applied = true;
            }
        }

        public void Start()
        {
            player = GetComponent<Player>();

            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public void OnDestroy()
        {
            RemoveJammed();

            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        public void RemoveJammed()
        {
            foreach (Player jammedPlayer in playersJammed)
            {
                RadarJammed jammedEffect = jammedPlayer.gameObject.GetComponent<RadarJammed>();
                Destroy(jammedEffect);
            }
            playersJammed.Clear();
            applied = false;
        }

        private void PlayerDied(Player p, int idk)
        {
            // BEWARE: PlayerDied gets run twice for some reason
            if (p == player)
            {
                // owner died, remove RadarJammed effect from everyone
                RemoveJammed();
            }
        }
    }

    public class RadarJammed : ReversibleEffect, ISingletonEffect
    {
        public int CardAmount { get; set; }

        public override void OnStart()
        {
            SetLivesToEffect(int.MaxValue);

            gunStatModifier.spread_add = RadarJammer.BULLET_SPREAD * CardAmount;

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

            gunStatModifier.projectileSpeed_mult += (RNG.NextFloat() * 2 - 1) * RadarJammer.BULLET_SPEED_RAND * CardAmount;

            ApplyModifiers();

            Supcom2.Instance.ExecuteAfterFrames(1, RemoveRandomizedBulletSpeed);
        }

        private void RemoveRandomizedBulletSpeed()
        {
            ClearModifiers();

            gunStatModifier.projectileSpeed_mult = 1;

            ApplyModifiers();
        }
    }
}