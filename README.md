# S1 Special Customers

An independent Schedule I mod based on the developer-previewed special-customer update. Temporary customer crews visit Highland Point, request bulk orders with randomized preferred effects, and sell visit-exclusive cosmetics.

The preview describes non-final game content. This mod recreates the announced gameplay loop without using unreleased source or extracted assets.

## Crews

- Hippies buy marijuana.
- The business delegation buys cocaine.
- The road crew buys methamphetamine.
- The party bus buys shrooms.

## Visit loop

1. One crew is scheduled every 5-9 in-game days. The first visit can occur after 1-3 days.
2. The Travel Wire contact sends notice 2-3 days before arrival.
3. Each visit rolls 1-3 preferred effects and a rank-scaled group budget.
4. The lead customer, a travel assistant, and a matching available vehicle arrive in Northtown.
5. The best matching listed product is offered as a native S1API bulk contract.
6. Matching every requested effect raises payment by up to 50%.
7. The crew sells one exclusive cosmetic per visit and leaves the following day.

Native customer contracts handle acceptance, delivery, payment, receipts, saving, and multiplayer transaction RPCs.

## Requirements

- Schedule I 0.4.6 beta or alternate-beta
- MelonLoader 0.7.x
- S1API 3.1.0 beta or newer

Mono and IL2CPP use separate builds. The mod intentionally compiles against each backend's concrete game types rather than using a runtime reflection adapter.

## Build

Copy `example.build.props` to `local.build.props`, then set the local game and S1API paths.

```powershell
dotnet build .\S1SpecialCustomers.csproj -c Mono
dotnet build .\S1SpecialCustomers.csproj -c Il2cpp
```

Outputs:

- `bin/Mono/netstandard2.1/S1SpecialCustomers_Mono.dll`
- `bin/Il2cpp/net6.0/S1SpecialCustomers_Il2Cpp.dll`

## Smoke test

A gated runtime probe loads a save, waits for S1API custom NPC initialization, verifies six NPCs and four exclusive items, writes a result file, and exits.

```text
--s1-special-customers-smoke --s1-special-customers-smoke-save-slot 3
```

Verified against the 0.4.6 beta installs:

- Mono: build passed; loaded-save smoke passed.
- IL2CPP: build passed; loaded-save smoke passed.

## Preview differences

The preview showed cosmetics that are not present in the tested beta resources. The mod uses game-owned stand-ins: a skirt, executive hat, collar jacket, and mushroom hat. They can be replaced with the final public assets without changing saved visit state or contract behavior.

The preview also leaves group-vs-individual spending and arrival presentation undecided. This implementation uses one lead buyer with a shared budget and a Wire notice rather than a cutscene.

## Repository boundary

This repository contains no Schedule I assemblies, generated IL2CPP wrappers, decompiled dumps, prefabs, textures, downloaded videos, API keys, or local machine paths. Game files and private assembly repositories are local build inputs only.
