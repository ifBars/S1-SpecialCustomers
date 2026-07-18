#if MONO
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MelonLoader;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using S1API.Entities;
using S1API.Items;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RovingSpecialCustomers.Services;

internal sealed class SpecialCustomerSmokeTest
{
    private static readonly string[] ExpectedNpcIds =
    {
        "rsc_dispatcher", "rsc_hippie_leader", "rsc_business_leader",
        "rsc_biker_leader", "rsc_party_bus_leader", "rsc_travel_assistant"
    };

    private static readonly string[] ExpectedItemIds =
    {
        "rsc_hippie_skirt", "rsc_executive_hat", "rsc_road_jacket", "rsc_party_hat"
    };

    private readonly string _resultPath;
    private readonly int _saveSlot;
    private float _elapsed;
    private bool _loadRequested;

    private SpecialCustomerSmokeTest(string outputDirectory, int saveSlot)
    {
        Directory.CreateDirectory(outputDirectory);
        _resultPath = Path.Combine(outputDirectory, "result.txt");
        _saveSlot = saveSlot;
        if (File.Exists(_resultPath))
        {
            File.Delete(_resultPath);
        }
    }

    public static SpecialCustomerSmokeTest? TryCreate()
    {
        var args = Environment.GetCommandLineArgs();
        if (!args.Contains("--s1-special-customers-smoke", StringComparer.OrdinalIgnoreCase))
        {
            return null;
        }

        var outputDirectory = Path.Combine(Path.GetTempPath(), "S1SpecialCustomers.Smoke.Mono");
        var saveSlot = 1;
        for (var index = 0; index < args.Length; index++)
        {
            if (args[index] == "--s1-special-customers-smoke-dir" && index + 1 < args.Length)
            {
                outputDirectory = args[++index];
            }
            else if (args[index] == "--s1-special-customers-smoke-save-slot" && index + 1 < args.Length && int.TryParse(args[++index], out var parsedSlot))
            {
                saveSlot = parsedSlot;
            }
        }

        MelonLogger.Msg($"[S1SpecialCustomersSmoke] Enabled for Mono save slot {saveSlot}.");
        return new SpecialCustomerSmokeTest(outputDirectory, saveSlot);
    }

    public void Update()
    {
        _elapsed += Time.unscaledDeltaTime;
        if (_elapsed > 180f)
        {
            Finish(false, "Timed out waiting for a loaded save and registered content.");
            return;
        }

        if (!_loadRequested)
        {
            TryStartSave();
            return;
        }

        if (SceneManager.GetActiveScene().name != "Main" || !NPC.CustomNpcsReady)
        {
            return;
        }

        var missingNpcs = ExpectedNpcIds.Where(id => NPC.Get(id) is null).ToArray();
        var missingItems = ExpectedItemIds.Where(id => ItemManager.GetDefinition(id) is null).ToArray();
        var missingTravelWireIcon = NPC.Get("rsc_dispatcher")?.Icon is null;
        if (missingNpcs.Length == 0 && missingItems.Length == 0 && !missingTravelWireIcon)
        {
            Finish(true, $"Registered {ExpectedNpcIds.Length} NPCs, {ExpectedItemIds.Length} exclusive items, and the Travel Wire icon in scene Main.");
        }
        else
        {
            Finish(false, $"Missing NPCs=[{string.Join(",", missingNpcs)}] Items=[{string.Join(",", missingItems)}] TravelWireIcon={missingTravelWireIcon}.");
        }
    }

    private void TryStartSave()
    {
        if (!Singleton<LoadManager>.InstanceExists || !Singleton<SaveManager>.InstanceExists)
        {
            return;
        }

        var loadManager = Singleton<LoadManager>.Instance;
        if (loadManager.IsLoading)
        {
            return;
        }

        loadManager.RefreshSaveInfo();
        var index = Mathf.Clamp(_saveSlot, 1, LoadManager.SaveGames.Length) - 1;
        var save = LoadManager.SaveGames[index] ?? LoadManager.LastPlayedGame;
        if (save is null)
        {
            Finish(false, $"No save found for requested slot {_saveSlot}.");
            return;
        }

        _loadRequested = true;
        MelonLogger.Msg($"[S1SpecialCustomersSmoke] Loading '{save.OrganisationName}'.");
        loadManager.StartGame(save, false, false);
    }

    private void Finish(bool passed, string reason)
    {
        var result = $"{(passed ? "PASS" : "FAIL")}|Backend=Mono|Reason={reason}";
        File.WriteAllText(_resultPath, result);
        if (passed)
        {
            MelonLogger.Msg($"[S1SpecialCustomersSmoke] {result}");
        }
        else
        {
            MelonLogger.Error($"[S1SpecialCustomersSmoke] {result}");
        }

        Application.Quit();
    }
}
#endif
