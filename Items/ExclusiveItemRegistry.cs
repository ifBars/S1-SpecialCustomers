using System;
using MelonLoader;
using S1API.Console;
using S1API.Items;
using ClothingColor = S1API.Items.Clothing.ClothingColor;
using ClothingItemCreator = S1API.Items.Clothing.ClothingItemCreator;
using S1API.Money;
using RovingSpecialCustomers.Models;

namespace RovingSpecialCustomers.Items;

public static class ExclusiveItemRegistry
{
    private static bool _initialized;

    public static void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        CreateClothing("skirt", "rsc_hippie_skirt", "Sunwashed Tour Skirt", "A bright skirt sold only by the roving hippie crew.", "Avatar/Accessories/Bottom/MediumSkirt/MediumSkirt", ClothingColor.Lime, 325f);
        CreateClothing("porkpiehat", "rsc_executive_hat", "Executive Travel Hat", "A restrained black hat sold only by the visiting business delegation.", "Avatar/Accessories/Head/PorkpieHat/PorkpieHat", ClothingColor.Black, 650f);
        CreateClothing("collarjacket", "rsc_road_jacket", "Road Crew Jacket", "A charcoal jacket sold only by the roving road crew.", "Avatar/Accessories/Chest/CollarJacket/CollarJacket", ClothingColor.Charcoal, 500f);
        CreateClothing("mushroomhat", "rsc_party_hat", "Afterparty Mushroom Hat", "A fungal statement piece sold only by the party bus crew.", "Avatar/Accessories/Head/MushroomHat/MushroomHat", ClothingColor.HotPink, 550f);
        _initialized = true;
    }

    public static bool TryPurchase(CrewDefinition definition, out string message)
    {
        var item = ItemManager.GetDefinition(definition.ExclusiveItemId);
        if (item is null)
        {
            message = $"The exclusive item '{definition.ExclusiveItemName}' is unavailable.";
            return false;
        }

        var cash = Money.GetCashBalance();
        if (cash < definition.ExclusiveItemPrice)
        {
            message = $"You need ${definition.ExclusiveItemPrice:0} in cash for {definition.ExclusiveItemName}.";
            return false;
        }

        Money.ChangeCashBalance(-definition.ExclusiveItemPrice, visualizeChange: true, playCashSound: true);
        ConsoleHelper.AddItemToInventory(definition.ExclusiveItemId, 1);
        message = $"Purchased {definition.ExclusiveItemName} for ${definition.ExclusiveItemPrice:0}.";
        return true;
    }

    private static void CreateClothing(
        string baseItemId,
        string id,
        string name,
        string description,
        string assetPath,
        ClothingColor color,
        float price)
    {
        try
        {
            if (ItemManager.GetDefinition(id) is not null)
            {
                return;
            }

            var builder = ClothingItemCreator.CloneFrom(baseItemId);
            if (builder is null)
            {
                MelonLogger.Error($"[RSC] Could not clone '{baseItemId}' for '{id}'.");
                return;
            }

            builder.WithBasicInfo(id, name, description, ItemCategory.Clothing)
                .WithClothingAsset(assetPath)
                .WithColorable(false)
                .WithDefaultColor(color)
                .WithPricing(price, 0.5f)
                .Build();
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[RSC] Failed to create exclusive item '{id}': {ex.Message}");
        }
    }
}
