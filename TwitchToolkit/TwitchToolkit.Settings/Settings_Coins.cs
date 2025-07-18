using System;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings;

public static class Settings_Coins
{
	public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
	{
        optionsListing.AddLabeledNumericalTextField("TwitchToolkitStartingBalance".Translate(), ref ToolkitSettings.StartingBalance, 0.8f);
		optionsListing.SliderLabeled("TwitchToolkitCoinInterval".Translate(),  ref ToolkitSettings.CoinInterval, Math.Round((float)ToolkitSettings.CoinInterval).ToString(), 1f, 15f);
		optionsListing.SliderLabeled("TwitchToolkitCoinAmount".Translate(), ref ToolkitSettings.CoinAmount, Math.Round((float)ToolkitSettings.CoinAmount).ToString());
		optionsListing.AddLabeledNumericalTextField("TwitchToolkitMinimumPurchasePrice".Translate(), ref ToolkitSettings.MinimumPurchasePrice, 0.8f);
		optionsListing.Gap(12f);
		optionsListing.CheckboxLabeled("TwitchToolkitUnlimitedCoins".Translate(), ref ToolkitSettings.UnlimitedCoins, null);
		optionsListing.GapLine(12f);
		optionsListing.Gap(12f);
		optionsListing.CheckboxLabeled("TwitchToolkitChatReqsForCoins".Translate(), ref ToolkitSettings.ChatReqsForCoins, null);
		optionsListing.SliderLabeled("TwitchToolkitTimeBeforeHalfCoins".Translate(),  ref ToolkitSettings.TimeBeforeHalfCoins, Math.Round((double)ToolkitSettings.TimeBeforeHalfCoins).ToString(), 15f, 120f);
		optionsListing.SliderLabeled("TwitchToolkitTimeBeforeNoCoins".Translate(),  ref ToolkitSettings.TimeBeforeNoCoins, Math.Round((double)ToolkitSettings.TimeBeforeNoCoins).ToString(), 30f, 240f);
	}
}
