using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>Exposes <see cref="Alert"/>-related functions specific to Player-faction <see cref="Pawn"/>s being idle</para>
	///		<para>Subclass of <see cref="Alert"/></para>
	/// </summary>
	public class Alert_ColonistsIdle : Alert {
		public const int MinDaysPassed = 1;
		/// <summary>
		///		<para>Returns a list of all the <see cref="Pawn"/>s who have been idle</para>
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> list of all the <see cref="Pawn"/>s who have been idle</returns>
		private IEnumerable<Pawn> IdleColonists {
			get {
				List<Map> maps = Find.Maps;
				for(int i = 0; i<maps.Count; i++) {
					if(maps[i].IsPlayerHome) {
						foreach(Pawn p in maps[i].mapPawns.FreeColonistsSpawned) {
							if(p.mindState.IsIdle) {
								yield return p;
							}
						}
					}
				}
			}
		}
		/// <summary>
		///		<para>Returns a string saying how many <see cref="Pawn"/>s are idle</para>
		/// </summary>
		/// <returns>String saying how many <see cref="Pawn"/>s are idle</returns>
		public override string GetLabel() {
			return "ColonistsIdle".Translate(this.IdleColonists.Count<Pawn>().ToStringCached());
		}
		/// <summary>
		///		<para>Returns a string containing the <see cref="Pawn"/>s who have been idle</para>
		/// </summary>
		/// <returns>A string containing the <see cref="Pawn"/>s who have been idle</returns>
		public override string GetExplanation() {
			StringBuilder stringBuilder = new StringBuilder();
			foreach(Pawn current in this.IdleColonists) {
				stringBuilder.AppendLine("    "+current.LabelShort.CapitalizeFirst());
			}
			return "ColonistsIdleDesc".Translate(stringBuilder.ToString());
		}
		/// <summary>
		///		<para>Return an <see cref="AlertReport"/> containing the <see cref="Pawn"/>s who have been idle</para>
		///		<para>Override of <see cref="Alert.GetReport"/></para>
		/// </summary>
		/// <returns>An <see cref="AlertReport"/> containing the <see cref="Pawn"/>s who have been idle</returns>
		public override AlertReport GetReport() {
			if(GenDate.DaysPassed<1) {
				return false;
			}
			return AlertReport.CulpritsAre(this.IdleColonists);
		}
	}
}
