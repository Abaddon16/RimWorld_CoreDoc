using System;
using UnityEngine;
using Verse;

namespace RimWorld {
	/// <summary>
	///		<para>A classification level of <see cref="Alert "/>. This is the highest classification</para>
	///		<para>Subclass of <see cref="Alert"/></para>
	/// </summary>
	public abstract class Alert_Critical : Alert {
		private int lastActiveFrame = -1;
		private const float PulseFreq = 0.5f;
		private const float PulseAmpCritical = 0.6f;
		private const float PulseAmpTutorial = 0.2f;
		/// <summary>
		///		<para>Returns a Color for the background. Provides the pulsing background for the <see cref="Alert_Critical"/></para>
		/// </summary>
		protected override Color BGColor {
			get {
				float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
				return new Color(num, num, num)*Color.red;
			}
		}
		/// <summary>
		///		<para>Sets the <see cref="Alert.defaultPriority"/> to <see cref="AlertPriority.Critical"/></para>
		/// </summary>
		public Alert_Critical() {
			this.defaultPriority=AlertPriority.Critical;
		}
		/// <summary>
		///		<para></para>
		/// </summary>
		//TODO: Alert_Critical.AlertActiveUpdate()
		public override void AlertActiveUpdate() {
			if(this.lastActiveFrame<Time.frameCount-1) {
				Messages.Message("MessageCriticalAlert".Translate(this.GetLabel().CapitalizeFirst()), new LookTargets(this.GetReport().culprits), MessageTypeDefOf.ThreatBig, true);
			}
			this.lastActiveFrame=Time.frameCount;
		}
	}
}
