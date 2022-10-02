#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class VeterancyEffect : ReversibleEffect
    {
        public int HowMany = 0;

        private int rank = 0;
        public int Rank
        {
            get { return rank; }

            set
            {
                rank = value;

                // update buffs
                ClearModifiers();
                gunStatModifier.damage_mult = GetMult();
                characterDataModifier.maxHealth_mult = GetMult();
                ApplyModifiers();

                // heal to adjust for new max health
                player.data.health *= GetMult();

                rankIcons.SetAmount(rank);
            }
        }

        // for some reason PlayerDied gets run twice when somebody dies so kills are doubled
        private int killsX2 = 0;

        private readonly VeterancyRankIcons rankIcons = new VeterancyRankIcons();

        private float GetMult() => (1 + rank * Veterancy.ADD_MULT_PER_KILL);

        private void PlayerDied(Player p, int idk)
        {
            if (p.teamID != player.teamID && p.data.lastSourceOfDamage == player && rank < Veterancy.MAX_KILLS * HowMany)
            {
                killsX2++;
                return;
            }
            if (p == player)
            {
                // owner died, hide ranks
                rankIcons.Draw(new Vector3(100, 100, 0));
            }
        }

        public override void OnUpdate()
        {
            if (killsX2 >= 2)
            {
                Rank++;
                killsX2 -= 2;
            }

            rankIcons.Draw(player.transform.position);
        }

        public override void OnStart()
        {
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public override void OnOnDestroy()
        {
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }
    }

    public class VeterancyRankIcons
    {
        public Color Color = Color.yellow;
        public float Height = 2.1f;
        public Material Material = new Material(Shader.Find("UI/Default"));
        public float Width = 1.8f;

        private readonly List<VeterancyRankIcon> rankIcons = new List<VeterancyRankIcon>();

        public void SetAmount(int amount)
        {
            // overcomplicated but useful for testing

            int count = rankIcons.Count;
            if (amount > count)
            {
                for (int i = 0; i < amount - count; i++)
                {
                    rankIcons.Add(new VeterancyRankIcon(Color, Material));
                }
            }
            else if (count > amount)
            {
                for (int i = 0; i < count - amount; i++)
                {
                    rankIcons.RemoveAt(0);
                }
            }
        }

        public void Draw(Vector3 playerTransformPosition)
        {
            float y = playerTransformPosition.y + Height;
            switch(rankIcons.Count)
            {
                case 1:
                    rankIcons[0].Draw(playerTransformPosition.x, y);
                    break;
                case 2:
                    rankIcons[0].Draw(playerTransformPosition.x - Width / 4f, y);
                    rankIcons[1].Draw(playerTransformPosition.x + Width / 4f, y);
                    break;
                default:
                    float xStart = playerTransformPosition.x - Width / 2f;
                    float xEnd = playerTransformPosition.x + Width / 2f;
                    float count = rankIcons.Count - 1;

                    float x;
                    for (int i = 0; i < rankIcons.Count; i++)
                    {
                        x = (xStart + ((xEnd - xStart) / count) * i);

                        rankIcons[i].Draw(x, y);
                    }
                    break;
            }
        }
    }

    public class VeterancyRankIcon
    {
        public Color Color;
        public float Size = 0.2f;
        public float Width = 0.15f;
        public float Z = -5;

        private readonly LineRenderer lineL;
        private readonly Vector3[] cordsL = new Vector3[2];

        private readonly LineRenderer lineR;
        private readonly Vector3[] cordsR = new Vector3[2];

        public VeterancyRankIcon(Color color, Material material)
        {
            Color = color;

            lineL = new GameObject().AddComponent<LineRenderer>();
            lineL.name = "VeterancyIconLine";
            lineL.startWidth = Width;
            lineL.endWidth = Width;
            lineL.startColor = Color.white;
            lineL.endColor = Color.white;
            lineL.material = material;
            lineL.material.color = Color;
            lineL.useWorldSpace = true;

            lineR = new GameObject().AddComponent<LineRenderer>();
            lineR.name = "VeterancyIconLine";
            lineR.startWidth = Width;
            lineR.endWidth = Width;
            lineR.startColor = Color.white;
            lineR.endColor = Color.white;
            lineR.material = material;
            lineR.material.color = Color;
            lineR.useWorldSpace = true;

            cordsL[0].z = Z;
            cordsL[1].z = Z;
            cordsR[0].z = Z;
            cordsR[1].z = Z;
        }

        ~VeterancyRankIcon()
        {
            GameObject.Destroy(lineL);
            GameObject.Destroy(lineR);
        }

        public void Draw(float x, float y)
        {
            cordsL[0].x = x - Size;
            cordsL[0].y = y + Size;
            cordsL[1].x = x;
            cordsL[1].y = y;

            cordsR[0].x = x + Size;
            cordsR[0].y = y + Size;
            cordsR[1].x = x;
            cordsR[1].y = y;

            lineL.SetPositions(cordsL);
            lineR.SetPositions(cordsR);
        }
    }
}
