using ModdingUtils.MonoBehaviours;
using System;
using UnityEngine;

namespace Supcom2Cards
{
    /// <summary>
    /// <para>A MonoBehaviour that can be applied only once per player</para>
    /// <para>Subsequent card picks increment CardAmount to buff the effects</para>
    /// <para>Removing cards decrements CardAmount until 0 where mono is destroyed</para>
    /// </summary>
    public interface ISingletonEffect
    {
        /// <summary>
        /// Integer used to keep track of how many times a player has picked the card with this effect
        /// </summary>
        public int CardAmount { get; set; }
    }

    public static partial class ExtensionMethods
    {
        /// <summary>
        /// (See also: <see cref="ISingletonEffect"/>)
        /// </summary>
        public static T IncrementCardEffect<T>(this Player player) where T : Component, ISingletonEffect
        {
            T mono = player.gameObject.GetComponent<T>();
            if (mono == null)
            {
                mono = player.gameObject.AddComponent<T>();
            }
            mono.CardAmount++;
            return mono;
        }

        /// <summary>
        /// (See also: <see cref="ISingletonEffect"/>)
        /// </summary>
        public static T? DecrementCardEffect<T>(this Player player) where T : Component, ISingletonEffect
        {
            T mono = player.gameObject.GetComponent<T>();
            mono.CardAmount--;

            if (mono.CardAmount < 1)
            {
                UnityEngine.Object.Destroy(mono);
                return null;
            }
            else
            {
                return mono;
            }
        }
    }
}
