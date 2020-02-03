using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld {
	/// <summary>
	/// 	<para>Exposes Age-related Injury functions</para>
	/// </summary>
	[HasDebugOutput]
	public static class AgeInjuryUtility {
		private const int MaxPermanentInjuryAge = 100;
		private static List<Thing> emptyIngredientsList = new List<Thing>();
		/// <summary>
		///		<para>Returns a random <see cref="Hediff"/> for the given <see cref="Pawn"/></para>
		/// 	<para>Calls <see cref="RandomHediffsToGainOnBirthday(ThingDef, int)" /></para>
		/// </summary>
		/// <param name="pawn"></param>
		/// <param name="age">Integer age of the <see cref="Pawn"/> in question</param>
		/// <returns></returns>
		public static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(Pawn pawn, int age) {
			return AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn.def, age);
		}
		/// <summary>
		/// 	<para>Returns a random set of <see cref="Hediff"/>s for the given <see cref="ThingDef"/> </para>
		/// </summary>
		/// <param name="raceDef"><see cref="ThingDef"/> to get a set of <see cref="Hediff"/>s for</param>
		/// <param name="age">Integer age of the <see cref="Thing"/> in question</param>
		/// <returns>An <see cref="IEnumerable{T}"/> list of the <see cref="Hediff"/>s to be given</returns>
		[DebuggerHidden]
		private static IEnumerable<HediffGiver_Birthday> RandomHediffsToGainOnBirthday(ThingDef raceDef, int age) {
			List<HediffGiverSetDef> sets = raceDef.race.hediffGiverSets;
			if(sets != null) {
				for(int i = 0; i < sets.Count; i++) {
					List<HediffGiver> givers = sets[i].hediffGivers;
					for(int j = 0; j < givers.Count; j++) {
						HediffGiver_Birthday agb = givers[j] as HediffGiver_Birthday;
						if(agb != null) {
							float ageFractionOfLifeExpectancy = (float) age / raceDef.race.lifeExpectancy;
							if(Rand.Value < agb.ageFractionChanceCurve.Evaluate(ageFractionOfLifeExpectancy)) {
								yield return agb;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// 	<para>Gives a random <see cref="Hediff"/> inflicted by the old age of the <see cref="Pawn"/>.</para>
		/// 	<para>Given a Boolean on whether to try and kill the Pawn or not.</para>
		/// </summary>
		/// <param name="pawn"></param>
		/// <param name="tryNotToKillPawn"></param>
		public static void GenerateRandomOldAgeInjuries(Pawn pawn, bool tryNotToKillPawn) {
			float num = (!pawn.RaceProps.IsMechanoid) ? pawn.RaceProps.lifeExpectancy : 2500f;
			float num2 = num / 8f;
			float b = num * 1.5f;
			float chance = (!pawn.RaceProps.Humanlike) ? 0.03f : 0.15f;
			int num3 = 0;
			for(float num4 = num2; num4 < Mathf.Min((float) pawn.ageTracker.AgeBiologicalYears, b); num4 += num2) {
				if(Rand.Chance(chance)) {
					num3++;
				}
			}
			for(int i = 0; i < num3; i++) {
				IEnumerable<BodyPartRecord> source = from x in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where x.depth == BodyPartDepth.Outside && (x.def.permanentInjuryChanceFactor != 0f || x.def.pawnGeneratorCanAmputate) && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x)
				select x;
				if(source.Any<BodyPartRecord>()) {
					BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
					DamageDef dam = AgeInjuryUtility.RandomPermanentInjuryDamageType(bodyPartRecord.def.frostbiteVulnerability > 0f && pawn.RaceProps.ToolUser);
					HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dam, pawn, bodyPartRecord);
					if(bodyPartRecord.def.pawnGeneratorCanAmputate && Rand.Chance(0.3f)) {
						Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart) HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, null);
						hediff_MissingPart.lastInjury = hediffDefFromDamage;
						hediff_MissingPart.Part = bodyPartRecord;
						hediff_MissingPart.IsFresh = false;
						if(!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_MissingPart)) {
							pawn.health.AddHediff(hediff_MissingPart, bodyPartRecord, null, null);
							if(pawn.RaceProps.Humanlike && bodyPartRecord.def == BodyPartDefOf.Leg && Rand.Chance(0.5f)) {
								RecipeDefOf.InstallPegLeg.Worker.ApplyOnPawn(pawn, bodyPartRecord, null, AgeInjuryUtility.emptyIngredientsList, null);
							}
						}
					}
					else if(bodyPartRecord.def.permanentInjuryChanceFactor > 0f && hediffDefFromDamage.HasComp(typeof(HediffComp_GetsPermanent))) {
						Hediff_Injury hediff_Injury = (Hediff_Injury) HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
						hediff_Injury.Severity = (float) Rand.RangeInclusive(2, 6);
						hediff_Injury.TryGetComp<HediffComp_GetsPermanent>().IsPermanent = true;
						hediff_Injury.Part = bodyPartRecord;
						if(!tryNotToKillPawn || !pawn.health.WouldDieAfterAddingHediff(hediff_Injury)) {
							pawn.health.AddHediff(hediff_Injury, bodyPartRecord, null, null);
						}
					}
				}
			}
			for(int j = 1; j < pawn.ageTracker.AgeBiologicalYears; j++) {
				foreach (HediffGiver_Birthday current in AgeInjuryUtility.RandomHediffsToGainOnBirthday(pawn, j)) {
					current.TryApplyAndSimulateSeverityChange(pawn, (float) j, tryNotToKillPawn);
					if(pawn.Dead) {
						break;
					}
				}
				if(pawn.Dead) {
					break;
				}
			}
		}
		/// <summary>
		///		<para>Returns a random injury of <see cref="DamageDefOf"/> type; can include <see cref="DamageDefOf.Frostbite"/> or not</para>
		/// </summary>
		/// <param name="allowFrostbite"></param>
		/// <returns>The <see cref="DamageDef"/> randomly selected</returns>
		private static DamageDef RandomPermanentInjuryDamageType(bool allowFrostbite) {
			switch (Rand.RangeInclusive(0, 3 + ((!allowFrostbite) ? 0 : 1))) {
				case 0:
					return DamageDefOf.Bullet;
				case 1:
					return DamageDefOf.Scratch;
				case 2:
					return DamageDefOf.Bite;
				case 3:
					return DamageDefOf.Stab;
				case 4:
					return DamageDefOf.Frostbite;
				default:
					throw new Exception();
			}
		}
		/// <summary>
		///		<para>Generates a list of age-related injuries</para>
		///		<para>Prints a list of theoretical injuries (given 1000 iterations, but no duplicates)</para>
		///		<para>Prints a list of the actual injuries, based on only gaining one injury per year after 40 years old</para>
		/// </summary>
		[DebugOutput]
		public static void PermanentInjuryCalculations() {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Theoretical injuries=========");
			for(int i = 0; i < 10; i++) {
				stringBuilder.AppendLine("#" + i + ":");
				List<HediffDef> list = new List<HediffDef>();
				for(int j = 0; j < 100; j++) {
					foreach (HediffGiver_Birthday current in AgeInjuryUtility.RandomHediffsToGainOnBirthday(ThingDefOf.Human, j)) {
						if(!list.Contains(current.hediff)) {
							list.Add(current.hediff);
							stringBuilder.AppendLine(string.Concat(new object[] {
								"  age ",
								j,
								" - ",
								current.hediff
							}));
						}
					}
				}
			}
			Log.Message(stringBuilder.ToString(), false);
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("=======Actual injuries=========");
			for(int k = 0; k < 200; k++) {
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				if(pawn.ageTracker.AgeBiologicalYears >= 40) {
					stringBuilder.AppendLine(pawn.Name + " age " + pawn.ageTracker.AgeBiologicalYears);
					foreach (Hediff current2 in pawn.health.hediffSet.hediffs) {
						stringBuilder.AppendLine(" - " + current2);
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			Log.Message(stringBuilder.ToString(), false);
		}
	}
}