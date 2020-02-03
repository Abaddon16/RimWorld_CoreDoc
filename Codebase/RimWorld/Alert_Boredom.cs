using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>Exposes <see cref="Alert"/>-related functions specific to <see cref="Pawn"/> boredom</para>
	///		<para>Subclass of <see cref="Alert"/></para>
	/// </summary>
	public class Alert_Boredom : Alert {
		private const float JoyNeedThreshold = 0.24000001f;
		/// <summary>
		///		<para>Information needed to generate an <see cref="Alert"/> that the a <see cref="Pawn"/> is bored</para>
		/// </summary>
		public Alert_Boredom() {
			this.defaultLabel="Boredom".Translate();
			this.defaultPriority=AlertPriority.Medium;
		}
		/// <summary>
		///		<para>Return an <see cref="AlertReport"/> containing all <see cref="Pawn"/>s that are bored</para>
		///		<para>Override of <see cref="Alert.GetReport"/></para>
		/// </summary>
		/// <returns>An <see cref="AlertReport"/> containing all <see cref="Pawn"/>s that are bored</returns>
		public override AlertReport GetReport() {
			return AlertReport.CulpritsAre(this.BoredPawns());
		}
		/// <summary>
		///		<para>Returns a string containing all bored <see cref="Pawn"/>s</para>
		/// </summary>
		/// <returns>A string containing all bored <see cref="Pawn"/>s</returns>
		public override string GetExplanation() {
			StringBuilder stringBuilder = new StringBuilder();
			Pawn pawn = null;
			foreach(Pawn current in this.BoredPawns()) {
				stringBuilder.AppendLine("   "+current.Label);
				if(pawn==null) {
					pawn=current;
				}
			}
			string value = JoyUtility.JoyKindsOnMapString(pawn.Map);
			return "BoredomDesc".Translate(stringBuilder.ToString().TrimEndNewlines(), pawn.LabelShort, value, pawn.Named("PAWN"));
		}
		/// <summary>
		///		<para>Returns a list of all the Bored <see cref="Pawn"/>s</para>
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> list of bored <see cref="Pawn"/>s</returns>
		[DebuggerHidden]
		private IEnumerable<Pawn> BoredPawns() {
			foreach(Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned) {
				if((p.needs.joy.CurLevelPercentage<0.24000001f||p.GetTimeAssignment()==TimeAssignmentDefOf.Joy)&&p.needs.joy.tolerances.BoredOfAllAvailableJoyKinds(p)) {
					yield return p;
				}
			}
		}
	}
}
