#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Supcom2Cards.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class DarkenoidEffect : MonoBehaviour, ISingletonEffect
    {
        private int _cardAmount = 0;
        public int CardAmount
        {
            get { return _cardAmount; }
            set
            {
                _cardAmount = value;

                int count = Darkenoid.BEAM_COUNT * _cardAmount;

                lasers.SetListCount(count);

                int count2 = count / 2;
                bool countIsEven = (count % 2 == 0);

                for (int i = 0; i < count; i++)
                {
                    Laser laser = lasers[i];

                    // make the inside of the beam White and the edges Cyan
                    Color color;
                    if (countIsEven)
                    {
                        if (i == count2 + 1 || i == count2 - 1)
                        {
                            color = Color.white;
                        }
                        else
                        {
                            color = Color.cyan;
                        }
                    }
                    else
                    {
                        color = (i == count2) ? Color.white : Color.cyan;
                    }

                    //laser.Color = player.GetTeamColors().color; TODO: wrong material? Cast32?
                    laser.Color = color;
                    laser.Width = 0.05f;
                }
            }
        }

        public int beam_width = 1;

        public Player player;

        private float counter = 0;
        private const float DT = 1 / Darkenoid.UPS;

        private readonly List<Laser> lasers = new List<Laser>(Darkenoid.BEAM_COUNT);

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();

            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public void OnDestroy()
        {
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        public void FixedUpdate()
        {
            if (CardAmount < 1)
            {
                return;
            }

            counter -= TimeHandler.deltaTime;

            Draw();

            if (counter <= 0)
            {
                Damage();
                counter = DT;
            }
        }

        private void Draw()
        {

        }

        private void Damage()
        {

        }

        private void PlayerDied(Player p, int idk)
        {
            if (p == player)
            {
                // owner died, hide drones
                foreach (Laser laser in lasers)
                {
                    laser.DrawHidden();
                }
            }
        }
    }
}
