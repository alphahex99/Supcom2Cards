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

        private void Explode(Vector2 position)
        {
            Vector2 random = UnityEngine.Random.insideUnitCircle.normalized * Supcom2.RNG.Next(0, ClusterBomb.EXPLOSION_SPREAD);

            // spawn explosion near bullet hit
            ObjectsToSpawn.SpawnObject(Explosion, position + random, new Quaternion(0, 0, 0, 0));
        }

        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {
            Supcom2.instance.StartCoroutine(IDoExplosions(position));
        }

        public IEnumerator IDoExplosions(Vector2 position)
        {
            for (int i = 0; i < ClusterBomb.EXPLOSION_AMOUNT * HowMany; i++)
            {
                Explode(position);

                int delay = Supcom2.RNG.Next(ClusterBomb.FRAMES_MIN, ClusterBomb.FRAMES_MAX);
                for (int frame = 0; frame < delay; frame++)
                {
                    yield return null;
                }
            }
        }
    }
}
