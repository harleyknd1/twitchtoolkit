using System;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings;

public static class Settings_Store
{
	public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0023: Unknown result type (might be due to invalid IL or missing erences)
		//IL_010f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_012b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0147: Unknown result type (might be due to invalid IL or missing erences)
		optionsListing.CheckboxLabeled("TwitchToolkitEarningCoins".Translate(), ref ToolkitSettings.EarningCoins, null);
		optionsListing.AddLabeledTextField("TwitchToolkitCustomPricingLink".Translate(), ref ToolkitSettings.CustomPricingSheetLink);
		optionsListing.Gap(12f);
		optionsListing.GapLine(12f);
		if (optionsListing.ButtonTextLabeled("Items Edit", "Open"))
		{
			Type type2 = typeof(StoreItemsWindow);
			Find.WindowStack.TryRemove(type2, true);
			Window window2 = new StoreItemsWindow();
			Find.WindowStack.Add(window2);
		}
		optionsListing.Gap(12f);
		optionsListing.GapLine(12f);
		if (optionsListing.ButtonTextLabeled("Events Edit", "Open"))
		{
			Type type = typeof(StoreIncidentsWindow);
			Find.WindowStack.TryRemove(type, true);
			Window window = new StoreIncidentsWindow();
			Find.WindowStack.Add(window);
		}
		optionsListing.Gap(12f);
		optionsListing.GapLine(12f);
		optionsListing.CheckboxLabeled("TwitchToolkitPurchaseConfirmations".Translate(), ref ToolkitSettings.PurchaseConfirmations, null);
		optionsListing.CheckboxLabeled("TwitchToolkitRepeatViewerNames".Translate(), ref ToolkitSettings.RepeatViewerNames, null);
		optionsListing.CheckboxLabeled("TwitchToolkitMinifiableBuildings".Translate(), ref ToolkitSettings.MinifiableBuildings, null);
	}
}
