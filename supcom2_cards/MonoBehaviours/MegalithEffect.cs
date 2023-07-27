#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Supcom2Cards.MonoBehaviours
{
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
                }
            }
        }

        public Player player;
        public Block block;

        private float counter = 1;
        private const float DT = 1 / Megalith.UPS;

        private IEnumerable<Player> visibleEnemies;
        private Player[] targets;

        private readonly List<Laser> lasers = new List<Laser>(2);

        public void FixedUpdate()
        {
            if (CardAmount > 0)
            {
                counter -= TimeHandler.deltaTime;

                // order visible enemies that are alive by their distance from the player
                Player[] possibleTargets = visibleEnemies.OrderBy(p => Vector3.Distance(p.transform.position, player.transform.position)).ToArray();

                if (possibleTargets.Length == 0)
                {
                    // no targets, hide lasers
                    lasers.ForEach(l => l.DrawHidden());
                }
                else
                {
                    int lengthTargets = targets.Length;
                    {
                        int lengthPossibleTargets = possibleTargets.Length;
                        for (int i = 0; i < lengthTargets; i++)
                        {
                            targets[i] = possibleTargets[i % lengthPossibleTargets];
                        }
                    }

                    float dps = 0;
                    if (counter <= 0)
                    {
                        dps = Megalith.DPS_ABS + player.data.maxHealth * Megalith.DPS_REL;
                    }

                    for (int i = 0; i < lengthTargets; i++)
                    {
                        Player target = targets[i];

                        lasers[i].Draw(player.transform.position, target.transform.position);

                        if (dps > 0)
                        {
                            target.data.healthHandler.TakeDamage(Vector2.up * dps * DT, target.data.transform.position, damagingPlayer: player);

                            // reset counter
                            counter = DT;
                        }
                    }
                }
            }
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();

            visibleEnemies = PlayerManager.instance.players.Where(p => !p.data.dead && p.teamID != player.teamID && PlayerManager.instance.CanSeePlayer(player.data.transform.position, p).canSee);

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