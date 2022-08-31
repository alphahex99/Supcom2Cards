#pragma warning disable CS8602 // Dereference of a possibly null reference.

using System;
using UnityEngine;
using ModdingUtils.RoundsEffects;
using System.Collections;

namespace Supcom2Cards.RoundsEffects
{
    public class ClusterBombEffect : HitSurfaceEffect
    {
        public ObjectsToSpawn? Explosion;

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
            Supcom2.instance.StartCoroutine(IDoExplosions(position));
        }

        public IEnumerator IDoExplosions(Vector2 position)
        {
            for (int i = 0; i < Explosions * HowMany; i++)
            {
                Explode(position);
                for (int frame = 0; frame < FramesBetweenExplosions; frame++)
                {
                    yield return null;
                }
            }
        }
    }
}
