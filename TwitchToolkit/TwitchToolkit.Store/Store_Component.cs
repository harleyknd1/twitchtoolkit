using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.Store;

public class Store_Component : GameComponent
{
	private static int EventCooldownInDays => GenDate.TicksPerDay * ToolkitSettings.EventCooldownInterval;

	public int lastID = 0;

	public Dictionary<int, int> tickHistory = new Dictionary<int, int>();

	public Dictionary<int, string> abbreviationHistory = new Dictionary<int, string>();

	public Dictionary<int, string> karmaHistory = new Dictionary<int, string>();

	public Store_Component(Game game)
	{
	}

	public override void GameComponentTick()
	{
		if (Find.TickManager.TicksGame % 1000 == 0)
		{
			CleanLog();
		}
	}

	public void CleanLog()
	{
		int currentTick = Find.TickManager.TicksGame;
		List<int> toRemove = new List<int>();

		foreach (KeyValuePair<int, int> pair in tickHistory)
		{
			if (pair.Value + EventCooldownInDays < currentTick)
			{
				toRemove.Add(pair.Key);
			}
		}

		foreach (int i in toRemove)
		{
			tickHistory.Remove(i);
			abbreviationHistory.Remove(i);
			karmaHistory.Remove(i);
		}
	}

	public int IncidentsInLogOf(string abbreviation)
	{
		return abbreviationHistory.Count((KeyValuePair<int, string> pair) => pair.Value == abbreviation);
	}

	public int KarmaTypesInLogOf(KarmaType karmaType)
	{
		var karma = karmaType.ToString();
		return karmaHistory.Count((KeyValuePair<int, string> pair) => pair.Value == karma);
	}

	public float DaysTillIncidentIsPurchaseable(StoreIncident incident) => DaysTillIncidentIsPurchaseable(incident, out _);

	public float DaysTillIncidentIsPurchaseable(StoreIncident incident, out bool isKarmaCapped)
	{
		int karmaTick = GetKarmaOrItemCooldown(incident);
		int capTick = GetIncidentCooldown(incident);

		int cooldownTick;
		if (karmaTick >= capTick)
		{
			cooldownTick = karmaTick;
			isKarmaCapped = true;
		}
		else
		{
			cooldownTick = capTick;
			isKarmaCapped = false;
		}

		if (cooldownTick < 0)
		{
			return -1;
		}

		int currentTick = Find.TickManager.TicksGame;
		float expirationDays = ((float)(cooldownTick + EventCooldownInDays - currentTick)) / GenDate.TicksPerDay;
		return (float)Math.Round(expirationDays, 1);
	}

	public void LogIncident(StoreIncident incident)
	{
		int currentTick = Find.TickManager.TicksGame;
		int logID = lastID;

		tickHistory.Add(logID, currentTick);
		abbreviationHistory.Add(logID, incident.abbreviation);
		karmaHistory.Add(logID, incident.karmaType.ToString());

		lastID++;
	}

	public override void ExposeData()
	{
		Scribe_Values.Look(ref lastID, "logID", 0);
		Scribe_Collections.Look(ref tickHistory, "tickHistory", LookMode.Value, LookMode.Value);
		Scribe_Collections.Look(ref abbreviationHistory, "incidentHistory", LookMode.Value, LookMode.Value);
		Scribe_Collections.Look(ref karmaHistory, "karmaHistory", LookMode.Value, LookMode.Value);
	}

	private int GetKarmaOrItemCooldown(StoreIncident incident)
	{
		if (!ToolkitSettings.MaxEvents)
		{
			return -1;
		}

		if (incident.IsItem)
		{
			return FindMinIfCapped(abbreviationHistory, incident.abbreviation, ToolkitSettings.MaxCarePackagesPerInterval);
		}

		return FindMinIfCapped(karmaHistory, incident.karmaType.ToString(), Karma.GetKarmaCap(incident.karmaType));
	}

	private int GetIncidentCooldown(StoreIncident incident)
	{
		if (!ToolkitSettings.EventsHaveCooldowns)
		{
			return -1;
		}

		return FindMinIfCapped(abbreviationHistory, incident.abbreviation, incident.eventCap);
	}

	private int FindMinIfCapped(IEnumerable<KeyValuePair<int, string>> collection, string criteria, int needed)
	{
		int count = 0;
		int minTick = int.MaxValue;

		foreach (KeyValuePair<int, string> pair in collection)
		{
			if (pair.Value == criteria)
			{
				count++;
				minTick = Math.Min(minTick, tickHistory[pair.Key]);
			}
		}

		if (count >= needed)
		{
			return minTick;
		}

		return -1;
	}
}
