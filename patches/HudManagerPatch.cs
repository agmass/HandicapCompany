using System;
using HarmonyLib;
using HandicapCompany;
using GameNetcodeStuff;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace HandicapCompany.patches {

    [HarmonyPatch(typeof(HUDManager))]
    internal class HUDManagerPatch {

        
        [HarmonyPatch("Update")]
		[HarmonyPostfix]
        public static void FadeToNothing(ref HUDManager __instance)
		{
            if (Plugin.Instance.illiterate) {
                __instance.HideHUD(true);
            }
		}


        [HarmonyPatch("UpdateHealthUI")]
		[HarmonyPrefix]
        public static bool FadeToNothing3(ref HUDManager __instance)
		{
            if (Plugin.Instance.illiterate) {
                return false;
            }
            return true;
		}

        [HarmonyPatch("PingHUDElement")]
		[HarmonyPrefix]
        public static bool FadeToNothing2(ref HUDManager __instance)
		{
            if (Plugin.Instance.illiterate) {
                return false;
            }
            return true;
		}


    }

}