using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.Store;

[StaticConstructorOnStartup]
public static class Purchase_Handler
{
	public static List<StoreIncidentSimple> allStoreIncidentsSimple;

	public static List<StoreIncidentVariables> allStoreIncidentsVariables;

	public static List<string> viewerNamesDoingVariableCommands;

	static Purchase_Handler()
	{
		allStoreIncidentsSimple = DefDatabase<StoreIncidentSimple>.AllDefs.ToList();
		allStoreIncidentsVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList();
		viewerNamesDoingVariableCommands = new List<string>();

		Helper.Log("trying to load vars after def database loaded");
		Toolkit.Mod.GetSettings<ToolkitSettings>();
	}

	public static void ResolvePurchase(Viewer viewer, ITwitchMessage twitchMessage, bool separateChannel = false)
	{
		Log.Warning("reached purchase resolver");
		List<string> command = twitchMessage.Message.Split(' ').ToList();
		if (command.Count < 2)
		{
			return;
		}
		if (command[0] == "!levelskill")
		{
			command[0] = "levelskill";
			command.Insert(0, "!buy");
		}
		string productKey = command[1].ToLower();
		string formattedMessage = string.Join(" ", command.ToArray());
		StoreIncidentSimple incident = allStoreIncidentsSimple.Find(s => productKey.ToLower() == s.abbreviation);
		if (incident != null)
		{
			ResolvePurchaseSimple(viewer, twitchMessage, incident, formattedMessage);
			return;
		}
		StoreIncidentVariables incidentVariables = allStoreIncidentsVariables.Find((StoreIncidentVariables s) => productKey.ToLower() == s.abbreviation);
		if (incidentVariables != null)
		{
			ResolvePurchaseVariables(viewer, twitchMessage, incidentVariables, formattedMessage);
			return;
		}
		Item item = StoreInventory.items.Find((Item s) => s.abr == productKey);
		Helper.Log("abr: " + productKey + " ");
		if (item != null)
		{
			List<string> commandSplit = twitchMessage.Message.Split(' ').ToList();
			commandSplit.Insert(1, "item");
			if (commandSplit.Count < 4)
			{
				commandSplit.Add("1");
			}
			if (!int.TryParse(commandSplit[3], out var _))
			{
				commandSplit.Insert(3, "1");
			}
			formattedMessage = string.Join(" ", commandSplit.ToArray());
			ResolvePurchaseVariables(viewer, twitchMessage, StoreIncidentDefOf.Item, formattedMessage);
		}
	}

