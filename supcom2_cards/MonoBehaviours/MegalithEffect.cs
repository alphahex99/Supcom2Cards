#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Supcom2Cards.MonoBehaviours
{
    public class MegalithEffect : MonoBehaviour
    {
        private int _howMany = 0;
        public int HowMany
        {
            get { return _howMany; }
            set
            {
                _howMany = value;

                targets = new Player[Megalith.LASERS * _howMany];
                lasers.SetListCount(Megalith.LASERS * _howMany);
            }
        }

        public Player player;
        public Block block;

        private float counter = 1;
        private const float DT = 1 / Megalith.UPS;

        private IEnumerable<Player> visibleEnemies;
        private Player[] targets;

        private readonly List<MegalithLaser> lasers = new List<MegalithLaser>(2);

        public void FixedUpdate()
        {
            if (HowMany > 0)
            {
                counter -= Time.deltaTime;

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
                        dps = player.data.maxHealth * Megalith.DPS_MULT;
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

    public class MegalithLaser
    {
        public Color Color = Color.red;
        public Material Material = new Material(Shader.Find("UI/Default"));
        public float Width = 0.35f;
        public float Z = -5;

        private readonly LineRenderer line;
        private readonly Vector3[] cords = new Vector3[2];

        private static int id = 0;

        public MegalithLaser()
        {
            line = new GameObject().AddComponent<LineRenderer>();
            line.name = $"MagnetronLaserLine_{id++}";
            line.startWidth = Width;
            line.endWidth = Width;
            line.startColor = Color.white;
            line.endColor = Color.white;
            line.material = Material;
            line.material.color = Color;
            line.useWorldSpace = true;

            cords[0].z = Z;
            cords[1].z = Z;
        }

        ~MegalithLaser()
        {
            GameObject.Destroy(line);
        }

        public void Draw(float x1, float y1, float x2, float y2)
        {
            cords[0].x = x1;
            cords[0].y = y1;
            cords[1].x = x2;
            cords[1].y = y2;

            line.SetPositions(cords);
        }

        public void Draw(Vector3 a, Vector3 b)
        {
            Draw(a.x, a.y, b.x, b.y);
        }

        public void DrawHidden()
        {
            Draw(100, 100, 100, 100);
        }
    }
}