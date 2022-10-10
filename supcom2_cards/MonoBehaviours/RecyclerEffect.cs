#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using System.Linq;

namespace Supcom2Cards.MonoBehaviours
{
    public class RecyclerEffect : MonoBehaviour
    {
        public Player player;

        private float counter = 1;

        private readonly List<Laser> lasers = new List<Laser>();

        public void FixedUpdate()
        {
            counter -= TimeHandler.deltaTime;
            if (counter <= 0)
            {
                // damage enemies
                float stolenHP = 0;
                foreach (Player p in PlayerManager.instance.players)
                {
                    if (!p.data.dead && p.teamID != player.teamID)
                    {
                        float dmg = p.data.maxHealth * Recycler.DPS_HP_PERCENT / 100f;
                        p.TakeDamage(dmg, player);
                        stolenHP += dmg;
                    }
                }
                // heal owner
                player.data.healthHandler.Heal(stolenHP);

                // reset counter
                counter = 1;
            }

            int i = 0;
            foreach (Player enemy in PlayerManager.instance.players.Where(p => !p.data.dead && p.teamID != player.teamID))
            {
                lasers[i++].Draw(player.transform.position, enemy.transform.position);
            }
            while (i < lasers.Count)
            {
                lasers[i++].DrawHidden();
            }
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();

            lasers.SetListCount(PlayerManager.instance.players.Count);
            foreach (Laser laser in lasers)
            {
                laser.Color = Color.yellow;
                laser.Width = 0.05f;
            }

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