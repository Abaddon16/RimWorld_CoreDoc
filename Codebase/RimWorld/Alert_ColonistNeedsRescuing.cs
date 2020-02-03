using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>Exposes <see cref="Alert"/>-related functions specific to Player-faction <see cref="Pawn"/>s needing rescue</para>
	///		<para>Subclass of <see cref="Alert"/></para>
	/// </summary>
	public class Alert_ColonistNeedsRescuing : Alert_Critical {
		/// <summary>
		///		<para>Returns a list of all the <see cref="Pawn"/>s needing rescue</para>
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> list of all the <see cref="Pawn"/>s needing rescue</returns>
		private IEnumerable<Pawn> ColonistsNeedingRescue {
			get {
				foreach(Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned) {
					if(Alert_ColonistNeedsRescuing.NeedsRescue(p)) {
						yield return p;
					}
				}
			}
		}
		/// <summary>
		///		<para>Boolean check for if <see cref="Pawn"/> needs rescue</para>
		/// </summary>
		/// <param name="p"><see cref="Pawn"/> to check if it needs rescue</param>
		/// <returns>Boolean; true if the <see cref="Pawn"/> needs rescue, false if not</returns>
		public static bool NeedsRescue(Pawn p) {
			return p.Downed&&!p.InBed()&&!(p.ParentHolder is Pawn_CarryTracker)&&(p.jobs.jobQueue==null||p.jobs.jobQueue.Count<=0||!p.jobs.jobQueue.Peek().job.CanBeginNow(p, false));
		}
		/// <summary>
		///		<para>Returns a string saying <see cref="Pawn"/>s need rescue</para>
		/// </summary>
		/// <returns>String saying a <see cref="Pawn"/>/Pawns needs rescue</returns>
		public override string GetLabel() {
			if(this.ColonistsNeedingRescue.Count<Pawn>()==1) {
				return "ColonistNeedsRescue".Translate();
			}
			return "ColonistsNeedRescue".Translate();
		}
		/// <summary>
		///		<para>Returns a string containing the <see cref="Pawn"/>s needing rescuing</para>
		/// </summary>
		/// <returns>A string containing the <see cref="Pawn"/>s needing rescuing</returns>
		public override string GetExplanation() {
			StringBuilder stringBuilder = new StringBuilder();
			foreach(Pawn current in this.ColonistsNeedingRescue) {
				stringBuilder.AppendLine("    "+current.LabelShort);
			}
			return "ColonistsNeedRescueDesc".Translate(stringBuilder.ToString());
		}
		/// <summary>
		///		<para>Return an <see cref="AlertReport"/> containing the <see cref="Pawn"/>s that need rescuing</para>
		///		<para>Override of <see cref="Alert.GetReport"/></para>
		/// </summary>
		/// <returns>An <see cref="AlertReport"/> containing the <see cref="Pawn"/>s that need rescuing</returns>
		public override AlertReport GetReport() {
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}
	}
}
