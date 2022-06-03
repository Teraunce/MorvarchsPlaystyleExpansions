﻿using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnnamedWotrMod;

namespace ExpandedContent.Config {
    public class Blueprints : IUpdatableSettings {
        [JsonProperty]
        private bool OverrideIds = false;
        [JsonProperty]
        private readonly SortedDictionary<string, Guid> NewBlueprints = new SortedDictionary<string, Guid>();
        [JsonProperty]
        private readonly SortedDictionary<string, Guid> AutoGenerated = new SortedDictionary<string, Guid>();
        [JsonProperty]
        private readonly SortedDictionary<string, Guid> UnusedGUIDs = new SortedDictionary<string, Guid>();
        private readonly SortedDictionary<string, Guid> UsedGUIDs = new SortedDictionary<string, Guid>();

        public void OverrideSettings(IUpdatableSettings userSettings) {
            var loadedSettings = userSettings as Blueprints;
            if (loadedSettings == null) { return; }
            if (loadedSettings.OverrideIds) {
                OverrideIds = loadedSettings.OverrideIds;
                loadedSettings.NewBlueprints.ForEach(entry => {
                    if (NewBlueprints.ContainsKey(entry.Key)) {
                        NewBlueprints[entry.Key] = entry.Value;
                    }
                });
            }
            loadedSettings.AutoGenerated.ForEach(entry => {
                AutoGenerated[entry.Key] = entry.Value;
            });
        }
        public BlueprintGuid GetGUID(string name) {

            Guid Id;
            if (!NewBlueprints.TryGetValue(name, out Id)) {
#if DEBUG
                if (!AutoGenerated.TryGetValue(name, out Id)) {
                    Id = Guid.NewGuid();
                    AutoGenerated.Add(name, Id);
                    Main.LogDebug($"Generated new GUID: {name} - {Id}");
                } else {
                    Main.LogDebug($"WARNING: GUID: {name} - {Id} is autogenerated");
                }
#endif
            }
            if (Id == null) { Main.Log($"ERROR: GUID for {name} not found"); }
            UsedGUIDs[name] = Id;
            return new BlueprintGuid(Id);
        }

        public void Init() {
        }

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class AutoGUID_Log_Patch {

            [HarmonyPriority(Priority.Last)]
            public static void Postfix() {
                GenerateUnused();
                ModSettings.SaveSettings("Blueprints.json", ModSettings.Blueprints);
            }
            static void GenerateUnused() {
                ModSettings.Blueprints.AutoGenerated.ForEach(entry => {
                    if (!ModSettings.Blueprints.UsedGUIDs.ContainsKey(entry.Key)) {
                        ModSettings.Blueprints.UnusedGUIDs[entry.Key] = entry.Value;
                    }
                });
                ModSettings.Blueprints.NewBlueprints.ForEach(entry => {
                    if (!ModSettings.Blueprints.UsedGUIDs.ContainsKey(entry.Key)) {
                        ModSettings.Blueprints.UnusedGUIDs[entry.Key] = entry.Value;
                    }
                });
            }
        }
    }
}
