using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace ShopTrader;

/// <summary>
/// Common colorless multiplayer card: costs X (all of your remaining energy), swaps
/// your current energy with a friendly player's energy, and makes you lose 4 HP.
/// Upgraded, you no longer lose HP.
/// </summary>
[Pool(typeof(ColorlessCardPool))]
public sealed class SwapEnergy : CustomCardModel
{
	public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

	public override string? CustomPortraitPath => "res://ShopTrader/trade.png";

	public override string PortraitPath => base.PortraitPath;

	/// <summary>
	/// X-cost cards spend all of the owner's remaining energy when played, so by the
	/// time OnPlay runs our energy is already spent and the swap gives us the ally's energy.
	/// </summary>
	public override bool HasEnergyCostX => true;

	public SwapEnergy()
		: base(0, CardType.Skill, CardRarity.Common, TargetType.AnyAlly, autoAdd: false)
	{
	}

	public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target?.Player is not Player otherPlayer || otherPlayer.Creature.IsDead)
		{
			return;
		}

		Player self = Owner;

		// X cost spends all of your energy (stored in CapturedXValue) before OnPlay runs,
		// so capture the target's current energy first, then hand your spent energy over:
		// a true swap of both players' pools.
		int energyYouPaid = ResolveEnergyXValue();
		int otherEnergy = otherPlayer.PlayerCombatState != null ? otherPlayer.PlayerCombatState.Energy : 0;
		otherEnergy = System.Math.Max(0, otherEnergy);

		await PlayerCmd.SetEnergy(otherEnergy, self);
		await PlayerCmd.SetEnergy(energyYouPaid, otherPlayer);

		if (!IsUpgraded)
		{
			await CreatureCmd.Damage(choiceContext, self.Creature, 4m, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, null, this);
		}
	}

	public override void OnUpgrade()
	{
		// Upgraded: no longer lose HP. Handled in OnPlay via IsUpgraded.
	}

	public override List<(string, string)>? Localization =>
	[
		("title", "Swap Energy"),
		("description", "Swap your energy with a friendly player's energy. Lose 4 HP."),
		("selectionScreenPrompt", "Select a player to swap energy with."),
	];
}