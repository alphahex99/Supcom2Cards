#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable IDE0059 // Unnecessary assignment of a value

using System;
using UnboundLib;

namespace Supcom2Cards
{
    public static class ExtensionMethods
    {
        public static void RemovePlayerDiedAction(this PlayerManager pm, Action<Player, int> listener)
        {
            Action<Player, int> action = (Action<Player, int>)pm.GetFieldValue("PlayerDiedAction");
            action -= listener;
        }
    }
}
