using System;
using ModdingUtils.MonoBehaviours;
using UnboundLib;
using UnityEngine;
using ModdingUtils;
using ModdingUtils.RoundsEffects;

namespace Supcom2Cards.MonoBehaviours
{
    public class ClusterBombEffect : HitSurfaceEffect
    {
        public ObjectsToSpawn? Explosion;
        public Player? Player;

        public int Explosions = 0;
        public int FramesBetweenExplosions = 0;
        public int HowMany = 0;
        public int Spread = 0;

        private static readonly System.Random rng = new System.Random() { };

        private void Explode(Vector2 position)
        {
            double radians = rng.NextDouble() * 2 * Math.PI;
            Vector2 random = new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians)) * rng.Next(0, Spread);

            // spawn explosion near bullet hit
            ObjectsToSpawn.SpawnObject(Explosion, position + random, new Quaternion(0, 0, 0, 0));
        }

        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {
            if (Explosions > 0 && Explosion != null && HowMany > 0 && Supcom2.instance != null)
            {
                for (int i = 0; i < Explosions * HowMany; i++)
                {
                    Supcom2.instance.ExecuteAfterFrames(i * FramesBetweenExplosions, GetExplodeAction(position));
                }
            }
        }

        private Action GetExplodeAction(Vector2 position)
        {
            return delegate
            {
                Explode(position);
            };
        }
    }
}
