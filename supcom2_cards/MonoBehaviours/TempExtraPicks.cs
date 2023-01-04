#region https://github.com/willis81808/Arcana/blob/main/Scripts/Snippets/TempExtraPicks.cs

using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

public class TempExtraPicks : MonoBehaviour
{
    [SerializeField]
    public int ExtraPicks;

    internal static IEnumerator HandleExtraPicks()
    {
        foreach (Player player in PlayerManager.instance.players.ToArray())
        {
            TempExtraPicks[] extraDrawComponents = player.GetComponentsInChildren<TempExtraPicks>();

            int remainingDraws = extraDrawComponents.Sum(e => e.ExtraPicks);
            if (remainingDraws <= 0) continue;

            extraDrawComponents.Where(e => e.ExtraPicks > 0).First().ExtraPicks -= 1;

            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickStart);
            CardChoiceVisuals.instance.Show(Enumerable.Range(0, PlayerManager.instance.players.Count).Where(i => PlayerManager.instance.players[i].playerID == player.playerID).First(), true);
            yield return CardChoice.instance.DoPick(1, player.playerID, PickerType.Player);
            yield return new WaitForSecondsRealtime(0.1f);
            yield return GameModeManager.TriggerHook(GameModeHooks.HookPlayerPickEnd);
            yield return new WaitForSecondsRealtime(0.1f);
        }
        yield break;
    }
}

#endregion