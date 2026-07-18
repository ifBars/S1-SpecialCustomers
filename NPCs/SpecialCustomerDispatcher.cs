using MelonLoader;
using RovingSpecialCustomers.Models;
using RovingSpecialCustomers.Services;
using S1API.Entities;
using S1API.Saveables;
using S1API.Utils;
using UnityEngine;

namespace RovingSpecialCustomers.NPCs;

public sealed class SpecialCustomerDispatcher : NPC
{
    private const string IconResourceName = "RovingSpecialCustomers.Assets.TravelWireIcon.png";

    private static Sprite? _travelWireIcon;

    public static SpecialCustomerDispatcher? Instance { get; private set; }

    [SaveableField("RovingSpecialCustomerVisits")]
    private RovingVisitState _state = new();

    public override bool IsPhysical => false;
    internal RovingVisitState State => _state;

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("rsc_dispatcher", "Travel", "Wire");

        var icon = GetTravelWireIcon();
        if (icon is not null)
        {
            builder.WithIcon(icon);
        }
        else
        {
            MelonLogger.Warning("[RSC] Travel Wire icon resource could not be loaded.");
        }
    }

    protected override void OnCreated()
    {
        base.OnCreated();
        Instance = this;
        ApplyTravelWireIcon();
        ClearConversationCategories();
        RovingVisitCoordinator.Bind(this);
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        _state ??= new RovingVisitState();
        Instance = this;
        ApplyTravelWireIcon();
        RovingVisitCoordinator.Bind(this);
    }

    protected override void OnDestroyed()
    {
        RovingVisitCoordinator.Unbind(this);
        if (ReferenceEquals(Instance, this))
        {
            Instance = null;
        }
        base.OnDestroyed();
    }

    internal void Tick(float deltaTime) => RovingVisitCoordinator.Tick(deltaTime);
    internal void SendSystemMessage(string message) => SendTextMessage(message);
    internal void SaveState() => RequestGameSave();

    private static Sprite? GetTravelWireIcon()
    {
        _travelWireIcon ??= ImageUtils.LoadImageFromResource(
            typeof(Core).Assembly,
            IconResourceName);

        return _travelWireIcon;
    }

    private void ApplyTravelWireIcon()
    {
        var icon = GetTravelWireIcon();
        if (icon is null)
            return;

        Icon = icon;
        RefreshMessagingIcons();
    }
}
