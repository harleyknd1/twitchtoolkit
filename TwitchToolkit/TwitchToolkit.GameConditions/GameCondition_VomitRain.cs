using System;
using RimWorld;
using Verse;

namespace TwitchToolkit.GameConditions;

public class GameCondition_VomitRain : GameCondition_Flashstorm
{
	public override void GameConditionTick()
	{
		IntVec3 newFilthLoc = CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 sq) => GenGrid.Standable(sq, this.AffectedMaps[0]) && !this.AffectedMaps[0].roofGrid.Roofed(sq)), this.AffectedMaps[0], 1000);
		FilthMaker.TryMakeFilth(newFilthLoc, this.AffectedMaps[0], ThingDefOf.Filth_Vomit, 1, (FilthSourceFlags)0);
	}
}
