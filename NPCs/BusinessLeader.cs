using S1API.Economy;
using S1API.Entities;
using S1API.Products;
using S1API.Properties;
using S1API.Entities.Appearances.AccessoryFields;
using S1API.Entities.Appearances.BodyLayerFields;
using S1API.Entities.Appearances.CustomizationFields;
using UnityEngine;

namespace RovingSpecialCustomers.NPCs;

public sealed class BusinessLeader : RovingCrewLeader
{
    protected override string CrewId => "businessmen";
    protected override string DialogueContainerId => "RSC_Business_Visit";

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("rsc_business_leader", "Grant", "Mercer")
            .WithSpawnPosition(new Vector3(0f, Utils.Constants.World.HiddenY, 0f))
            .WithAppearanceDefaults(appearance =>
            {
                appearance.Gender = 0f;
                appearance.Height = 1.02f;
                appearance.Weight = 0.44f;
                appearance.SkinColor = new Color32(189, 151, 122, 255);
                appearance.LeftEyeLidColor = appearance.SkinColor;
                appearance.RightEyeLidColor = appearance.SkinColor;
                appearance.HairColor = new Color(0.08f, 0.05f, 0.03f);
                appearance.HairPath = HairStyle.Spiky;
                appearance.WithBodyLayer<Shirts>(Shirts.RolledButtonUp, new Color(0.80f, 0.84f, 0.90f));
                appearance.WithBodyLayer<Pants>(Pants.CargoPants, new Color(0.12f, 0.13f, 0.16f));
                appearance.WithAccessoryLayer<Head>(Head.SmallRoundGlasses, Color.black);
                appearance.WithAccessoryLayer<Feet>(Feet.CombatBoots, Color.black);
            })
            .EnsureCustomer()
            .WithCustomerDefaults(customer =>
            {
                customer.WithSpending(0f, 0f).WithOrdersPerWeek(0, 0).WithStandards(CustomerStandard.High)
                    .AllowDirectApproach(false).WithCallPoliceChance(0f).WithDependence(0f, 0f)
                    .WithAffinities(new[] { (DrugType.Marijuana, -1f), (DrugType.Shrooms, -1f), (DrugType.Methamphetamine, -0.5f), (DrugType.Cocaine, 1f), (DrugType.MDMA, -0.5f), (DrugType.Heroin, -1f) })
                    .WithPreferredProperties(Property.Focused);
            })
            .WithRelationshipDefaults(relationship => relationship.SetUnlocked(true));
    }
}
