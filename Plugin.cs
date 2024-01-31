using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DunGen;
using GameNetcodeStuff;
using HandicapCompany.patches;
using HarmonyLib;

namespace HandicapCompany
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        
        private const string modGUID = "org.agmas.HandicapCompany";
        private const string modName = "Handicap Company";
        private const string modVersion = "0.3.0";

        public float timeSinceLastDmg = 0;
        public float modScareFactor = 0;
        public int ogSense = 0;

        public bool blind = false;
        public bool mute = false;
        public bool palsy = false;
        public bool deaf = false;
        public bool crippled = false;

        public bool illiterate = false;
        public bool introvert = false;
        public bool extrovert = false;
        public bool weak = false;
        public bool weighty = false;
        public bool paranoid = false;
        public bool conductive = false;

        public ConfigEntry<string> availableHandicaps;
        public List<int> available;
        public ConfigEntry<int> handicapChance;
        public ConfigEntry<bool> allowMuteWhileMuted;
        public ConfigEntry<bool> allowMuteWhileSolo;
        public ConfigEntry<bool> allowIntroExtroWhileSolo;

        private ConfigEntry<string> prv;

        private readonly Harmony harmony = new Harmony(modGUID);
        public static Plugin Instance;

        public ManualLogSource mls;

        public void reloadConfig() {
            mls.LogInfo("Load/Reload Config");
            availableHandicaps = Config.Bind("General",      // The section under which the option is shown
                                         "AvailableHandicaps",  // The key of the configuration option in the configuration file
                                         "Blind,Deaf,Mute,Palsy,Legs,NoUI,Intro,Weak,Weight,Paranoid,Conduct,Extro", // The default value
                                         "All the available handicaps, sperated by \",\". Does not sync between host and client."); // Description of the option to show in the config file
            handicapChance = Config.Bind("General",      // The section under which the option is shown
                                         "HandiChance",  // The key of the configuration option in the configuration file
                                         100, // The default value
                                         "Chance to be handicapped. Does not sync between host and client."); // Description of the option to show in the config fil
             mls.LogInfo("Binding Finished, Reading info from: " + availableHandicaps.Value);
            allowMuteWhileMuted = Config.Bind("Auto-Disable",      // The section under which the option is shown
                                         "amwm",  // The key of the configuration option in the configuration file
                                         false, // The default value
                                         "Allows you to get mute when your microphone settings are off."); // Description of the option to show in the config file
            allowMuteWhileSolo = Config.Bind("Auto-Disable",      // The section under which the option is shown
                                         "amws",  // The key of the configuration option in the configuration file
                                         false, // The default value
                                         "Allows you to get mute when you're alone."); // Description of the option to show in the config file
            allowIntroExtroWhileSolo = Config.Bind("Auto-Disable",      // The section under which the option is shown
                                         "aiews",  // The key of the configuration option in the configuration file
                                         false, // The default value
                                         "Allows you to get Introvet/Extrovert when you're alone.\nTHIS USES THE SANITY VALUES, WHICH IN SINGLEPLAYER, RISE WHEN IN THE SHIP OR FACILITY, BUT LOWER OUTSIDE."); // Description of the option to show in the config file
            available = new List<int>();
            string[] handis = availableHandicaps.Value.Split(',');
            foreach (var s in handis) {
                mls.LogInfo(s);
                switch (s.ToLower()) {
                    case "blind":
                        available.Add(0);
                        continue;
                    case "deaf":
                        available.Add(3);
                        continue;
                    case "mute":
                        available.Add(1);
                        continue;
                    case "palsy":
                        available.Add(2);
                        continue;
                    case "legs":
                        available.Add(4);
                        continue;
                    case "noui":
                        available.Add(5);
                        continue;
                    case "intro":
                        available.Add(6);
                        continue;
                    case "weight":
                        available.Add(7);
                        continue;
                    case "weak":
                        available.Add(8);
                        continue;
                    case "paranoid":
                        available.Add(9);
                        continue;
                    case "conduct":
                        available.Add(10);
                        continue;
                    case "extro":
                        available.Add(11);
                        continue;
                }
            }
        }
        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
            }   

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("We're up and running!");
            
            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(StartOfRoundPatch));
            harmony.PatchAll(typeof(RoundManagerPatch));
            harmony.PatchAll(typeof(IngamePlayerSettingsPatch));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            harmony.PatchAll(typeof(HUDManagerPatch));
            harmony.PatchAll(typeof(TerminalPach));


             mls.LogInfo("Binding Config");

        
            prv = Config.Bind("__Do not touch",      // The section under which the option is shown
                                         "prv",  // The key of the configuration option in the configuration file
                                         "0.4.0", // The default value
                                         "Do not change, Used to automatically add new handicaps when they come out!"); // Description of the option to show in the config file


            reloadConfig();
            if (prv.Value != "0.4.1") {
                availableHandicaps.SetSerializedValue("Blind,Deaf,Mute,Palsy,Legs,NoUI,Intro,Weak,Weight,Paranoid,Conduct,Extro");
                prv.SetSerializedValue("0.4.1");
                reloadConfig();
             }
            mls.LogInfo(available);
            
            
        }
    }
}