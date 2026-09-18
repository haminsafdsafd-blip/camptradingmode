using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace ShopTrader;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    private const string ModId = "ShopTrader";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    /// <summary>
    /// Verbose log helper — only emits when ShopTraderConfig.VerboseLogging is on.
    /// Errors should always use Logger.Error directly and are never gated.
    /// </summary>
    public static void LogVerbose(string message)
    {
        if (ShopTraderConfig.VerboseLogging)
            Logger.Info(message);
    }

    public static void Initialize()
    {
        Logger.Info("ShopTrader: Shop Trader mod initializing...");

        ModConfigRegistry.Register(ModId, new ShopTraderConfig());

        ModHelper.AddModelToPool<ColorlessCardPool, SwapEnergy>();
        ModHelper.AddModelToPool<ColorlessCardPool, SwapCards>();

        // Make both cards eligible for end-of-combat card rewards in any character's run.
        ModHelper.AddModelToPool<IroncladCardPool, SwapEnergy>();
        ModHelper.AddModelToPool<IroncladCardPool, SwapCards>();
        ModHelper.AddModelToPool<SilentCardPool, SwapEnergy>();
        ModHelper.AddModelToPool<SilentCardPool, SwapCards>();
        ModHelper.AddModelToPool<DefectCardPool, SwapEnergy>();
        ModHelper.AddModelToPool<DefectCardPool, SwapCards>();
        ModHelper.AddModelToPool<NecrobinderCardPool, SwapEnergy>();
        ModHelper.AddModelToPool<NecrobinderCardPool, SwapCards>();
        ModHelper.AddModelToPool<RegentCardPool, SwapEnergy>();
        ModHelper.AddModelToPool<RegentCardPool, SwapCards>();
        Logger.Info("ShopTrader: registered Swap Energy and Swap Cards in ColorlessCardPool and all character card pools.");

        Harmony harmony = new(ModId);
        harmony.PatchAll();

        Logger.Info("ShopTrader: Harmony patches applied.");
    }
}