	public static void ResolvePurchaseSimple(Viewer viewer, ITwitchMessage twitchMessage, StoreIncidentSimple incident, string formattedMessage, bool separateChannel = false)
	{
		int cost = incident.cost;
		if (cost < 1 || CheckIfViewerIsInVariableCommandList(viewer.username) || !CheckIfViewerHasEnoughCoins(viewer, cost) || IsPurchaseMaxed(incident, viewer.username))
		{
			return;
		}
		IncidentHelper helper = StoreIncidentMaker.MakeIncident(incident);
		if (helper == null)
		{
			Helper.Log("Missing helper for incident " + incident.defName);
			return;
		}
		if (!helper.IsPossible())
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " " + "TwitchToolkitEventNotPossible".Translate());
			return;
		}
		if (!ToolkitSettings.UnlimitedCoins)
		{
			viewer.TakeViewerCoins(cost);
		}
		Store_Component component = Current.Game.GetComponent<Store_Component>();
		helper.Viewer = viewer;
		helper.message = formattedMessage;
		Ticker.IncidentHelpers.Enqueue(helper);
		Store_Logger.LogPurchase(viewer.username, formattedMessage);
		component.LogIncident(incident);
		viewer.CalculateNewKarma(incident.karmaType, cost);
		if (ToolkitSettings.PurchaseConfirmations)
		{
			TwitchWrapper.SendChatMessage(Helper.ReplacePlaceholder("TwitchToolkitEventPurchaseConfirm".Translate(), first: GenText.CapitalizeFirst(incident.label), viewer: viewer.username));
		}
	}

	public static void ResolvePurchaseVariables(Viewer viewer, ITwitchMessage twitchMessage, StoreIncidentVariables incident, string formattedMessage, bool separateChannel = false)
	{
		int cost = incident.cost;
		if ((cost < 1 && !incident.IsItem) || CheckIfViewerIsInVariableCommandList(viewer.username) || !CheckIfViewerHasEnoughCoins(viewer, cost))
		{
			return;
		}
		if (IsPurchaseMaxed(incident, viewer.username))
		{
			return;
		}
		viewerNamesDoingVariableCommands.Add(viewer.username);
		IncidentHelperVariables helper = StoreIncidentMaker.MakeIncidentVariables(incident);
		if (helper == null)
		{
			Helper.Log("Missing helper for incident " + incident.defName);
			return;
		}
		if (!helper.IsPossible(formattedMessage, viewer))
		{
			if (viewerNamesDoingVariableCommands.Contains(viewer.username))
			{
				viewerNamesDoingVariableCommands.Remove(viewer.username);
			}
			return;
		}
		Store_Component component = Current.Game.GetComponent<Store_Component>();
		helper.Viewer = viewer;
		helper.message = formattedMessage;
		Ticker.IncidentHelperVariables.Enqueue(helper);
		Store_Logger.LogPurchase(viewer.username, twitchMessage.Message);
		component.LogIncident(incident);
	}

	public static bool CheckIfViewerIsInVariableCommandList(string username, bool separateChannel = false)
	{
		if (viewerNamesDoingVariableCommands.Contains(username))
		{
			TwitchWrapper.SendChatMessage("@" + username + " you must wait for the game to unpause to buy something else.");
			return true;
		}

		return false;
	}

	public static bool CheckIfViewerHasEnoughCoins(Viewer viewer, int finalPrice, bool separateChannel = false)
	{
		if (!ToolkitSettings.UnlimitedCoins && viewer.GetViewerCoins() < finalPrice)
		{
			TwitchWrapper.SendChatMessage(Helper.ReplacePlaceholder("TwitchToolkitNotEnoughCoins".Translate(), viewer: viewer.username, amount: finalPrice.ToString(), first: viewer.GetViewerCoins().ToString()));
			return false;
		}

		return true;
	}

	[Obsolete]
	public static bool CheckIfKarmaTypeIsMaxed(StoreIncident incident, string username, bool separateChannel = false)
	{
		if (CheckTimesKarmaTypeHasBeenUsedRecently(incident))
		{
			Store_Component component = Current.Game.GetComponent<Store_Component>();

			float daysTill = component.DaysTillIncidentIsPurchaseable(incident);
			TwitchWrapper.SendChatMessage($"@{username} {GenText.CapitalizeFirst(incident.label)} is maxed from karmatype, wait {GetDays(daysTill)} to purchase.");
			return true;
		}

		return false;
	}

	public static bool CheckTimesKarmaTypeHasBeenUsedRecently(StoreIncident incident)
	{
		if (!ToolkitSettings.MaxEvents)
		{
			return false;
		}

		Store_Component component = Current.Game.GetComponent<Store_Component>();
		int cap = Karma.GetKarmaCap(incident.karmaType);
		return component.KarmaTypesInLogOf(incident.karmaType) >= cap;
	}

	[Obsolete]
	public static bool CheckIfCarePackageIsOnCooldown(string username, bool separateChannel = false)
	{
		if (!ToolkitSettings.MaxEvents)
		{
			return false;
		}

		Store_Component component = Current.Game.GetComponent<Store_Component>();
		StoreIncidentVariables incident = DefDatabase<StoreIncidentVariables>.GetNamed("Item", true);
		if (component.IncidentsInLogOf(incident.abbreviation) >= ToolkitSettings.MaxCarePackagesPerInterval)
		{
			float daysTill = component.DaysTillIncidentIsPurchaseable(incident);
			TwitchWrapper.SendChatMessage($"@{username} care packages are on cooldown, wait {GetDays(daysTill)}.");
			return true;
		}

		return false;
	}

	[Obsolete]
	public static bool CheckIfIncidentIsOnCooldown(StoreIncident incident, string username, bool separateChannel = false)
	{
		if (!ToolkitSettings.EventsHaveCooldowns)
		{
			return false;
		}

		Store_Component component = Current.Game.GetComponent<Store_Component>();
		if (component.IncidentsInLogOf(incident.abbreviation) >= incident.eventCap)
		{
			float daysTill = component.DaysTillIncidentIsPurchaseable(incident);
			TwitchWrapper.SendChatMessage($"@{username} {GenText.CapitalizeFirst(incident.label)} is maxed, wait {GetDays(daysTill)} to purchase.");
			return true;
		}

		return false;
	}

	public static void QueuePlayerMessage(Viewer viewer, string message, int variables = 0)
	{
		string colorCode = Viewer.GetViewerColorCode(viewer.username);
		string userNameTag = "<color=#" + colorCode + ">" + viewer.username + "</color>";
		string[] command = message.Split(' ');
		string output = "\n\n";
		if (command.Length - 2 == variables)
		{
			output = output + "<i>from</i> " + userNameTag;
		}
		else
		{
			output = output + userNameTag + ":";
			for (int i = 2 + variables; i < command.Length; i++)
			{
				output = ((!(viewer.username.ToLower() == "hodlhodl") && !(viewer.username.ToLower() == "sirrandoo") && !(viewer.username.ToLower() == "saschahi")) ? ((!viewer.IsSub) ? ((!viewer.IsVIP) ? ((!viewer.mod) ? (output + " " + command[i]) : (output + " " + ModText(command[i]))) : (output + " " + VIPText(command[i]))) : (output + " " + SubText(command[i]))) : (output + " " + AdminText(command[i])));
			}
		}
		Helper.playerMessages.Add(output);
	}

	private static bool IsPurchaseMaxed(StoreIncident incident, string username)
	{
		Store_Component component = Current.Game.GetComponent<Store_Component>();

		float daysTill = component.DaysTillIncidentIsPurchaseable(incident, out bool isKarma);
		if (daysTill < 0)
		{
			return false;
		}

		string days = GetDays(daysTill);

		if (isKarma)
		{
			TwitchWrapper.SendChatMessage($"@{username} {GenText.CapitalizeFirst(incident.label)} is maxed from karmatype, wait {days} to purchase.");
		}
		else if (incident.IsItem)
		{
			TwitchWrapper.SendChatMessage($"@{username} care packages are on cooldown, wait {days}.");
		}
		else
		{
			TwitchWrapper.SendChatMessage($"@{username} {GenText.CapitalizeFirst(incident.label)} is maxed, wait {days} to purchase.");
		}

		return true;
	}

	private static string GetDays(float days) => (days == 1) ? "1 day" : days + " days";

	private static string AdminText(string input)
	{
		StringBuilder output = new StringBuilder();
		foreach (char str in input.ToCharArray())
		{
			output.Append($"<color=#{Helper.GetRandomColorCode()}>{str}</color>");
		}
		return output.ToString();
	}

	private static string SubText(string input) => "<color=#D9BB25>" + input + "</color>";

	private static string VIPText(string input) => "<color=#5F49F2>" + input + "</color>";

	private static string ModText(string input) => "<color=#238C48>" + input + "</color>";
}
