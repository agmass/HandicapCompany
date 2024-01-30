using System;
using HarmonyLib;
using HandicapCompany;

namespace HandicapCompany.patches {

    [HarmonyPatch(typeof(IngamePlayerSettings))]
    internal class IngamePlayerSettingsPatch {

        [HarmonyPatch(typeof(IngamePlayerSettings), "SaveSettingsToPrefs")]
        [HarmonyPrefix]
        public static bool gammaPatch2(IngamePlayerSettings __instance) {
            if (Plugin.Instance.blind && Plugin.Instance.mute && Plugin.Instance.palsy && Plugin.Instance.deaf) {
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(IngamePlayerSettings), "UpdateGameToMatchSettings")]
        [HarmonyPostfix]
        public static void gammaPatch(IngamePlayerSettings __instance) {
            if (Plugin.Instance.blind) {
                __instance.ChangeGamma(-95);
            }
            if (Plugin.Instance.mute) {
                __instance.settings.micEnabled = false;
            }
            if (Plugin.Instance.palsy) {
                __instance.settings.lookSensitivity = -Plugin.Instance.ogSense;
            }
            if (Plugin.Instance.deaf) {
                __instance.ChangeMasterVolume(0);
            }
        }
    }

}