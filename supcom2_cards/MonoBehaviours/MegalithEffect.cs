#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Supcom2Cards.MonoBehaviours
{
    public class MegalithEffect : ReversibleEffect
    {
        private int _howMany = 0;
        public int HowMany
        {
            get { return _howMany; }
            set
            {
                _howMany = value;

                targets = new Player[Megalith.LASERS * _howMany];
                MegalithLaser.SetAmount(megalithLasers, Megalith.LASERS * _howMany);
            }
        }

        private float counter = 1;
        private const float UPDATE_PERIOD = 1 / Megalith.UPS; // time between targetting updates in milliseconds

        private IEnumerable<Player> visibleEnemiesAlive;
        private Player[] targets;

        private readonly List<MegalithLaser> megalithLasers = new List<MegalithLaser>(2);

        public override void OnStart()
        {
            visibleEnemiesAlive = PlayerManager.instance.players.Where(p => PlayerManager.instance.CanSeePlayer(player.data.transform.position, p).canSee).Where(p => !p.data.dead && p.teamID != player.teamID);
        }

        public override void OnUpdate()
        {
            if (!player.data.dead && HowMany > 0)
            {
                counter -= Time.deltaTime;

                // order visible enemies that are alive by their distance from the player
                Player[] possibleTargets = visibleEnemiesAlive.OrderBy(p => Vector3.Distance(p.transform.position, player.transform.position)).ToArray();

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
                    // calculate dps with card stats and UPS adjusted
                    dps = Supcom2.GetGunDPS(gun, gunAmmo) * Megalith.DPS_MULT * UPDATE_PERIOD;
                }

                for (int i = 0; i < lengthTargets; i++)
                {
                    megalithLasers[i].Draw(player.transform.position, targets[i].transform.position);

                    if (dps > 0)
                    {
                        targets[i].data.healthHandler.TakeDamage(Vector2.up * dps, targets[i].data.transform.position);

                        // reset counter
                        counter = UPDATE_PERIOD;
                    }
                }
            }
            else
            {
                // owner died, hide lasers
                megalithLasers.ForEach(l => l.DrawHidden());
            }
        }
    }

    public class MegalithLaser
    {
        public Color Color = Color.red;
        public Material Material = new Material(Shader.Find("UI/Default"));
        public float Width = 0.5f;
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

        public static void SetAmount(List<MegalithLaser> megalithLasers, int amount)
        {
            // overcomplicated but useful for testing

            int count = megalithLasers.Count;
            if (amount > count)
            {
                for (int i = 0; i < amount - count; i++)
                {
                    megalithLasers.Add(new MegalithLaser());
                }
            }
            else if (count > amount)
            {
                for (int i = 0; i < count - amount; i++)
                {
                    megalithLasers.RemoveAt(0);
                }
            }
        }
    }
}