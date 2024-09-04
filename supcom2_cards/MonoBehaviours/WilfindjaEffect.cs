#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using Supcom2Cards.Cards;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Supcom2Cards.MonoBehaviours
{
    public class WilfindjaEffect : MonoBehaviour, ISingletonEffect
    {
        private int _cardAmount = 0;
        public int CardAmount
        {
            get { return _cardAmount; }
            set
            {
                _cardAmount = value;

                drones.SetListCount(Wilfindja.DRONES * _cardAmount);

                double a = 6.283185307179586476925286766559d / Wilfindja.DRONES; // angle between 2 drones in radians
                for (int i = 0; i < drones.Count; i++)
                {
                    drones[i].angle = i * a;
                }

                lasers.SetListCount(Wilfindja.DRONE_EDGES * drones.Count);

                foreach (Laser laser in lasers)
                {
                    //laser.Color = player.GetTeamColors().color; TODO: wrong material? Cast32?
                    laser.Color = Color.cyan;
                    laser.Width = 0.075f;
                }
            }
        }

        public Player player;
        public Gun gun;

        private float counter = 0;
        private const float DT = 1 / Wilfindja.UPS;

        private float spin = 0;
        private readonly List<Drone> drones = new List<Drone>(Wilfindja.DRONES);
        private readonly List<Laser> lasers = new List<Laser>(Wilfindja.DRONES * Wilfindja.DRONE_EDGES);

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            gun = player.data.weaponHandler.gun;

            PlayerManager.instance.AddPlayerDiedAction(PlayerDied);
        }

        public void OnDestroy()
        {
            PlayerManager.instance.RemovePlayerDiedAction(PlayerDied);
        }

        public void FixedUpdate()
        {
            if (CardAmount < 1 || !player.Simulated())
            {
                return;
            }

            counter -= TimeHandler.fixedDeltaTime;

            spin += TimeHandler.fixedDeltaTime * Wilfindja.RPM;
            if (spin > 60f)
            {
                spin -= 60f;
            }

            Draw();

            if (player.data.view.IsMine && counter < 0)
            {
                Damage();
                counter = DT;
            }
        }

        private void Draw()
        {
            double dphi = spin * 0.10471975511965977461542144610932d; // dphi = counter * 2pi / 60
            for (int i = 0; i < drones.Count; i++)
            {
                int layer = i / Wilfindja.DRONES;
                float layerAmp = GetLayerAmplitude(layer);

                double angle = drones[i].angle + dphi * (layer * 0.25d + 1d);

                float x = player.transform.position.x + layerAmp * (float)Math.Cos(angle);
                float y = player.transform.position.y + layerAmp * (float)Math.Sin(angle);

                drones[i].Draw(x, y, lasers.GetRange(i * Wilfindja.DRONE_EDGES, Wilfindja.DRONE_EDGES), angle);
            }
        }

        private void Damage()
        {
            var enemies = PlayerManager.instance.players.Where(p => !p.data.dead && p.teamID != player.teamID);
            foreach (Player enemy in enemies)
            {
                float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

                // go through all layers if card is stacked
                for (int i = 0; i < CardAmount; i++)
                {
                    float layer = GetLayerAmplitude(i);

                    float min = layer - 0.5f * Wilfindja.DRONE_SIZE * Wilfindja.DRONE_HITBOX;
                    float max = layer + 0.5f * Wilfindja.DRONE_SIZE * Wilfindja.DRONE_HITBOX;

                    if (distance > min && distance < max)
                    {
                        // 55 = default gun damage; gun.damage = multiplier (default = 1f)
                        float dps = Wilfindja.DPS_REL * 55f * gun.damage;

                        enemy.data.healthHandler.CallTakeDamage(
                            Vector2.up * dps * DT,
                            enemy.data.transform.position,
                            damagingPlayer: player
                        );
                    }
                }
            }
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

        private float GetLayerAmplitude(int layer)
        {
            return Wilfindja.DRONE_DISTANCE * (layer * 0.5f + 1f);
        }

        private class Drone
        {
            public double angle = 0d;

            // for some fucking reason Laser[] and List<Laser> don't generate unless created in WilfindjaEffect
            //private readonly List<Laser> lasers = new List<Laser>(3);

            // angle between 2 points
            private readonly static double a = 6.283185307179586476925286766559d / Wilfindja.DRONE_EDGES;

            public void Draw(float x, float y, List<Laser> lasers, double angle = 0d)
            {
                float x1;
                float y1;
                float x2 = x + Wilfindja.DRONE_SIZE * (float)Math.Cos((lasers.Count - 1) * a - angle * Wilfindja.DRONE_RPM_MULT);
                float y2 = y + Wilfindja.DRONE_SIZE * (float)Math.Sin((lasers.Count - 1) * a - angle * Wilfindja.DRONE_RPM_MULT);

                for (int i = 0; i < lasers.Count; i++)
                {
                    x1 = x2;
                    y1 = y2;

                    double b = i * a - angle * Wilfindja.DRONE_RPM_MULT;
                    x2 = x + Wilfindja.DRONE_SIZE * (float)Math.Cos(b);
                    y2 = y + Wilfindja.DRONE_SIZE * (float)Math.Sin(b);

                    lasers[i].Draw(x1, y1, x2, y2);
                }
            }
        }
    }
}