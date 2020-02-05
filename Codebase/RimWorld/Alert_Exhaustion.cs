using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>Exposes <see cref="Alert"/>-related functions specific to Player-faction <see cref="Pawn"/>s that are exhausted</para>
	///		<para>Subclass of <see cref="Alert"/></para>
	/// </summary>
	public class Alert_Exhaustion : Alert {
		/// <summary>
		///		<para>Returns a list of all the <see cref="Pawn"/>s that are exhausted</para>
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> list of all the <see cref="Pawn"/>s that are exhausted</returns>
		private IEnumerable<Pawn> ExhaustedColonists {
			get {
				return from p in PawnsFinder.AllMaps_FreeColonistsSpawned
					   where p.needs.rest!=null&&p.needs.rest.CurCategory==RestCategory.Exhausted
					   select p;
			}
		}
		/// <summary>
		///		<para>Information needed to generate an <see cref="Alert"/> that the a <see cref="Pawn"/> that are exhausted</para>
		///		<para>Sets the <see cref="Alert.defaultPriority"/> to <see cref="AlertPriority.High"/></para>
		/// </summary>
		public Alert_Exhaustion() {
			this.defaultLabel="Exhaustion".Translate();
			this.defaultPriority=AlertPriority.High;
		}
		/// <summary>
		///		<para>Returns a string containing the <see cref="Pawn"/>s that are exhausted</para>
		/// </summary>
		/// <returns>A string containing the <see cref="Pawn"/>s that are exhausted</returns>
		public override string GetExplanation() {
			StringBuilder stringBuilder = new StringBuilder();
			foreach(Pawn current in this.ExhaustedColonists) {
				stringBuilder.AppendLine("    "+current.LabelShort);
			}
			return "ExhaustionDesc".Translate(stringBuilder.ToString());
		}
		/// <summary>
		///		<para>Return an <see cref="AlertReport"/> containing the <see cref="Pawn"/>s that are exhausted</para>
		///		<para>Override of <see cref="Alert.GetReport"/></para>
		/// </summary>
		/// <returns>An <see cref="AlertReport"/> containing the <see cref="Pawn"/>s that are exhausted</returns>
		public override AlertReport GetReport() {
			return AlertReport.CulpritsAre(this.ExhaustedColonists);
		}
	}
}
