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

                drones.ForEach(d => d.Size = Wilfindja.DRONE_SIZE);

                lasers.SetListCount(Wilfindja.DRONE_EDGES * drones.Count);

                lasers.ForEach(l => l.Width = 0.075f);
                lasers.SetTeamColor(gameObject, 1.5f);
            }
        }

        public Player player;
        public Gun gun;

        private float counter = 0;
        private const float DT = 1 / Wilfindja.UPS;

        private float spin = 0;
        private readonly List<Polygon> drones = new List<Polygon>(Wilfindja.DRONES);
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
            if (CardAmount < 1)
            {
                return;
            }
            if (!player.Simulated())
            {
                lasers.ForEach(l => l.DrawHidden());
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
            float dphi = spin * 0.10471975511965977461542144610932f; // dphi = spin * 2pi / 60

            float angleSum = 0f;

            int layerLast = 0;
            for (int i = 0; i < drones.Count; i++)
            {
                int layer = i / Wilfindja.DRONES;
                if (layer != layerLast)
                {
                    angleSum = 0f;
                    layerLast = layer;
                }

                float layerAmp = GetLayerAmplitude(layer);

                angleSum += 6.283185307179586476925286766559f / Wilfindja.DRONES;
                float angle = angleSum + dphi * (layer * 0.25f + 1f);

                float x = player.transform.position.x + layerAmp * Mathf.Cos(angle);
                float y = player.transform.position.y + layerAmp * Mathf.Sin(angle);

                drones[i].Draw(x, y, lasers.GetRange(i * Wilfindja.DRONE_EDGES, Wilfindja.DRONE_EDGES), angle * Wilfindja.DRONE_RPM_MULT);
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
                lasers.ForEach(l => l.DrawHidden());
            }
        }

        private float GetLayerAmplitude(int layer)
        {
            return Wilfindja.DRONE_DISTANCE * (layer * 0.5f + 1f);
        }
    }
}