using System;
using HarmonyLib;
using HandicapCompany;
using GameNetcodeStuff;
using UnityEngine;
using DigitalRuby.ThunderAndLightning;
using System.Numerics;
using System.Collections;
using Dissonance;


namespace HandicapCompany.patches {

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal class PlayerControllerBPatch {

         [HarmonyPatch(typeof(PlayerControllerB), "SpectateNextPlayer")]
        [HarmonyPostfix]    
        static void undo(PlayerControllerB __instance) {
            Plugin.Instance.stopHandicapStacking = false;
            Plugin.Instance.mute = false;
            Plugin.Instance.deaf = false;
            if (Plugin.Instance.palsy) {
                IngamePlayerSettings.Instance.settings.lookSensitivity = Plugin.Instance.ogSense;
            }
            Plugin.Instance.palsy = false;
            Plugin.Instance.blind = false;
            Plugin.Instance.talkative = false;
            Plugin.Instance.drunk = false;
            GameNetworkManager.Instance.localPlayerController.drunkness = 0f;
            Plugin.Instance.crippled = false;
            Plugin.Instance.introvert = false;
            Plugin.Instance.illiterate = false;
            Plugin.Instance.weak = false;
            Plugin.Instance.weighty = false;
            Plugin.Instance.paranoid = false;
            Plugin.Instance.conductive = false;
            Plugin.Instance.extrovert = false;
            Plugin.Instance.hyperactive = false;
            GameNetworkManager.Instance.localPlayerController.carryWeight = 1f;
            HUDManager.Instance.HideHUD(false);
            IngamePlayerSettings.Instance.LoadSettingsFromPrefs();
            IngamePlayerSettings.Instance.UpdateGameToMatchSettings();
        }

        [HarmonyPatch(typeof(PlayerControllerB), "SetPlayerSanityLevel")]
        [HarmonyPostfix]
        public static void insanity(PlayerControllerB __instance) {

            if (Plugin.Instance.introvert && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                if (__instance.insanitySpeedMultiplier > 0) {
                    __instance.insanitySpeedMultiplier += 1.5f;
                }
                if (__instance.insanitySpeedMultiplier < 0) {
                    __instance.insanitySpeedMultiplier -= 0.7f;
                }
                
                __instance.insanityLevel = Mathf.MoveTowards(__instance.insanityLevel, __instance.maxInsanityLevel, (float)(Time.deltaTime * (-__instance.insanitySpeedMultiplier)*1.4));
                if (__instance.insanityLevel < 0f) {
                    __instance.insanityLevel = 0f;
                }
                 if (__instance.insanityLevel > 50f) {
                    __instance.insanityLevel = 50f;
                }
                if (__instance.insanityLevel >= 50) {
                    Plugin.Instance.modScareFactor = 0.76f;
                    if (Plugin.Instance.timeSinceLastDmg >= 0.35) {
                        __instance.DamagePlayer(1, true, true, CauseOfDeath.Abandoned);
                        Plugin.Instance.timeSinceLastDmg = 0;
                    }
                    
                    
                } else {
                    if (__instance.insanityLevel >= 45) {
                        Plugin.Instance.modScareFactor = 0.42f;
                        if (!Plugin.Instance.intipShown) {
                            Plugin.Instance.intipShown = true;
                            HUDManager.Instance.DisplayTip("Handicap Company", "Get away from people!\nYou'll start taking damage due to\nbeing introverted!", true);
                        }
                    } else {
                        if (__instance.insanityLevel >= 35) {
                            Plugin.Instance.modScareFactor = 0.15f;
                        }
                    }
                }
            }

            if (Plugin.Instance.extrovert && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                if (__instance.insanitySpeedMultiplier < 0) {
                    __instance.insanitySpeedMultiplier -= 1.5f;
                }
                if (__instance.insanitySpeedMultiplier > 0) {
                    __instance.insanitySpeedMultiplier += 0.7f;
                }
                
                __instance.insanityLevel = Mathf.MoveTowards(__instance.insanityLevel, __instance.maxInsanityLevel, (float)(Time.deltaTime * __instance.insanitySpeedMultiplier*1.4));
                if (__instance.insanityLevel < 0f) {
                    __instance.insanityLevel = 0f;
                }
                 if (__instance.insanityLevel > 50f) {
                    __instance.insanityLevel = 50f;
                }
                if (__instance.insanityLevel >= 50) {
                    Plugin.Instance.modScareFactor = 0.76f;
                    if (Plugin.Instance.timeSinceLastDmg >= 0.35) {
                        __instance.DamagePlayer(1, true, true, CauseOfDeath.Abandoned);
                        Plugin.Instance.timeSinceLastDmg = 0;
                    }
                    
                    
                } else {
                    if (__instance.insanityLevel >= 45) {
                        Plugin.Instance.modScareFactor = 0.42f;
                        if (!Plugin.Instance.extipShown) {
                            Plugin.Instance.extipShown = true;
                            HUDManager.Instance.DisplayTip("Handicap Company", "Get back to people!\nYou'll start taking damage due to\nbeing extroverted!", true);
                        }
                    } else {
                        if (__instance.insanityLevel >= 35) {
                            Plugin.Instance.modScareFactor = 0.15f;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Crouch")]
        [HarmonyPrefix]
        public static bool gammaPatch5(PlayerControllerB __instance) {
            if (Plugin.Instance.hyperactive && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Crouch_performed")]
        [HarmonyPrefix]
        public static bool nuhUh(PlayerControllerB __instance) {
            if (Plugin.Instance.hyperactive && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                HUDManager.Instance.DisplayTip("Handicap Company", "nuh uh");
                return false;
            }
            return true;
        }


        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        public static void gammaPatch(PlayerControllerB __instance) {
            if (Plugin.Instance.hyperactive && GameNetworkManager.Instance.localPlayerController.Equals(__instance) && StartOfRound.Instance.shipHasLanded) {
                if (__instance.moveInputVector.Equals(UnityEngine.Vector2.zero)) {
                    Plugin.Instance.modScareFactor += (float)(Time.deltaTime*1.5);
                    if (Plugin.Instance.modScareFactor > 0.74f) {
                        Plugin.Instance.modScareFactor = 0.74f;
                    }
                    if (Plugin.Instance.modScareFactor >= 0.74f) {
                        if (Plugin.Instance.timeSinceLastDmg > 0.025f) {
                            Plugin.Instance.timeSinceLastDmg = 0;
                            __instance.DamagePlayer(1, true, true, CauseOfDeath.Crushing);
                        }
                    }
                } else {
                    Plugin.Instance.modScareFactor -= Time.deltaTime/2;
                }
            }
            Plugin.Instance.timeSinceLastDmg += Time.deltaTime;
            float prevscare = Plugin.Instance.modScareFactor;
            Plugin.Instance.modScareFactor -= Time.deltaTime/100;
            if (Plugin.Instance.modScareFactor < 0f) {
                Plugin.Instance.modScareFactor = 0f;
            }
            if (Plugin.Instance.modScareFactor > 1f) {
                Plugin.Instance.modScareFactor = 1f;
            }
            if (StartOfRound.Instance.fearLevel < Plugin.Instance.modScareFactor) {
                StartOfRound.Instance.fearLevel = Plugin.Instance.modScareFactor;
            } else {
                if (Plugin.Instance.modScareFactor < prevscare) {
                     StartOfRound.Instance.fearLevel = Plugin.Instance.modScareFactor;
                }
            }
            
            if (Plugin.Instance.paranoid && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                if (Plugin.Instance.timeSinceLastDmg > 15) {
                    Plugin.Instance.timeSinceLastDmg = 0;
                    switch (new System.Random().Next(0,10)) {
                        case 0:
                            __instance.DamagePlayer(0,true);
                            break;
                        case 1:
                            RoundManager.Instance.FlickerLights(true);
                            break;
                        case 2:
                            HUDManager.Instance.DisplayTip("boo", "did i scare you", true);
                            break;
                    }
                }
            }
            if (Plugin.Instance.conductive && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                if (Plugin.Instance.timeSinceLastDmg > 1.5) {
                    Plugin.Instance.timeSinceLastDmg = 0;
                    if (!__instance.isInsideFactory && !__instance.isInHangarShipRoom) {
                        foreach (GrabbableObject g in __instance.ItemSlots) {
                            if (g == null) {
                                continue;
                            }
                            if (g.itemProperties == null) {
                                continue;
                            }
                            if (g.itemProperties.isConductiveMetal) {
                                if (new System.Random().Next(0,3) == 0) {
                                    Landmine.SpawnExplosion(GameNetworkManager.Instance.localPlayerController.transform.position + UnityEngine.Vector3.up * 0.25f, spawnExplosionEffect: false, 2.4f, 5f);
                                    HUDManager.Instance.ShakeCamera(ScreenShakeType.Big);
                                    GameNetworkManager.Instance.localPlayerController.DamagePlayer(999, true, true, CauseOfDeath.Electrocution);
                                    HUDManager.Instance.DisplayTip("Handicap Company", "You were too conductive!\nYou exploded due to too much electricity.\nSkill issue", true);
                                }
                            }
                        }
                    }
                }
            }
            if (Plugin.Instance.weighty && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                if (__instance.carryWeight < 1.6f) {
                    __instance.carryWeight = 1.6f;
                }
            }
            if (Plugin.Instance.drunk && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                GameNetworkManager.Instance.localPlayerController.drunkness = 0.75f;
            }
            if (Plugin.Instance.talkative && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                if (!IngamePlayerSettings.Instance.settings.micEnabled || (UnityEngine.Object)(object)StartOfRound.Instance.voiceChatModule == (UnityEngine.Object)null)
			    {
        
                } else {
                    VoicePlayerState val = StartOfRound.Instance.voiceChatModule.FindPlayer(StartOfRound.Instance.voiceChatModule.LocalPlayerName);
                    if (val.IsSpeaking)
			        {
				        float num = Mathf.Clamp(val.Amplitude*140f, 0f, 4.6f);
				        __instance.movementSpeed = num;
			        }
                }
			    
            }
            if (Plugin.Instance.weak && GameNetworkManager.Instance.localPlayerController.Equals(__instance) && __instance.twoHanded && Plugin.Instance.timeSinceLastDmg >= 0.5) {
                __instance.DamagePlayer(1, true, true, CauseOfDeath.Gravity);
                Plugin.Instance.timeSinceLastDmg = 0;
            }
            if (Plugin.Instance.illiterate && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                HUDManager.Instance.HideHUD(true);
            }
            
            if (Plugin.Instance.crippled && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                if (__instance.health > 15) {
                    __instance.health = 15;
                }
                __instance.MakeCriticallyInjured(enable: true);
            }

            if (Plugin.Instance.paranoid && GameNetworkManager.Instance.localPlayerController.Equals(__instance) && Plugin.Instance.timeSinceLastDmg >= 1) {
                if (StartOfRound.Instance.fearLevel >= 0.1f) {
                    __instance.DamagePlayer(1, true, true, CauseOfDeath.Suffocation);
                    if (StartOfRound.Instance.fearLevel >= 0.4f) {
                        __instance.DamagePlayer(2, true, true, CauseOfDeath.Suffocation);
                    }
                    if (StartOfRound.Instance.fearLevel >= 0.74f) {
                        __instance.DamagePlayer(3, true, true, CauseOfDeath.Suffocation);
                    }
                    Plugin.Instance.timeSinceLastDmg = 0;
                }
            }
            
            if (Plugin.Instance.weak && GameNetworkManager.Instance.localPlayerController.Equals(__instance)) {
                if (__instance.health > 50) {
                    __instance.health = 50;
                }
            }
        }
    }

    

}