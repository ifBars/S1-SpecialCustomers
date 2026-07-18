#if IL2CPP
using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.Effects;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Levelling;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.Vehicles;
using MelonLoader;
using RovingSpecialCustomers.Models;
using RovingSpecialCustomers.NPCs;
using UnityEngine;

namespace RovingSpecialCustomers.Services;

public static class SpecialCustomerRuntime
{
    private static LandVehicle? _activeVehicle;

    public static bool IsReady => TimeManager.Instance is not null && LevelManager.Instance is not null;
    public static bool IsHost => Lobby.Instance is null || Lobby.Instance.IsHost;
    public static int ElapsedDays => TimeManager.Instance.ElapsedDays;
    public static int CurrentTime => TimeManager.Instance.CurrentTime;

    public static float GetRankBudgetMultiplier() =>
        LevelManager.GetOrderLimitMultiplier(LevelManager.Instance.GetFullRank());

    public static void ApplyVisitEffects(RovingCrewLeader leader, IReadOnlyCollection<string> effectIds)
    {
        try
        {
            var customer = leader.gameObject.GetComponent<Customer>();
            if (customer?.CustomerData is null)
            {
                MelonLogger.Warning($"[RSC] Customer data missing for {leader.ID}.");
                return;
            }

            customer.CustomerData.PreferredProperties.Clear();
            var requested = new HashSet<string>(effectIds, StringComparer.OrdinalIgnoreCase);
            var resolved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var path in EffectResourcePaths)
            {
                var effects = Resources.LoadAll<Effect>(path);
                for (var index = 0; index < effects.Length; index++)
                {
                    var effect = effects[index];
                    if (effect is not null && requested.Contains(effect.ID) && resolved.Add(effect.ID))
                    {
                        customer.CustomerData.PreferredProperties.Add(effect);
                    }
                }
            }

            LogMissingEffects(requested, resolved);
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[RSC] Failed to apply visit effects: {ex}");
        }
    }

    public static void SpawnCrewVehicle(CrewDefinition definition)
    {
        DestroyCrewVehicle();
        try
        {
            var manager = VehicleManager.Instance;
            if (manager is null)
            {
                return;
            }

            var code = ResolveVehicleCode(manager, definition.VehicleHints);
            if (code is null)
            {
                MelonLogger.Warning($"[RSC] No vehicle prefab matched {definition.DisplayName}; continuing without a vehicle.");
                return;
            }

            _activeVehicle = manager.SpawnAndReturnVehicle(code, definition.VehiclePosition, definition.VehicleRotation, false);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[RSC] Failed to spawn crew vehicle: {ex}");
        }
    }

    public static void DestroyCrewVehicle()
    {
        try
        {
            _activeVehicle?.DestroyVehicle();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[RSC] Failed to remove crew vehicle: {ex}");
        }
        finally
        {
            _activeVehicle = null;
        }
    }

    private static readonly string[] EffectResourcePaths =
        { "Properties/Tier1", "Properties/Tier2", "Properties/Tier3", "Properties/Tier4", "Properties/Tier5" };

    private static string? ResolveVehicleCode(VehicleManager manager, IReadOnlyList<string> hints)
    {
        foreach (var hint in hints)
        {
            for (var index = 0; index < manager.VehiclePrefabs.Count; index++)
            {
                var prefab = manager.VehiclePrefabs[index];
                if (prefab is not null && !string.IsNullOrWhiteSpace(prefab.VehicleCode) &&
                    prefab.VehicleCode.Contains(hint, StringComparison.OrdinalIgnoreCase))
                {
                    return prefab.VehicleCode;
                }
            }
        }

        return manager.GetVehiclePrefab("shitbox") is not null ? "shitbox" : null;
    }

    private static void LogMissingEffects(IEnumerable<string> requested, ISet<string> resolved)
    {
        foreach (var missing in requested.Where(id => !resolved.Contains(id)))
        {
            MelonLogger.Warning($"[RSC] Could not resolve effect asset '{missing}'.");
        }
    }
}
#endif
