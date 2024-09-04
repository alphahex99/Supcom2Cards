#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Supcom2Cards.MonoBehaviours
{
    //TODO: refactor, optimize
    public class MegalithEffect : MonoBehaviour, ISingletonEffect
    {
        private int _cardAmount = 0;
        public int CardAmount
        {
            get { return _cardAmount; }
            set
            {
                _cardAmount = value;

                targets = new Player[Megalith.LASERS * _cardAmount];
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

        private float counter = 0f;
        private const float DT = 1f / Megalith.UPS;

        private IEnumerable<Player> visibleEnemies;
        private Player[] targets;

        private readonly List<Laser> lasers = new List<Laser>(2);

        public void FixedUpdate()
        {
            if (CardAmount < 1 || !player.Simulated())
            {
                return;
            }

            counter -= TimeHandler.fixedDeltaTime;

            // order visible enemies that are alive by their distance from the player
            Player[] possibleTargets = visibleEnemies.OrderBy(p => Vector3.Distance(p.transform.position, player.transform.position)).ToArray();

            if (possibleTargets.Length > 0)
            {
                for (int t = 0; t < targets.Length; t++)
                {
                    targets[t] = possibleTargets[t % possibleTargets.Length];
                }

                // draw loop
                for (int i = 0; i < targets.Length; i++)
                {
                    Player target = targets[i];
                    int locks = targets.Count(x => x == target);

                    if (locks > 0)
                    {
                        lasers[i].Width = Megalith.LASER_WIDTH * locks * 1.5f;
                    }
                    lasers[i].Draw(player.transform.position, target.transform.position);
                }

                // damage loop
                if (player.data.view.IsMine && counter < 0f)
                {
                    // reset counter
                    counter = DT;

                    for (int i = 0; i < targets.Length; i++)
                    {
                        Player target = targets[i];

                        float dps = Megalith.DPS_ABS + Megalith.DPS_REL * target.data.maxHealth;

                        target.data.healthHandler.CallTakeDamage(
                            Vector2.up * dps * DT,
                            target.data.transform.position,
                            damagingPlayer: player
                        );
                    }
                }
            }
            else
            {
                // no targets, hide lasers
                lasers.ForEach(l => l.DrawHidden());
            }
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();

            visibleEnemies = PlayerManager.instance.players.Where(
                p => !p.data.dead && p.teamID != player.teamID &&
                PlayerManager.instance.CanSeePlayer(player.data.transform.position, p).canSee
            );

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