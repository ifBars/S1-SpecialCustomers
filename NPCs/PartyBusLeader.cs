using S1API.Economy;
using S1API.Entities;
using S1API.Products;
using S1API.Properties;
using S1API.Entities.Appearances.AccessoryFields;
using S1API.Entities.Appearances.BodyLayerFields;
using S1API.Entities.Appearances.CustomizationFields;
using S1API.Entities.Appearances.FaceLayerFields;
using UnityEngine;

namespace RovingSpecialCustomers.NPCs;

public sealed class PartyBusLeader : RovingCrewLeader
{
    protected override string CrewId => "party_bus";
    protected override string DialogueContainerId => "RSC_PartyBus_Visit";

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("rsc_party_bus_leader", "Nova", "Reyes")
            .WithSpawnPosition(new Vector3(0f, Utils.Constants.World.HiddenY, 0f))
            .WithAppearanceDefaults(appearance =>
            {
                appearance.Gender = 0.85f;
                appearance.Height = 0.98f;
                appearance.Weight = 0.46f;
                appearance.SkinColor = new Color32(181, 132, 99, 255);
                appearance.LeftEyeLidColor = appearance.SkinColor;
                appearance.RightEyeLidColor = appearance.SkinColor;
                appearance.HairColor = new Color(0.48f, 0.06f, 0.32f);
                appearance.HairPath = HairStyle.Spiky;
                appearance.WithFaceLayer<Face>(Face.SlightSmile, Color.black);
                appearance.WithBodyLayer<Shirts>(Shirts.TShirt, new Color(0.82f, 0.12f, 0.62f));
                appearance.WithBodyLayer<Pants>(Pants.Jorts, new Color(0.10f, 0.34f, 0.74f));
                appearance.WithAccessoryLayer<Head>(Head.SmallRoundGlasses, new Color(0.95f, 0.45f, 0.75f));
                appearance.WithAccessoryLayer<Feet>(Feet.Sneakers, Color.white);
            })
            .EnsureCustomer()
            .WithCustomerDefaults(customer =>
            {
                customer.WithSpending(0f, 0f).WithOrdersPerWeek(0, 0).WithStandards(CustomerStandard.Moderate)
                    .AllowDirectApproach(false).WithCallPoliceChance(0f).WithDependence(0f, 0f)
                    .WithAffinities(new[] { (DrugType.Marijuana, -0.5f), (DrugType.Shrooms, 1f), (DrugType.Methamphetamine, -1f), (DrugType.Cocaine, -0.5f), (DrugType.MDMA, 1f), (DrugType.Heroin, -1f) })
                    .WithPreferredProperties(Property.Euphoric);
            })
            .WithRelationshipDefaults(relationship => relationship.SetUnlocked(true));
    }
}
