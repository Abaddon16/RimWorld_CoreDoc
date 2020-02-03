using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>Exposes <see cref="Alert"/>-related functions specific to Awaiting Medical Operation alerts.</para>
	///		<para>Subclass of <see cref="Alert"/></para>
	/// </summary>
	public class Alert_AwaitingMedicalOperation : Alert {
		/// <summary>
		///		<para>An <see cref="IEnumerable{T}"/> list of all <see cref="Pawn"/>s on the map that should have surgery done now, and are in a bed</para>
		/// </summary>
		private IEnumerable<Pawn> AwaitingMedicalOperation {
			get {
				return from p in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer).Concat(PawnsFinder.AllMaps_PrisonersOfColonySpawned)
					   where HealthAIUtility.ShouldHaveSurgeryDoneNow(p)&&p.InBed()
					   select p;
			}
		}
		/// <summary>
		///		<para>Returns a string containing a count of how many <see cref="Pawn"/>s need an operation</para>
		/// </summary>
		/// <returns>A string containing a count of how many <see cref="Pawn"/>s need an operation</returns>
		public override string GetLabel() {
			return "PatientsAwaitingMedicalOperation".Translate(this.AwaitingMedicalOperation.Count<Pawn>().ToStringCached());
		}
		/// <summary>
		///		<para>Returns a string containing the <see cref="Pawn"/>s needing an operation</para>
		/// </summary>
		/// <returns>A string containing the <see cref="Pawn"/>s needing an operation</returns>
		public override string GetExplanation() {
			StringBuilder stringBuilder = new StringBuilder();
			foreach(Pawn current in this.AwaitingMedicalOperation) {
				stringBuilder.AppendLine("    "+current.LabelShort.CapitalizeFirst());
			}
			return "PatientsAwaitingMedicalOperationDesc".Translate(stringBuilder.ToString());
		}
		/// <summary>
		///		<para>Return an <see cref="AlertReport"/> containing the <see cref="Pawn"/>s that need an operation</para>
		///		<para>Override of <see cref="Alert.GetReport"/></para>
		/// </summary>
		/// <returns>An <see cref="AlertReport"/> containing the <see cref="Pawn"/>s that need an operation</returns>
		public override AlertReport GetReport() {
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}
	}
}
