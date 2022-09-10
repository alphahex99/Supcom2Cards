#pragma warning disable CS8602 // Dereference of a possibly null reference.

using UnityEngine;
using ModdingUtils.RoundsEffects;
using System.Collections;
using Supcom2Cards.Cards;

namespace Supcom2Cards.RoundsEffects
{
    public class ClusterBombEffect : HitSurfaceEffect
    {
        public ObjectsToSpawn? Explosion;

        public int HowMany = 0;

        private static readonly System.Random rng = new System.Random() { };

        private void Explode(Vector2 position)
        {
            Vector2 random = UnityEngine.Random.insideUnitCircle.normalized * rng.Next(0, ClusterBomb.SPREAD);

            // spawn explosion near bullet hit
            ObjectsToSpawn.SpawnObject(Explosion, position + random, new Quaternion(0, 0, 0, 0));
        }

        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {
            Supcom2.instance.StartCoroutine(IDoExplosions(position));
        }

        public IEnumerator IDoExplosions(Vector2 position)
        {
            for (int i = 0; i < ClusterBomb.EXPLOSIONS * HowMany; i++)
            {
                Explode(position);

                int delay = rng.Next(ClusterBomb.FRAMES_MIN, ClusterBomb.FRAMES_MAX);
                for (int frame = 0; frame < delay; frame++)
                {
                    yield return null;
                }
            }
        }
    }
}
