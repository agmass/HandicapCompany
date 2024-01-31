using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using UnityEngine.AI;

namespace HandicapCompany.patches {
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch {

        [HarmonyPatch(typeof(RoundManager), "OnDestroy")]
        [HarmonyPostfix]
        static void undo(RoundManager __instance) {
            Plugin.Instance.mute = false;
            Plugin.Instance.deaf = false;
            if (Plugin.Instance.palsy) {
                IngamePlayerSettings.Instance.settings.lookSensitivity = Plugin.Instance.ogSense;
            }
            Plugin.Instance.palsy = false;
            Plugin.Instance.blind = false;
            Plugin.Instance.crippled = false;
            Plugin.Instance.introvert = false;
            Plugin.Instance.illiterate = false;
            Plugin.Instance.weak = false;
            Plugin.Instance.weighty = false;
            Plugin.Instance.extrovert = false;
            Plugin.Instance.paranoid = false;
            Plugin.Instance.conductive = false;
            GameNetworkManager.Instance.localPlayerController.carryWeight = 1f;
            HUDManager.Instance.HideHUD(false);
            IngamePlayerSettings.Instance.LoadSettingsFromPrefs();
            IngamePlayerSettings.Instance.UpdateGameToMatchSettings();
        }

        [HarmonyPatch(typeof(RoundManager), "GenerateNewLevelClientRpc")]
        [HarmonyPostfix]
        static void gammaPatch(RoundManager __instance) {
            if (!Plugin.Instance.blind && !Plugin.Instance.mute && !Plugin.Instance.deaf && !Plugin.Instance.palsy && !Plugin.Instance.crippled && !Plugin.Instance.illiterate && !Plugin.Instance.introvert && !Plugin.Instance.weak && !Plugin.Instance.weighty && !Plugin.Instance.conductive && !Plugin.Instance.extrovert) {
            Plugin.Instance.reloadConfig();
            if ((100-Plugin.Instance.handicapChance.Value) < new System.Random().Next(0,101)) {
            int disability = Plugin.Instance.available[new System.Random().Next(0,Plugin.Instance.available.Count)];
            switch(disability) {
                case 0:
                    IngamePlayerSettings.Instance.ChangeGamma(-95);
                    Plugin.Instance.blind = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Blind", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're blind!\nYou can still see, but not that well... Good luck!", true);
                    break;
                case 1:
                    IngamePlayerSettings.Instance.settings.micEnabled = false;
                    Plugin.Instance.mute = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Mute", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're mute!\nYou watch on in silence.", true);
                    break;
                case 2:
                    Plugin.Instance.ogSense = IngamePlayerSettings.Instance.settings.lookSensitivity;
                    IngamePlayerSettings.Instance.settings.lookSensitivity = -IngamePlayerSettings.Instance.settings.lookSensitivity;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Palsy", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    Plugin.Instance.palsy = true;
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're palsy!\n!yaw gnorW", true);
                    break;
                case 3:
                    IngamePlayerSettings.Instance.ChangeMasterVolume(0);
                    Plugin.Instance.deaf = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Deaf", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're deaf!\nI would put text here, but you can't hear it.", true);
                    break;
                case 4:
                    Plugin.Instance.crippled = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Broken Legs", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You have broken legs!\nYou're always at injured speeds, and are also at 15hp. Good luck.\nWARNING: IF YOU ARE NOT HOST, YOU CANNOT MOVE NORMALLY.", true);
                    break;
                case 5:
                    Plugin.Instance.illiterate = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Illiterate", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Illiterate!\nfsfhdjsfsjdfjskfhsfshjdfhjksshjfjk", true);
                    break;
                case 6:
                    Plugin.Instance.introvert = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Introvert", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're an Introvert!\nYou get more paranoid when you're with people..\nDon't get too paranoid or you'll take damage!", true);
                    break;
                case 7:
                    Plugin.Instance.weighty = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Weighty", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Weighty!\nYou naturally gained some weight.", true);
                    break;
                case 8:
                    Plugin.Instance.weak = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Weak", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Weak\nYou have less health and take 1hp/0.5s when holding 2-handed items.", true);
                    break;
                case 9:
                    Plugin.Instance.paranoid = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Paranoid", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Paranoid\nFear damages you.\nYou also seem to get scared by seemingly nothing..?", true);
                    break;
                case 10:
                    Plugin.Instance.conductive = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Conductive", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Conductive\nFor the love of god, don't take metal.", true);
                    break;
                case 11:
                    Plugin.Instance.extrovert = true;
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: Extrovert", (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're an Extrovert!\nYou get more paranoid when you're not with people..\nDon't get too paranoid or you'll take damage!", true);
                    break;
                }
            } else {
                HUDManager.Instance.DisplayTip("Handicap Company", "You're not handicapped!\nYou got lucky, atleast for now...", false);
            }
            }
        }
    }
    
}