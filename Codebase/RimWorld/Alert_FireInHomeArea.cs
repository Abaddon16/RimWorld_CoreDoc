using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>Exposes <see cref="Alert"/>-related functions specific to <see cref="Fire"/> in Player-faction homes</para>
	///		<para>Subclass of <see cref="Alert_Critical"/></para>
	/// </summary>
	public class Alert_FireInHomeArea : Alert_Critical {
		/// <summary>
		///		<para>Returns the instance of any <see cref="Fire"/> fire in the Player-faction's defined home area</para>
		/// </summary>
		private Fire FireInHomeArea {
			get {
				List<Map> maps = Find.Maps;
				for(int i = 0; i<maps.Count; i++) {
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.Fire);
					for(int j = 0; j<list.Count; j++) {
						Thing thing = list[j];
						if(maps[i].areaManager.Home[thing.Position]&&!thing.Position.Fogged(thing.Map)) {
							return (Fire)thing;
						}
					}
				}
				return null;
			}
		}
		/// <summary>
		///		<para>Information needed to generate an <see cref="Alert"/> that there is a <see cref="Fire"/></para>
		/// </summary>
		public Alert_FireInHomeArea() {
			this.defaultLabel="FireInHomeArea".Translate();
			this.defaultExplanation="FireInHomeAreaDesc".Translate();
		}
		/// <summary>
		///		<para>Return an <see cref="AlertReport"/> containing the <see cref="Fire"/> in the home area</para>
		///		<para>Override of <see cref="Alert.GetReport"/></para>
		/// </summary>
		/// <returns>An <see cref="AlertReport"/> containing the <see cref="Fire"/>s in the home area</returns>
		public override AlertReport GetReport() {
			return this.FireInHomeArea;
		}
	}
}
