using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld {
	/// <summary>
	///		Abstract/super class for Alert/notification-related functions.
	/// </summary>
	[StaticConstructorOnStartup]
	public abstract class Alert {
		protected AlertPriority defaultPriority;
		protected string defaultLabel;
		protected string defaultExplanation;
		protected float lastBellTime = -1000f;
		private int jumpToTargetCycleIndex;
		private AlertBounce alertBounce;
		public const float Width = 154f;
		private const float TextWidth = 148f;
		public const float Height = 28f;
		private const float ItemPeekWidth = 30f;
		public const float InfoRectWidth = 330f;
		private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);
		private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;
		private static List<GlobalTargetInfo> tmpTargets = new List<GlobalTargetInfo>();

		/// <summary>
		///		<para>Returns the default priority of the Alert</para>
		///		<para>Intended to be overwritten by subclasses</para>
		/// </summary>
		public virtual AlertPriority Priority {
			get {
				return this.defaultPriority;
			}
		}
		/// <summary>
		///		<para>Returns the default Color of the Alert</para>
		///		<para>Intended to be overwritten by subclasses</para>
		/// </summary>
		protected virtual Color BGColor {
			get {
				return Color.clear;
			}
		}
		/// <summary>
		///		<para>Returns if the Alert is active</para>
		///		<para>Intended to be overwritten by subclasses</para>
		/// </summary>
		public virtual bool Active {
			get {
				return this.GetReport().active;
			}
		}
		/// <summary>
		///		<para>Returns an <see cref="AlertReport"/> on the Alert</para>
		///		<para>Must be overwritten by subclasses</para>
		/// </summary>
		public abstract AlertReport GetReport();
		/// <summary>
		///		<para>Returns an explanation of the Alert</para>
		///		<para>Intended to be overwritten by subclasses</para>
		/// </summary>
		/// <returns>A string of the Alert explanation</returns>
		public virtual string GetExplanation() {
			return this.defaultExplanation;
		}
		/// <summary>
		///		<para>Returns the default label of the Alert</para>
		///		<para>Intended to be overwritten by subclasses</para>
		/// </summary>
		/// <returns></returns>
		public virtual string GetLabel() {
			return this.defaultLabel;
		}
		/// <summary>
		///		<para>Creates initial Notification of the Alert starting, plus the sounds</para>
		/// </summary>
		public void Notify_Started() {
			if(this.Priority>=AlertPriority.High) {
				if(this.alertBounce==null) {
					this.alertBounce=new AlertBounce();
				}
				this.alertBounce.DoAlertStartEffect();
				if(Time.timeSinceLevelLoad>1f&&Time.realtimeSinceStartup>this.lastBellTime+0.5f) {
					SoundDefOf.TinyBell.PlayOneShotOnCamera(null);
					this.lastBellTime=Time.realtimeSinceStartup;
				}
			}
		}
		/// <summary>
		///		<para>Does nothing by default</para>
		///		<para>Intended to be overwritten by subclasses</para>
		/// </summary>
		public virtual void AlertActiveUpdate() {
		}
		/// <summary>
		///		<para>Draws the Alert box rectangle</para>
		///		<para>Can be overwritten by subclasses, but provides all the needed code to display the Alert</para>
		/// </summary>
		/// <param name="topY">The top Y-value to draw from</param>
		/// <param name="minimized"></param>
		/// <returns>A Rect object to draw</returns>
		public virtual Rect DrawAt(float topY, bool minimized) {
			Text.Font=GameFont.Small;
			string label = this.GetLabel();
			float height = Text.CalcHeight(label, 148f);
			Rect rect = new Rect((float)UI.screenWidth-154f, topY, 154f, height);
			if(this.alertBounce!=null) {
				rect.x-=this.alertBounce.CalculateHorizontalOffset();
			}
			GUI.color=this.BGColor;
			GUI.DrawTexture(rect, Alert.AlertBGTex);
			GUI.color=Color.white;
			GUI.BeginGroup(rect);
			Text.Anchor=TextAnchor.MiddleRight;
			Widgets.Label(new Rect(0f, 0f, 148f, height), label);
			GUI.EndGroup();
			if(Mouse.IsOver(rect)) {
				GUI.DrawTexture(rect, Alert.AlertBGTexHighlight);
			}
			if(Widgets.ButtonInvisible(rect, false)) {
				IEnumerable<GlobalTargetInfo> culprits = this.GetReport().culprits;
				if(culprits!=null) {
					Alert.tmpTargets.Clear();
					foreach(GlobalTargetInfo current in culprits) {
						if(current.IsValid) {
							Alert.tmpTargets.Add(current);
						}
					}
					if(Alert.tmpTargets.Any<GlobalTargetInfo>()) {
						if(Event.current.button==1) {
							this.jumpToTargetCycleIndex--;
						}
						else {
							this.jumpToTargetCycleIndex++;
						}
						GlobalTargetInfo target = Alert.tmpTargets[GenMath.PositiveMod(this.jumpToTargetCycleIndex, Alert.tmpTargets.Count)];
						CameraJumper.TryJumpAndSelect(target);
						Alert.tmpTargets.Clear();
					}
				}
			}
			Text.Anchor=TextAnchor.UpperLeft;
			return rect;
		}
		/// <summary>
		///		<para></para>
		/// </summary>
		//* TODO DrawInfoPane()
		public void DrawInfoPane() {
			Alert.<DrawInfoPane>c__AnonStorey0<DrawInfoPane>c__AnonStorey=new Alert.<DrawInfoPane>c__AnonStorey0();
			if(Event.current.type!=EventType.Repaint) {
				return;
			}
			Text.Font=GameFont.Small;
			Text.Anchor=TextAnchor.UpperLeft;
			<DrawInfoPane>c__AnonStorey.expString=this.GetExplanation();
			if(this.GetReport().AnyCulpritValid) {
				<DrawInfoPane>c__AnonStorey.expString= <DrawInfoPane>c__AnonStorey.expString+"\n\n("+"ClickToJumpToProblem".Translate()+")";
			}
			float num = Text.CalcHeight(<DrawInfoPane>c__AnonStorey.expString, 310f);
			num+=20f;
			<DrawInfoPane>c__AnonStorey.infoRect=new Rect((float)UI.screenWidth-154f-330f-8f, Mathf.Max(Mathf.Min(Event.current.mousePosition.y, (float)UI.screenHeight-num), 0f), 330f, num);
			if(<DrawInfoPane>c__AnonStorey.infoRect.yMax>(float)UI.screenHeight) {
				Alert.<DrawInfoPane>c__AnonStorey0 expr_EE_cp_0= <DrawInfoPane>c__AnonStorey;
				expr_EE_cp_0.infoRect.y=expr_EE_cp_0.infoRect.y-((float)UI.screenHeight- <DrawInfoPane>c__AnonStorey.infoRect.yMax);
			}
			if(<DrawInfoPane>c__AnonStorey.infoRect.y<0f) {
				<DrawInfoPane>c__AnonStorey.infoRect.y=0f;
			}
			Find.WindowStack.ImmediateWindow(138956, <DrawInfoPane>c__AnonStorey.infoRect, WindowLayer.GameUI, delegate {
				Text.Font=GameFont.Small;
				Rect rect = <DrawInfoPane>c__AnonStorey.infoRect.AtZero();
				Widgets.DrawWindowBackground(rect);
				Rect position = rect.ContractedBy(10f);
				GUI.BeginGroup(position);
				Widgets.Label(new Rect(0f, 0f, position.width, position.height), <DrawInfoPane>c__AnonStorey.expString);
				GUI.EndGroup();
			}, false, false, 1f);
		}
	}
}
