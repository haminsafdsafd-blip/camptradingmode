using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace ShopTrader;

/// <summary>
/// Common colorless multiplayer card: costs 2, exhausts your hand and a friendly
/// player's hand, then gives each of you copies of the other's exhausted hand.
/// Upgraded, it costs 0.
/// </summary>
[Pool(typeof(ColorlessCardPool))]
public sealed class SwapCards : CustomCardModel
{
	public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

	public override string? CustomPortraitPath => "res://ShopTrader/crazy.png";

	public override string PortraitPath => base.PortraitPath;

	public SwapCards()
		: base(2, CardType.Skill, CardRarity.Common, TargetType.AnyAlly, autoAdd: false)
	{
	}

	public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target?.Player is not Player otherPlayer || otherPlayer.Creature.IsDead)
		{
			return;
		}

		Player self = Owner;

		List<CardModel> myCards = PileType.Hand.GetPile(self).Cards.ToList();
		List<CardModel> theirCards = PileType.Hand.GetPile(otherPlayer).Cards.ToList();

		// Exhaust both hands first, then swap as copies: each player receives
		// copies of the cards the other just exhausted.
		foreach (CardModel card in myCards)
			await CardCmd.Exhaust(choiceContext, card);
		foreach (CardModel card in theirCards)
			await CardCmd.Exhaust(choiceContext, card);

		foreach (CardModel card in myCards)
			await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), PileType.Hand, otherPlayer);
		foreach (CardModel card in theirCards)
			await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), PileType.Hand, self);
	}

	public override void OnUpgrade()
	{
		EnergyCost.UpgradeBy(-2);
	}

	public override List<(string, string)>? Localization =>
	[
		("title", "Swap Cards"),
		("description", "Exhaust your hand and the target's hand. Each of you receives copies of the other's exhausted hand."),
		("selectionScreenPrompt", "Select a player to swap hands with."),
	];
}