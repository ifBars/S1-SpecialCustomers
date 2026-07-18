# Research and implementation basis

## Developer preview

The supplied update video describes roughly weekly roving groups that:

- can arrive at any player level;
- buy a specific drug category per group;
- randomize favorite effects each visit;
- buy bulk quantities as an excess-inventory sink;
- pay a substantial effect-match bonus;
- scale spending with player rank;
- provide 2-3 days of advance warning; and
- sell group-exclusive, mostly cosmetic items.

The four previewed groups are hippies, businessmen, bikers, and party-bus visitors. The video explicitly states that details are non-final.

## Beta inspection

Focused Mono and IL2CPP inspection covered `TimeManager`, `Customer`, `CustomerData`, `ProductDefinition`, `LevelManager`, `VehicleManager`, `LandVehicle`, and the S1API customer/contract wrappers.

Findings used by the implementation:

- The tested 0.4.6 beta does not contain a complete native special-customer system to enable.
- `TimeManager` exposes elapsed day and current time on both backends.
- Native customer enjoyment already combines drug affinity, preferred effects, and quality.
- Native customer spending scales with `LevelManager`'s order-limit multiplier.
- S1API exposes typed `ContractInfo` and `NPCCustomer.OfferContract` APIs on both backends.
- Product wrappers expose listed products, drug type, properties, market value, and stable IDs.
- `VehicleManager` exposes prefab discovery and authoritative vehicle spawning.
- Native contracts and customer handovers provide the transaction and multiplayer path.
- Exact preview cosmetics are not available in the tested beta resource catalog.

## Engineering decisions

- S1API owns custom NPC registration, customer components, products, contracts, items, messaging, and persisted fields.
- Mono and IL2CPP runtime access lives in separate compile-time files using exact game types.
- Shared feature code uses typed S1API product, property, appearance, and item APIs.
- No IL transpilers, broad Harmony patches, backend-neutral reflection adapter, proprietary assets, or generated wrappers are included.
- A command-line-gated loaded-save smoke test validates registration without delaying normal gameplay startup.
