using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld {
    /// <summary>
    ///		Exposes Addiction-related functions.
    /// </summary>
    public static class AddictionUtility {
        /// <summary>
        ///		<para>Checks if a given <see cref="Pawn"/> is addicted to the specified <see cref="Thing"/></para>
        ///		<para>Calls <see cref="FindAddictionHediff(Pawn, Thing)"/></para>
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="drug"><see cref="Thing"/> to check if addicted to</param>
        /// <returns>true if <see cref="Pawn"/> is addicted, false if not</returns>
        public static bool IsAddicted(Pawn pawn, Thing drug) {
            return AddictionUtility.FindAddictionHediff(pawn, drug) != null;
        }
        /// <summary>
        ///		<para>Checks if a given <see cref="Pawn"/> is addicted to the specified <see cref="ChemicalDef"/></para>
        ///		<para>Calls <see cref="FindAddictionHediff(Pawn, ChemicalDef)"/></para>
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="chemical"><see cref="ChemicalDef"/> to check if addicted to</param>
        /// <returns>true if <see cref="Pawn"/> is addicted, false if not</returns>
        public static bool IsAddicted(Pawn pawn, ChemicalDef chemical) {
            return AddictionUtility.FindAddictionHediff(pawn, chemical) != null;
        }
        /// <summary>
        ///		<para>Given a <see cref="Thing"/>, find the <see cref="Hediff"/> related to it's addiction.</para>
        ///		<para>Does checks on <see cref="Thing"/> to see if it's a drug.</para>
        ///		<list type="bullet">
        ///			<item>If not, or is not addictive, returns null.</item>
        ///			<item>If it is, calls <see cref="FindAddictionHediff(Pawn, ChemicalDef)"/></item>
        ///		</list>
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="drug"><see cref="Thing"/> to look for a <see cref="Hediff"/> of</param>
        /// <returns></returns>
        public static Hediff_Addiction FindAddictionHediff(Pawn pawn, Thing drug) {
            if(!drug.def.IsDrug) {
                return null;
            }
            CompDrug compDrug = drug.TryGetComp<CompDrug>();
            if(!compDrug.Props.Addictive) {
                return null;
            }
            return AddictionUtility.FindAddictionHediff(pawn, compDrug.Props.chemical);
        }
        /// <summary>
        ///		<para>Returns specific <see cref="Hediff_Addiction"/> based on given <see cref="ChemicalDef"/></para>
        ///		<para>Probably shouldn't call this directly, it uses a lot of casts, with no try/catch, based on expecting a <see cref="ChemicalDef"/></para>
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="chemical"></param>
        /// <returns>Returns the <see cref="Hediff_Addiction"/> the <see cref="Pawn"/> has</returns>
        public static Hediff_Addiction FindAddictionHediff(Pawn pawn, ChemicalDef chemical) {
            return (Hediff_Addiction) pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.addictionHediff);
        }
        /// <summary>
        ///		<para>Checks a <see cref="Pawn"/> for a tolerance to a given <see cref="ChemicalDef"/></para>
        ///		<para>If the <see cref="ChemicalDef"/> has no <see cref="ChemicalDef.toleranceHediff"/>, or if the <see cref="Pawn"/> has no tolerance <see cref="Hediff"/>, returns null</para>
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="chemical"><see cref="ChemicalDef"/> to check for a tolerance to</param>
        /// <returns></returns>
        public static Hediff FindToleranceHediff(Pawn pawn, ChemicalDef chemical) {
            if(chemical.toleranceHediff == null) {
                return null;
            }
            return pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.toleranceHediff);
        }
        /// <summary>
        ///		<para>Changes <see cref="ChemicalDef"/> effects based on <see cref="Pawn.BodySize"/> </para>
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="chemicalDef">The <see cref="ChemicalDef"/> to modify the <see cref="Hediff"/>s of</param>
        /// <param name="effect">Reference to the effect value to alter</param>
        public static void ModifyChemicalEffectForToleranceAndBodySize(Pawn pawn, ChemicalDef chemicalDef, ref float effect) {
            if(chemicalDef != null) {
                List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
                for(int i = 0; i < hediffs.Count; i++) {
                    hediffs[i].ModifyChemicalEffect(chemicalDef, ref effect);
                }
            }
            effect /= pawn.BodySize;
        }
        /// <summary>
        ///		<para></para>//TODO: CheckDrugAddictionTeachOpportunity(), LessonAutoActivator()
        /// </summary>
        /// <param name="pawn"></param>
        public static void CheckDrugAddictionTeachOpportunity(Pawn pawn) {
            if(!pawn.RaceProps.IsFlesh || !pawn.Spawned) {
                return;
            }
            if(pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer) {
                return;
            }
            if(!AddictionUtility.AddictedToAnything(pawn)) {
                return;
            }
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugAddiction, pawn, OpportunityType.Important);
        }
        /// <summary>
        ///		<para>Checks if given <see cref="Pawn"/> is addicted to anything.</para>
        ///		<para>Specifically, if the <see cref="Pawn"/> has a <see cref="Hediff"/> of type <see cref="Hediff_Addiction"/></para>
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        public static bool AddictedToAnything(Pawn pawn) {
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for(int i = 0; i < hediffs.Count; i++) {
                if(hediffs[i] is Hediff_Addiction) {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        ///		<para>Checks if given <see cref="Pawn"/> can binge on the given <see cref="ChemicalDef"/></para>
        ///     <para></para>
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="chemical"><see cref="ChemicalDef"/> to check if the <see cref="Pawn"/> can binge on</param>
        /// <param name="drugCategory"><see cref="DrugCategory"/> of the <see cref="ChemicalDef"/> presented</param>
        /// <returns></returns>
        public static bool CanBingeOnNow(Pawn pawn, ChemicalDef chemical, DrugCategory drugCategory) {
            if(!chemical.canBinge) {
                return false;
            }
            if(!pawn.Spawned) {
                return false;
            }
            List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Drug);
            for(int i = 0; i < list.Count; i++) {
                if(!list[i].Position.Fogged(list[i].Map)) {
                    if(drugCategory == DrugCategory.Any || list[i].def.ingestible.drugCategory == drugCategory) {
                        CompDrug compDrug = list[i].TryGetComp<CompDrug>();
                        if(compDrug.Props.chemical == chemical) {
                            if(list[i].Position.Roofed(list[i].Map) || list[i].Position.InHorDistOf(pawn.Position, 45f)) {
                                if(pawn.CanReach(list[i], PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn)) {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}