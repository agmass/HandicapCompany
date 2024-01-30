using System;
using HarmonyLib;
using HandicapCompany;
using GameNetcodeStuff;
using UnityEngine;
using System.Collections;
using System.Threading;

namespace HandicapCompany.patches {

    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPach {

        [HarmonyPatch("ParsePlayerSentence")]
		[HarmonyPrefix]
		public static void ParsePlayerSentence(ref Terminal __instance)
		{
            if (Plugin.Instance.illiterate) {
			    __instance.screenText.text = "";
            }
		}

    }

}