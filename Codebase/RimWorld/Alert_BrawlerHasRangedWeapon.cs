using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>Exposes <see cref="Alert"/>-related functions specific to <see cref="Pawn"/>s with <see cref="TraitDefOf.Brawler"/> holding ranged weapons</para>
	///		<para>Subclass of <see cref="Alert"/></para>
	/// </summary>
	public class Alert_BrawlerHasRangedWeapon : Alert {
		/// <summary>
		///		<para>Returns a list of all the <see cref="Pawn"/>s with <see cref="TraitDefOf.Brawler"/> holding a ranged weapon</para>
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> list of <see cref="Pawn"/>s with <see cref="TraitDefOf.Brawler"/> holding ranged weapons</returns>
		private IEnumerable<Pawn> BrawlersWithRangedWeapon {
			get {
				foreach(Pawn p in PawnsFinder.AllMaps_FreeColonistsSpawned) {
					if(p.story.traits.HasTrait(TraitDefOf.Brawler)&&p.equipment.Primary!=null&&p.equipment.Primary.def.IsRangedWeapon) {
						yield return p;
					}
				}
			}
		}
		/// <summary>
		///		<para>Information needed to generate an <see cref="Alert"/> that the a <see cref="Pawn"/> with <see cref="TraitDefOf.Brawler"/> has a ranged weapon</para>
		/// </summary>
		public Alert_BrawlerHasRangedWeapon() {
			this.defaultLabel="BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation="BrawlerHasRangedWeaponDesc".Translate();
		}
		/// <summary>
		///		<para>Return an <see cref="AlertReport"/> containing all Brawlers with ranged weapons</para>
		///		<para>Override of <see cref="Alert.GetReport"/></para>
		/// </summary>
		/// <returns>An <see cref="AlertReport"/> containing all Brawlers with ranged weapons</returns>
		public override AlertReport GetReport() {
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}
	}
}
