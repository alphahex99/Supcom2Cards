#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;
using System;
using HarmonyLib;
using Sonigon.Internal;
using System.Globalization;

namespace Supcom2Cards.MonoBehaviours
{
    //TODO: refactor, optimize
    public class MegalithEffect : MonoBehaviour, ISingletonEffect
    {
        private int _cardAmount = 0;
        public int CardAmount
        {
            get => _cardAmount;

            set
            {
                _cardAmount = value;

                lasers.SetListCount(Megalith.LASERS * _cardAmount);
                foreach (Laser laser in lasers)
                {
                    laser.Color = Color.red;
                    laser.Width = Megalith.LASER_WIDTH;
                }
            }
        }

        public Player player;
        public Block block;

        private readonly List<Laser> lasers = new List<Laser>(2);

        public void FixedUpdate()
        {
            if (CardAmount < 1)
            {
                return;
            }

            lasers.ForEach(l => l.DrawHidden());

            if (!player.Simulated())
            {
                return;
            }

            // order visible enemies that are alive by their distance from the player
            Player[] targets = player.VisibleEnemies()
                .OrderBy(enemy => Vector3.Distance(enemy.transform.position, player.transform.position))
                .ToArray();

            int targetsCount = targets.Length;
            if (targetsCount < 1)
            {
                return;
            }

            int q = Megalith.LASERS * _cardAmount / targetsCount;
            int remaining = Megalith.LASERS * _cardAmount % targetsCount;

            bool isOwner = player.data.view.IsMine;

            for (int i = 0; i < lasers.Count; i++)
            {
                if (i >= targetsCount)
                {
                    break;
                }

                // how many lasers are locked on target
                int locks = q + ((i < remaining) ? 1 : 0);

                // draw
                lasers[i].Width = Megalith.LASER_WIDTH * locks;
                lasers[i].Draw(player.transform.position, targets[i].transform.position);

                // damage
                if (isOwner)
                {
                    float dps = Megalith.DPS_ABS + Megalith.DPS_REL * targets[i].data.maxHealth * locks;

                    targets[i].data.healthHandler.CallTakeDamage(
                        Vector2.up * dps * TimeHandler.fixedDeltaTime,
                        targets[i].data.transform.position,
                        damagingPlayer: player
                    );
                }
            }
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();

            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public void OnDestroy()
        {
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        private void PlayerDied(Player p, int idk)
        {
            if (p == player)
            {
                // owner died, hide lasers
                lasers.ForEach(r => r.DrawHidden());
            }
        }
    }
}