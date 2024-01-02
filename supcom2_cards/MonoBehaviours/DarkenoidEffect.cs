#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

using Supcom2Cards.Cards;
using System.Collections;
using UnboundLib;
using UnityEngine;

namespace Supcom2Cards.MonoBehaviours
{
    public class DarkenoidEffect : MonoBehaviour, ISingletonEffect
    {
        public int CardAmount { get; set; } = 0;

        public Player player;
        public Block block;

        public void Start()
        {
            player = gameObject.GetComponentInParent<Player>();
            block = player.GetComponent<Block>();
            
            block.BlockAction += OnBlock;
            ForceShootDir(new Vector3(0, -1, 0));
        }

        public void OnDestroy()
        {
            block.BlockAction -= OnBlock;

            // remove
            ForceShootDir(new Vector3(0, 0, 0));
        }

        private void OnBlock(BlockTrigger.BlockTriggerType trigger)
        {
            // this reset is necessary because of Radar Shot
            Supcom2.Instance.StartCoroutine(IDoForceShootDir());
            
        }
        public IEnumerator IDoForceShootDir()
        {
            // loop necessary in case of multiple players in Radar Shot range
            for (int i = 0; i < PlayerManager.instance.players.Count; i++)
            {
                // delay to wait for Radar Shot to end
                for (int frame = 0; frame < 12; frame++)
                {
                    yield return null;
                }
                ForceShootDir(new Vector3(0, -1, 0));
            }
        }

        private void ForceShootDir(Vector3 dir)
        {
            // gun.forceShootDir is a private field, this works
            player.data.weaponHandler.gun.SetFieldValue("forceShootDir", dir);
        }
    }
}
