using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

namespace ShopTrader.Messages;

/// <summary>
/// Sent by the host (from GoldGiftSynchronizer) at shop time to sync the gold-gift
/// rules, so every client resolves gifted gold identically. Carries the
/// "enable gold gifting" and "gifted gold triggers gain-gold effects" toggles.
/// </summary>
public struct GoldConfigMessage : INetMessage, IPacketSerializable
{
    public bool enableGoldGifting;
    public bool giftedGoldTriggersGainEffects;

    public bool ShouldBroadcast => true;
    public NetTransferMode Mode => NetTransferMode.Reliable;
    public LogLevel LogLevel => LogLevel.Debug;
    public bool ShouldBuffer => false;

    public void Serialize(PacketWriter writer)
    {
        writer.WriteBool(enableGoldGifting);
        writer.WriteBool(giftedGoldTriggersGainEffects);
    }

    public void Deserialize(PacketReader reader)
    {
        enableGoldGifting = reader.ReadBool();
        giftedGoldTriggersGainEffects = reader.ReadBool();
    }

    public override string ToString()
    {
        return $"GoldConfigMessage(enableGoldGifting={enableGoldGifting}, giftedGoldTriggersGainEffects={giftedGoldTriggersGainEffects})";
    }
}
