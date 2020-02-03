using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>Exposes <see cref="Alert"/>-related functions specific to alerts about Billiard Tables having unusable space.</para>
	///		<para>Subclass of <see cref="Alert"/></para>
	/// </summary>
	public class Alert_BilliardsTableOnWall : Alert {
		/// <summary>
		///		<para>A list of all the Player Faction Billiard Tables with unusable space around them</para>
		/// </summary>
		private IEnumerable<Thing> BadTables {
			get {
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for(int i = 0; i<maps.Count; i++) {
					List<Thing> bList = maps[i].listerThings.ThingsOfDef(ThingDefOf.BilliardsTable);
					for(int j = 0; j<bList.Count; j++) {
						if(bList[j].Faction==ofPlayer&&!JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(bList[j])) {
							yield return bList[j];
						}
					}
				}
			}
		}
		/// <summary>
		///		<para>Information needed to generate an <see cref="Alert"/> that the Billiard Table has unusable space</para>
		/// </summary>
		public Alert_BilliardsTableOnWall() {
			this.defaultLabel="BilliardsNeedsSpace".Translate();
			this.defaultExplanation="BilliardsNeedsSpaceDesc".Translate();
		}
		/// <summary>
		///		<para>Return an <see cref="AlertReport"/> containing the Billiard Tables that have blocked spaces</para>
		///		<para>Override of <see cref="Alert.GetReport"/></para>
		/// </summary>
		/// <returns>An <see cref="AlertReport"/> containing the Billiard Tables that have blocked spaces</returns>
		public override AlertReport GetReport() {
			return AlertReport.CulpritsAre(this.BadTables);
		}
	}
}
