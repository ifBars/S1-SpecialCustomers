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

public sealed class HippieLeader : RovingCrewLeader
{
    protected override string CrewId => "hippies";
    protected override string DialogueContainerId => "RSC_Hippie_Visit";

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity("rsc_hippie_leader", "Sunny", "Vale")
            .WithSpawnPosition(new Vector3(0f, Utils.Constants.World.HiddenY, 0f))
            .WithAppearanceDefaults(appearance =>
            {
                appearance.Gender = 0.75f;
                appearance.Height = 0.96f;
                appearance.Weight = 0.40f;
                appearance.SkinColor = new Color32(207, 166, 126, 255);
                appearance.LeftEyeLidColor = appearance.SkinColor;
                appearance.RightEyeLidColor = appearance.SkinColor;
                appearance.HairColor = new Color(0.40f, 0.20f, 0.07f);
                appearance.HairPath = HairStyle.BowlCut;
                appearance.WithFaceLayer<Face>(Face.Neutral, Color.black);
                appearance.WithFaceLayer<Eyes>(Eyes.Freckles, new Color(0.28f, 0.15f, 0.08f));
                appearance.WithBodyLayer<Shirts>(Shirts.TShirt, new Color(0.30f, 0.62f, 0.28f));
                appearance.WithBodyLayer<Pants>(Pants.Jeans, new Color(0.26f, 0.20f, 0.45f));
                appearance.WithAccessoryLayer<Head>(Head.SmallRoundGlasses, new Color(0.85f, 0.55f, 0.12f));
                appearance.WithAccessoryLayer<Feet>(Feet.Sneakers, Color.white);
            })
            .EnsureCustomer()
            .WithCustomerDefaults(customer =>
            {
                customer.WithSpending(0f, 0f).WithOrdersPerWeek(0, 0).WithStandards(CustomerStandard.Moderate)
                    .AllowDirectApproach(false).WithCallPoliceChance(0f).WithDependence(0f, 0f)
                    .WithAffinities(new[] { (DrugType.Marijuana, 1f), (DrugType.Shrooms, 1f), (DrugType.Methamphetamine, -1f), (DrugType.Cocaine, -1f), (DrugType.MDMA, -0.5f), (DrugType.Heroin, -1f) })
                    .WithPreferredProperties(Property.Euphoric);
            })
            .WithRelationshipDefaults(relationship => relationship.SetUnlocked(true));
    }
}
