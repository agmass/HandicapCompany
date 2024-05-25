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
            Plugin.Instance.stopHandicapStacking = false;
            Plugin.Instance.palsy = false;
            Plugin.Instance.blind = false;
            Plugin.Instance.talkative = false;
            Plugin.Instance.drunk = false;
            Plugin.Instance.crippled = false;
            Plugin.Instance.introvert = false;
            Plugin.Instance.illiterate = false;
            Plugin.Instance.weak = false;
            Plugin.Instance.weighty = false;
            Plugin.Instance.extrovert = false;
            Plugin.Instance.paranoid = false;
            Plugin.Instance.hyperactive = false;
            Plugin.Instance.conductive = false;
            GameNetworkManager.Instance.localPlayerController.carryWeight = 1f;
            GameNetworkManager.Instance.localPlayerController.drunkness = 0f;
            HUDManager.Instance.HideHUD(false);
            IngamePlayerSettings.Instance.LoadSettingsFromPrefs();
            IngamePlayerSettings.Instance.UpdateGameToMatchSettings();
        }

        static IEnumerator hyperTip() {
            yield return new WaitForSeconds(4);
            HUDManager.Instance.DisplayTip("Handicap Company", "You have a short grace period until the ship lands.\nGood luck!");
        }

        [HarmonyPatch(typeof(RoundManager), "GenerateNewLevelClientRpc")]
        [HarmonyPostfix]
        static void gammaPatch(RoundManager __instance) {
            if (Plugin.Instance.palsy) {
                IngamePlayerSettings.Instance.settings.lookSensitivity = Plugin.Instance.ogSense;
            }
            if (!Plugin.Instance.stopHandicapStacking) {
            Plugin.Instance.reloadConfig();
            if (StartOfRound.Instance.connectedPlayersAmount <= 0 && !Plugin.Instance.allowMuteWhileSolo.Value || !IngamePlayerSettings.Instance.settings.micEnabled  && !Plugin.Instance.allowMuteWhileMuted.Value) {
                Plugin.Instance.available.Remove(1);
                Plugin.Instance.mls.LogWarning("Removed Mute");
            }
            if (StartOfRound.Instance.connectedPlayersAmount <= 0 && !Plugin.Instance.allowIntroExtroWhileSolo.Value) {
                Plugin.Instance.available.Remove(6);
                Plugin.Instance.available.Remove(11);
                Plugin.Instance.mls.LogWarning("Removed Intro/Extro");
            }
            Plugin.Instance.stopHandicapStacking = true;
            if ((100-Plugin.Instance.handicapChance.Value) < new System.Random().Next(0,101)) {
            int disability = Plugin.Instance.available[new System.Random().Next(0,Plugin.Instance.available.Count)];
            string name = "Lorem ipsum";
            switch(disability) {
                case 0:
                    IngamePlayerSettings.Instance.ChangeGamma(-85);
                    Plugin.Instance.blind = true;
                    name = "Blind";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're blind!\nYou can still see, but not that well... Good luck!", true);
                    break;
                case 1:
                    IngamePlayerSettings.Instance.settings.micEnabled = false;
                    Plugin.Instance.mute = true;
                    name = "Mute";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're mute!\nYou watch on in silence.", true);
                    break;
                case 2:
                    Plugin.Instance.ogSense = IngamePlayerSettings.Instance.settings.lookSensitivity;
                    IngamePlayerSettings.Instance.settings.lookSensitivity = -IngamePlayerSettings.Instance.settings.lookSensitivity;
                    Plugin.Instance.palsy = true;
                    name = "Palsy";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're palsy!\n!yaw gnorW\nDon't be a pussy and stay on ship just because of this. Seriously.\nIt's so easy. I have had like 10 thousand people start\ncrying just because of this. It's not that hard.\nI swear.", true);
                    break;
                case 3:
                    IngamePlayerSettings.Instance.ChangeMasterVolume(0);
                    Plugin.Instance.deaf = true;
                    name = "Deaf";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're deaf!\nI would put text here, but you can't hear it.", true);
                    break;
                case 4:
                    Plugin.Instance.crippled = true;
                    name = "Broken Legs";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You have broken legs!\nYou're always at injured speeds, and are also at 15hp. Good luck.", true);
                    break;
                case 5:
                    __instance.StartCoroutine(illwaiter());
                    name = "Illiterate";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Illiterate!\nYou can't see UI or use Terminal..\nSay goodbye to this tip!", true);
                    break;
                case 6:
                    Plugin.Instance.introvert = true;
                    name = "Introverted";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're an Introvert!\nYou get more paranoid when you're with people..\nDon't get too paranoid or you'll take damage!", true);
                    break;
                case 7:
                    Plugin.Instance.weighty = true;
                    name = "Weighty";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Weighty!\nYou naturally gained some weight.", true);
                    break;
                case 8:
                    Plugin.Instance.weak = true;
                    name = "Weak";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Weak\nYou have less health and take 1hp/0.5s when holding 2-handed items.", true);
                    break;
                case 9:
                    Plugin.Instance.paranoid = true;
                    name = "Paranoid";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Paranoid\nFear damages you.\nYou also seem to get scared by seemingly nothing..?", true);
                    break;
                case 10:
                    Plugin.Instance.conductive = true;
                    name = "Conductive";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Conductive\nFor the love of god, don't take metal.", true);
                    break;
                case 11:
                    Plugin.Instance.extrovert = true;   
                    name = "Extroverted";      
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're an Extrovert!\nYou get more paranoid when you're not with people..\nDon't get too paranoid or you'll take damage!", true);
                    break;
                case 12:
                    GameNetworkManager.Instance.localPlayerController.Crouch(false);
                    Plugin.Instance.hyperactive = true;   
                    name = "Hyperactive";      
                    __instance.StartCoroutine(hyperTip());
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Hyperactive!\nYou can't crouch and must keep moving.", true);
                    break;
                case 13:
                    Plugin.Instance.talkative = true;
            GameNetworkManager.Instance.localPlayerController.drunkness = 0.2f;
                    name = "Talkative";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Talkative!\nTalking controls your player speed!", true);
                    break;
                case 14:
                    Plugin.Instance.drunk = true;
                    name = "Drunk";
                    HUDManager.Instance.DisplayTip("Handicap Company", "You're Drunk!\nYou're.. drunk. idk how else i'm supposed to explain that", true);
                    break;
                }
                if (Plugin.Instance.chatAnnouncement.Value == 0) {
                    HUDManager.Instance.AddTextToChatOnServer("[HC] Got the handicap: " + name, (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                }
                if (Plugin.Instance.chatAnnouncement.Value == 1) {
                    HUDManager.Instance.AddTextToChatOnServer("[HC] " + name, (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                }
            } else {
                HUDManager.Instance.DisplayTip("Handicap Company", "You're not handicapped!\nYou got lucky, atleast for now...", false);
            }
            }
        }

        static IEnumerator illwaiter() {
            yield return new WaitForSeconds(4);
            Plugin.Instance.illiterate = true;
        }
    }
    
}