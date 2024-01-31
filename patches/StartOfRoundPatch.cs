using HarmonyLib;
using Unity.Netcode;
using HandicapCompany;
using System;
using GameNetcodeStuff;

namespace HandicapCompany.patches {
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch {

        [HarmonyPatch(typeof(StartOfRound), "EndOfGame")]
        [HarmonyPostfix]
        static void undo(StartOfRound __instance) {
            Plugin.Instance.mute = false;
            Plugin.Instance.deaf = false;
            if (Plugin.Instance.palsy) {
                IngamePlayerSettings.Instance.settings.lookSensitivity = Plugin.Instance.ogSense;
            }
            Plugin.Instance.palsy = false;
            Plugin.Instance.blind = false;
            Plugin.Instance.crippled = false;
            Plugin.Instance.illiterate = false;
            Plugin.Instance.introvert = false;
            Plugin.Instance.weak = false;
            Plugin.Instance.paranoid = false;
            Plugin.Instance.extrovert = false;
            Plugin.Instance.weighty = false;
            Plugin.Instance.conductive = false;
            GameNetworkManager.Instance.localPlayerController.carryWeight = 1f;
            HUDManager.Instance.HideHUD(false);
            IngamePlayerSettings.Instance.LoadSettingsFromPrefs();
            IngamePlayerSettings.Instance.UpdateGameToMatchSettings();
        }
  
    }
}