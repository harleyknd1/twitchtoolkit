using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows;

public class Window_ToolkitUtils : Window
{
	public Window_ToolkitUtils()
	{
		base.doCloseButton = true;
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0046: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_007d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_008f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard listing = new Listing_Standard();
		((Listing)listing).Begin(inRect);
		listing.Label("Toolkit has detected ToolkitUtils is not active.");
		listing.Label("It is highly recommended to have ToolkitUtils Installed and Active.");
		Text.Font =((GameFont)2);
		listing.Label("Features");
		Text.Font =((GameFont)1);
		((Listing)listing).GapLine(12f);
		listing.Label("* Improved UI");
		listing.Label("* More Events");
		listing.Label("* More Commands");
		listing.Label("* Tweaks to Toolkit");
		listing.Label("...And More!");
		((Listing)listing).Gap(12f);
		if (listing.ButtonText("ToolkitUtils Workshop Page", (string)null))
		{
			Application.OpenURL("steam://url/CommunityFilePage/2009500580");
		}
		((Listing)listing).End();
	}
}
