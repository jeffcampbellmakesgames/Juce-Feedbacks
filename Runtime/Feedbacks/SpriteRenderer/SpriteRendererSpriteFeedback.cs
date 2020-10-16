﻿using System;
using UnityEngine;
using Juce.Tween;

namespace Juce.Feedbacks
{
    [FeedbackIdentifier("Sprite", "SpriteRenderer/")]
    public class SpriteRendererSpriteFeedback : Feedback
    {
        [Header("Target")]
        [SerializeField] private SpriteRenderer target = default;

        [SerializeField] [HideInInspector] private SpriteElement value = default;
        [SerializeField] [HideInInspector] private TimingElement timing = default;

        public override bool GetFeedbackErrors(out string errors)
        {
            if (target != null)
            {
                errors = "";
                return false;
            }

            errors = "Target is null";

            return true;
        }

        public override string GetFeedbackTargetInfo()
        {
            return target != null ? target.gameObject.name : string.Empty;
        }

        public override string GetFeedbackInfo()
        {
            return value.Value != null ? value.Value.name : string.Empty;
        }

        protected override void OnCreate()
        {
            AddElement<SpriteElement>(0, "Values");

            TimingElement timingElement = AddElement<TimingElement>(1, "Timing");
            timingElement.UseDuration = false;
        }

        protected override void OnLink()
        {
            value = GetElement<SpriteElement>(0);
            timing = GetElement<TimingElement>(1);
        }

        public override ExecuteResult OnExecute(FlowContext context, SequenceTween sequenceTween)
        {
            if (target == null)
            {
                return null;
            }

            Tween.Tween delayTween = null;

            if (timing.Delay > 0)
            {
                delayTween = new WaitTimeTween(timing.Delay);
                sequenceTween.Append(delayTween);
            }

            sequenceTween.AppendCallback(() =>
            {
                target.sprite = value.Value;
            });

            ExecuteResult result = new ExecuteResult();
            result.DelayTween = delayTween;

            return result;
        }
    }
}
