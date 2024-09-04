#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Sonigon;
using Supcom2Cards.Cards;
using System;
using System.Linq;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    //TODO: add visuals, maybe explosion?
    public class CybranasaurusEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; }

        public Player player;

        private static readonly float divisor = 1 / (Cybranasaurus.DISTANCE_MAX - Cybranasaurus.DISTANCE_MIN);

        private static SoundEvent sound;

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();

            player.data.TouchGroundAction = (Action<float, Vector3, Vector3, Transform>)Delegate.Combine(
                player.data.TouchGroundAction, new Action<float, Vector3, Vector3, Transform>(TouchGround)
            );

            sound = player.data.playerSounds.soundCharacterLandBig;
        }

        public void TouchGround(float sinceGrounded, Vector3 pos, Vector3 normal, Transform ground)
        {
            // normal jump is 0.57s
            const float minJump = 0.5f;
            if (sinceGrounded < minJump)
            {
                return;
            }

            // cap jump height damage multiplier at 3x
            float jumpMult = Math.Min(sinceGrounded, 3f * minJump) / minJump;

            var visibleEnemies = PlayerManager.instance.players.Where(p => !p.data.dead && p.teamID != player.teamID && PlayerManager.instance.CanSeePlayer(player.data.transform.position, p).canSee);
            foreach (Player enemy in visibleEnemies)
            {
                float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

                float max = Cybranasaurus.DISTANCE_MAX;
                float min = Cybranasaurus.DISTANCE_MIN;

                if (distance > max)
                {
                    // too far
                    continue;
                }

                float damage = Cybranasaurus.HP_DMG_MULT * player.data.maxHealth * CardAmount * jumpMult;

                if (distance > min)
                {
                    // falloff
                    damage *= 1f - Mathf.Pow((distance - min) * divisor, 2);
                }

                enemy.data.healthHandler.TakeDamage(Vector2.up * damage, enemy.data.transform.position, damagingPlayer: player);
            }

            // play sound
            SoundManager.Instance.Play(sound, transform, new SoundParameterIntensity(jumpMult * 100f));
        }

        public void OnDestroy()
        {
            player.data.TouchGroundAction -= TouchGround;
        }
    }
}
