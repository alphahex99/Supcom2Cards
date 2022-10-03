#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using ModdingUtils.MonoBehaviours;
using Supcom2Cards.Cards;
using System;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    // Test Scenarios:
    // 1. enemy killed in multiple hits, rank gained
    // 2. enemy killed in one hit, rank gained
    // 3. enemy damaged then suicide, rank gained
    // 4. enemy suicide after killed previous round, no rank
    // 5. enemy suicide after respawned but no damage, no rank
    public class VeterancyEffect : ReversibleEffect
    {
        public int HowMany = 0;

        public readonly static float RankIconsHeight = 2.1f;
        public readonly static float RankIconsWidth = 1.8f;

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

        // space between 2 rank icons
        private readonly static float dx = RankIconsWidth / Veterancy.MAX_KILLS;

        private readonly List<VeterancyRankIcon> rankIcons = new List<VeterancyRankIcon>();

        // keep track of last Player non-selfDamage sources of damage to avoid players suiciding to counter this card
        private readonly LastSourceOfDamageList lastSourcesOfDamage = new LastSourceOfDamageList();

        public override void OnUpdate()
        {
            // visuals
            int count = rankIcons.Count;
            Vector3 position = player.transform.position;
            float x = position.x + RankIconsWidth / 2 - VeterancyRankIcon.Size;
            float y = position.y + RankIconsHeight - 2 * VeterancyRankIcon.Size;
            for (int i = 0; i < count; i++)
            {
                if (i % Veterancy.MAX_KILLS == 0)
                {
                    // new row
                    x -= RankIconsWidth;
                    y += 2 * VeterancyRankIcon.Size;
                }

                x += dx;
                rankIcons[i].Draw(x, y);
            }
        }

        public override void OnStart()
        {
            On.CharacterStatModifiers.DealtDamage += OnDealtDamage;
            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public override void OnOnDestroy()
        {
            On.CharacterStatModifiers.DealtDamage -= OnDealtDamage;
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        private void OnDealtDamage(On.CharacterStatModifiers.orig_DealtDamage orig, CharacterStatModifiers self, Vector2 damage, bool selfDamage, Player damagedPlayer)
        {
            CharacterData? data = (CharacterData)self.GetFieldValue("data");

            if (selfDamage || data == null)
            {
                return;
            }

            lastSourcesOfDamage[damagedPlayer] = data.player;
        }

        private float GetMult() => (1 + Rank * Veterancy.ADD_MULT_PER_KILL);

        private void PlayerDied(Player p, int idk)
        {
            UnityEngine.Debug.Log("player died");

            try
            {
                if (p == player)
                {
                    // owner died, hide ranks
                    rankIcons.ForEach(r => r.DrawHidden());
                    return;
                }

                if (p.teamID != player.teamID && Rank < Veterancy.MAX_KILLS * HowMany && lastSourcesOfDamage[p] == player)
                {
                    Rank++;

                    // reset last source of damage to avoid giving ranks if player kills himself after respawning
                    lastSourcesOfDamage[p] = null;
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
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

    public class LastSourceOfDamageList : Dictionary<Player, Player?>
    {
        /// <returns>lastSourceOfDamage</returns>
        public new Player? this[Player player]
        {
            get
            {
                return ContainsKey(player) ? base[player] : null;
            }

            set
            {
                if (ContainsKey(player))
                {
                    base[player] = value;
                }
                else
                {
                    Add(player, value);
                }
            }
        }
    }
}
