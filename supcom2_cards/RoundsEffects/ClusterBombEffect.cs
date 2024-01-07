#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;
using ModdingUtils.RoundsEffects;
using System.Collections;
using Supcom2Cards.Cards;
using System;
using Sonigon;

namespace Supcom2Cards.RoundsEffects
{
    public class ClusterBombEffect : HitSurfaceEffect, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Explosion Explosion;
        public ObjectsToSpawn ExplosionToSpawn;

        public Gun gun;
        public static SoundEvent sound;

        private void Explode(Vector2 position)
        {
            double radians = RNG.NextDouble(0, 2 * Math.PI);
            Vector2 random = new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians)) * RNG.NextInt(0, ClusterBomb.EXPLOSION_SPREAD);

            // spawn explosion near bullet hit
            GameObject ex = Instantiate(ExplosionToSpawn.effect, position + random, Quaternion.identity);

            // delete explosion after 2s
            Destroy(ex, 2); // TODO: Doesn't work

            // sound
            SoundManager.Instance.Play(sound, transform); // TODO: new transform from Vector2 position?
        }

        public override void Hit(Vector2 position, Vector2 normal, Vector2 velocity)
        {
            Explosion.damage = ClusterBomb.EXPLOSION_DMG_MULT * gun.damage;
            Supcom2.Instance.StartCoroutine(IDoExplosions(position));
        }

        public IEnumerator IDoExplosions(Vector2 position)
        {
            for (int i = 0; i < ClusterBomb.EXPLOSION_AMOUNT * CardAmount; i++)
            {
                Explode(position);

                int delay = (int)RNG.NextFloat(1, ClusterBomb.FRAMES_MAX); // TODO: random delay
                for (int frame = 0; frame < delay; frame++)
                {
                    yield return null;
                }
            }
        }
    }
}
