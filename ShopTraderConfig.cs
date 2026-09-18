using BaseLib.Config;
using Godot;

namespace ShopTrader;

public class ShopTraderConfig : SimpleModConfig
{
    /// <summary>
    /// When true, "Give Gold" buttons appear under other players at merchant shops,
    /// letting players gift gold to each other. On by default. Host-authoritative:
    /// synced to clients via GoldConfigMessage.
    /// </summary>
    public static bool EnableGoldGifting { get; set; } = true;

    /// <summary>
    /// When true, gold given via the shop Give Gold button triggers the recipient's
    /// gain-gold relic effects (e.g. Dragon Fruit's +Max HP) — matching normal earned
    /// gold. When false (default), gifted gold is a plain transfer that fires no
    /// gain-gold effects (prevents two Dragon Fruit owners farming Max HP by bouncing
    /// gold). Applied identically on every client, so it never desyncs. Host-authoritative.
    /// </summary>
    public static bool GiftedGoldTriggersGainEffects { get; set; } = false;

    /// <summary>
    /// Local-only (NOT synced over the network): when true, emit detailed sync logs
    /// to godot.log. Off by default so normal runs stay quiet and the game's
    /// LOCAL-vs-REMOTE desync state-diff dumps aren't buried under transfer chatter.
    /// Errors always log regardless of this setting.
    /// </summary>
    public static bool VerboseLogging { get; set; } = false;

    public ShopTraderConfig() { }

    public override void SetupConfigUI(Control optionContainer)
    {
        foreach (var child in optionContainer.GetChildren())
            child.Free();

        base.SetupConfigUI(optionContainer);
    }
}