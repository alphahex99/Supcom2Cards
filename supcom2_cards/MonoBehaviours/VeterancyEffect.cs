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

        public static float RankIconsHeight = 2.1f;
        public static float RankIconsWidth = 1.8f;

        private int _rank = 0;
        public int Rank
        {
            get { return _rank; }

            set
            {
                _rank = value;

                // update buffs
                ClearModifiers();
                gunStatModifier.damage_mult = GetMult();
                characterDataModifier.maxHealth_mult = GetMult();
                ApplyModifiers();

                // heal to adjust for new max health
                player.data.health *= GetMult();

                rankIcons.SetListCount(_rank);
            }
        }

        // for some reason PlayerDied gets run twice when somebody dies so kills are doubled
        private int killsX2 = 0;

        private readonly List<VeterancyRankIcon> rankIcons = new List<VeterancyRankIcon>();

        public override void OnUpdate()
        {
            if (killsX2 >= 2)
            {
                Rank++;
                killsX2 -= 2;
            }

            Vector3 position = player.transform.position;
            float y = position.y + RankIconsHeight;
            switch (rankIcons.Count)
            {
                case 1:
                    rankIcons[0].Draw(position.x, y);
                    break;
                case 2:
                    rankIcons[0].Draw(position.x - RankIconsWidth / 4f, y);
                    rankIcons[1].Draw(position.x + RankIconsWidth / 4f, y);
                    break;
                default:
                    float xStart = position.x - RankIconsWidth / 2f;
                    float xEnd = position.x + RankIconsWidth / 2f;
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

        public override void OnStart()
        {
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public override void OnOnDestroy()
        {
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        private float GetMult() => (1 + Rank * Veterancy.ADD_MULT_PER_KILL);

        private void PlayerDied(Player p, int idk)
        {
            if (p.teamID != player.teamID && p.data.lastSourceOfDamage == player && Rank < Veterancy.MAX_KILLS * HowMany)
            {
                killsX2++;
                return;
            }
            if (p == player)
            {
                // owner died, hide ranks
                rankIcons.ForEach(r => r.DrawHidden());
            }
        }
    }

    public class VeterancyRankIcon
    {
        public static Color Color = Color.yellow;
        public static Material Material = new Material(Shader.Find("UI/Default"));
        public static float Size = 0.2f;
        public static float Width = 0.15f;
        public static float Z = -5;

        private readonly LineRenderer lineL;
        private readonly Vector3[] cordsL = new Vector3[2];

        private readonly LineRenderer lineR;
        private readonly Vector3[] cordsR = new Vector3[2];

        private static int id = 0;

        public VeterancyRankIcon()
        {
            lineL = new GameObject().AddComponent<LineRenderer>();
            lineL.name = $"VeterancyIconLine_{id}L";
            lineL.startWidth = Width;
            lineL.endWidth = Width;
            lineL.startColor = Color.white;
            lineL.endColor = Color.white;
            lineL.material = Material;
            lineL.material.color = Color;
            lineL.useWorldSpace = true;

            lineR = new GameObject().AddComponent<LineRenderer>();
            lineR.name = $"VeterancyIconLine_{id++}R";
            lineR.startWidth = Width;
            lineR.endWidth = Width;
            lineR.startColor = Color.white;
            lineR.endColor = Color.white;
            lineR.material = Material;
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

        public void DrawHidden()
        {
            cordsL[0].x = 100;
            cordsL[0].y = 100;
            cordsL[1].x = 100;
            cordsL[1].y = 100;

            cordsR[0].x = 100;
            cordsR[0].y = 100;
            cordsR[1].x = 100;
            cordsR[1].y = 100;

            lineL.SetPositions(cordsL);
            lineR.SetPositions(cordsR);
        }
    }
}
