﻿using System;
using UnityEngine;
using UnityEngine.UI;
using Juce.Tween;

namespace Juce.Feedbacks
{
    [FeedbackIdentifier("Color Alpha", "Graphic Material/")]
    public class GraphicMaterialColorAlphaFeedback : Feedback
    {
        [SerializeField] [HideInInspector] private GraphicMaterialPropertyElement target = default;
        [SerializeField] [HideInInspector] private FloatElement value = default;
        [SerializeField] [HideInInspector] private TimingElement timing = default;
        [SerializeField] [HideInInspector] private LoopElement loop = default;
        [SerializeField] [HideInInspector] private EasingElement easing = default;

        public override bool GetFeedbackErrors(out string errors)
        {
            if (target.Graphic != null)
            {
                errors = "";
                return false;
            }

            errors = "Target is null";

            return true;
        }

        public override string GetFeedbackTargetInfo()
        {
            return target.Graphic != null ? target.Graphic.gameObject.name : string.Empty;
        }

        public override string GetFeedbackInfo()
        {
            string info = $"{timing.Duration}s";

            if (value.UseStartValue)
            {
                info += $" | Start: {value.StartValue} ";
            }

            info += $" | End: {value.EndValue} ";

            if (!easing.UseAnimationCurve)
            {
                info += $" | Ease: {easing.Easing}";
            }
            else
            {
                info += $" | Ease: Curve";
            }

            return info;
        }

        protected override void OnCreate()
        {
            target = AddElement<GraphicMaterialPropertyElement>("Target");
            target.MaterialPropertyType = MaterialPropertyType.Color;

            value = AddElement<FloatElement>("Values");
            value.MinValue = 0.0f;
            value.MaxValue = 1.0f;

            timing = AddElement<TimingElement>("Timing");
            loop = AddElement<LoopElement>("Loop");

            easing = AddElement<EasingElement>("Easing");
        }

        public override void OnExectue(FlowContext context, SequenceTween sequenceTween)
        {
            if (target.Graphic == null)
            {
                return;
            }

            GraphicMaterialUtils.TryInstantiateGraphicMaterial(target);

            Material material = target.Graphic.materialForRendering;

            bool hasProperty = material.HasProperty(target.Property);

            if (!hasProperty)
            {
                Debug.Log("");
                return;
            }

            sequenceTween.AppendWaitTime(context.CurrentDelay + timing.Delay);

            if (value.UseStartValue)
            {
                sequenceTween.Append(material.TweenColorAlpha(value.StartValue, target.Property, 0.0f));
            }

            sequenceTween.Append(material.TweenColorAlpha(value.EndValue, target.Property, timing.Duration));

            easing.SetEasing(sequenceTween);

            loop.SetLoop(sequenceTween);
        }
    }
}