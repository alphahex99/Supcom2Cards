#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Sonigon;
using Supcom2Cards.Cards;
using System;
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

        private float yMax = -1000f;

        public void FixedUpdate()
        {
            float y = player.transform.position.y;

            yMax = y > yMax ? y : yMax;
        }

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();

            player.data.TouchGroundAction += OnTouchGround;

            player.data.TouchWallAction += OnTouchWall;

            sound = player.data.playerSounds.soundCharacterLandBig;
        }

        public void OnTouchGround(float sinceGrounded, Vector3 pos, Vector3 groundNormal, Transform groundTransform)
        {
            float height = yMax - player.transform.position.y;
            height = Mathf.Clamp(height, 0, Cybranasaurus.JUMP_MAX);

            if (height < Cybranasaurus.JUMP_MIN)
            {
                return;
            }

            float jumpMult = height / Cybranasaurus.JUMP_MAX;

            float jumpDmg = player.data.health * Cybranasaurus.HP_DMG_MULT * jumpMult;

            // damage
            if (player.data.view.IsMine)
            {
                Damage(jumpDmg);
            }

            // play sound
            //SoundManager.Instance.Play(sound, transform, new SoundParameterIntensity(jumpDmg * 100f));

            // reset
            yMax = player.transform.position.y;
        }

        private void Damage(float jumpDmg)
        {
            foreach (Player enemy in player.VisibleEnemies())
            {
                float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
                if (distance > Cybranasaurus.DISTANCE_MAX)
                {
                    // too far
                    continue;
                }

                if (distance > Cybranasaurus.DISTANCE_MIN)
                {
                    // quadratic distance falloff
                    jumpDmg *= 1f - Mathf.Pow((distance - Cybranasaurus.DISTANCE_MIN) * divisor, 2);
                }

                enemy.data.healthHandler.CallTakeDamage(Vector2.up * jumpDmg, enemy.data.transform.position, damagingPlayer: player);
            }
        }

        public void OnTouchWall(float sinceWallGrab, Vector3 pos, Vector3 normal)
        {
            yMax = player.transform.position.y;
        }

        public void OnDestroy()
        {
            player.data.TouchGroundAction -= OnTouchGround;
            player.data.TouchWallAction -= OnTouchWall;
        }
    }
}
