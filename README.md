# Shop Trader

A **Slay the Spire 2** mod that adds a **Give Gold** button at merchant shops (real and event) so co-op teammates can share gold with each other.

> Targets STS2 Early Access **v0.111.0** (requires **v0.107.1** or newer). Built in C# / .NET 9 on the Godot SDK, using Harmony patches and the game's `INetMessage` co-op networking. Requires **[BaseLib](https://github.com/Alchyr/BaseLib-StS2)**.

## Give Gold at shops

At a merchant (real or event), a **Give Gold** button appears under each teammate's character. Click to send 50 gold; hold for an accelerating repeat to transfer larger amounts. Transfers are deterministic and synced across all clients — including the recipient's gain-gold relic effects (e.g. Dragon Fruit's +Max HP), which fire identically on every machine. The **Gifted gold triggers gain effects** toggle (default off) controls whether those effects fire at all; turn it on only if you want gifted gold to behave like normal earned gold. The whole feature is on by default and can be turned off in config.

## Configuration

Configured in-game via BaseLib's settings UI. Both settings are **host-authoritative** — the host's values are synced to clients:

- **Enable gold gifting** (default on — show Give Gold buttons at shops)
- **Gifted gold triggers gain effects** (default off — gifts are plain transfers; turn on to let gain-gold relics like Dragon Fruit fire)

## Building

```sh
dotnet build       # compiles and copies the DLL + manifest to the game's mods folder
dotnet publish     # additionally exports the .pck asset pack via MegaDot
```

The `.csproj` auto-detects the Steam install per-OS, publicizes `sts2.dll`, and references BaseLib. NuGet packages restore into a local `packages/` cache (gitignored).

## Credits

This mod is a trimmed-down, gold-gifting-only derivation of **[chaendizzle/STS2Trade](https://github.com/chaendizzle/STS2Trade)** ("Campfire Trading", on [Nexus Mods](https://www.nexusmods.com/slaythespire2/mods/107)). All campfire card/potion/relic trading was removed; the remaining pieces are the Give Gold shop feature, its networking, and the "gifted gold triggers gain effects" toggle. See [NOTICE.md](NOTICE.md) for full attribution.

The "gifted gold triggers gain effects" toggle was inspired by a fix in [Jzcse/STS2Trade](https://github.com/Jzcse/STS2Trade) (independently reimplemented here). The newer-build port [sirposh777/campfire-trading-update](https://github.com/sirposh777/campfire-trading-update) was also referenced during development. Config and infrastructure via [Alchyr's BaseLib](https://github.com/Alchyr/BaseLib-StS2) (MIT).