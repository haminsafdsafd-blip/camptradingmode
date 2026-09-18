using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

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

        Harmony harmony = new(ModId);
        harmony.PatchAll();

        Logger.Info("ShopTrader: Harmony patches applied.");
    }
}