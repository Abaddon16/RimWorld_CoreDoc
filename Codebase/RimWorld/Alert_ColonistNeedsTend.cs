using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>Exposes <see cref="Alert"/>-related functions specific to Player-faction <see cref="Pawn"/>s needing medical attention</para>
	///		<para>Subclass of <see cref="Alert"/></para>
	/// </summary>
	public class Alert_ColonistNeedsTend : Alert {
		/// <summary>
		///		<para>Returns a list of all the <see cref="Pawn"/>s needing medical attention</para>
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> list of all the <see cref="Pawn"/>s needing medical attention</returns>
		private IEnumerable<Pawn> NeedingColonists {
			get {
				foreach (Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned) {
					if(p.health.HasHediffsNeedingTendByPlayer(true)) {
						Building_Bed curBed = p.CurrentBed();
						if(curBed == null || !curBed.Medical) {
							if(!Alert_ColonistNeedsRescuing.NeedsRescue(p)) {
								yield return p;
							}
						}
					}
				}
			}
		}
		/// <summary>
		///		<para>Information needed to generate an <see cref="Alert"/> that the a <see cref="Pawn"/> needs medical attention</para>
		///		<para>Sets the <see cref="Alert.defaultPriority"/> to <see cref="AlertPriority.High"/></para>
		/// </summary>
		public Alert_ColonistNeedsTend() {
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}
		/// <summary>
		///		<para>Returns a string containing the <see cref="Pawn"/>s needing medical attention</para>
		/// </summary>
		/// <returns>A string containing the <see cref="Pawn"/>s needing medical attention</returns>
		public override string GetExplanation() {
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn current in this.NeedingColonists) {
				stringBuilder.AppendLine("    " + current.LabelShort);
			}
			return "ColonistNeedsTreatmentDesc".Translate(stringBuilder.ToString());
		}
		/// <summary>
		///		<para>Return an <see cref="AlertReport"/> containing the <see cref="Pawn"/>s that need medical attention</para>
		///		<para>Override of <see cref="Alert.GetReport"/></para>
		/// </summary>
		/// <returns>An <see cref="AlertReport"/> containing the <see cref="Pawn"/>s that need medical attention</returns>
		public override AlertReport GetReport() {
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}
	}
}