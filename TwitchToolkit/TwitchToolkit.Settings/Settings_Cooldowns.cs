using System;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings;

public static class Settings_Cooldowns
{
	public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0094: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00be: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing erences)
		//IL_011e: Unknown result type (might be due to invalid IL or missing erences)
		optionsListing.SliderLabeled("Days per cooldown period",  ref ToolkitSettings.EventCooldownInterval, Math.Round((double)ToolkitSettings.EventCooldownInterval).ToString(), 1f, 15f);
		optionsListing.Gap(12f);
		optionsListing.CheckboxLabeled("TwitchToolkitMaxEventsLimit".Translate(), ref ToolkitSettings.MaxEvents, null);
		optionsListing.Gap(12f);
		optionsListing.AddLabeledNumericalTextField("TwitchToolkitMaxBadEvents".Translate(), ref ToolkitSettings.MaxBadEventsPerInterval, 0.8f);
		optionsListing.AddLabeledNumericalTextField("TwitchToolkitMaxGoodEvents".Translate(), ref ToolkitSettings.MaxGoodEventsPerInterval, 0.8f);
		optionsListing.AddLabeledNumericalTextField("TwitchToolkitMaxNeutralEvents".Translate(), ref ToolkitSettings.MaxNeutralEventsPerInterval, 0.8f);
		optionsListing.AddLabeledNumericalTextField("TwitchToolkitMaxItemEvents".Translate(), ref ToolkitSettings.MaxCarePackagesPerInterval, 0.8f);
		optionsListing.Gap(12f);
		optionsListing.CheckboxLabeled("TwitchToolkitEventsHaveCooldowns".Translate(), ref ToolkitSettings.EventsHaveCooldowns, null);
	}
}
