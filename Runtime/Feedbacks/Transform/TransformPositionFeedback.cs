﻿using System;
using UnityEngine;
using Juce.Tween;

namespace Juce.Feedbacks
{
    [FeedbackIdentifier("Position", "Transform/")]
    public class TransformPositionFeedback : Feedback
    {
        [Header(FeedbackSectionsUtils.TargetSection)]
        [SerializeField] private Transform target = default;

        [Header(FeedbackSectionsUtils.ValuesSection)]
        [SerializeField] private CoordinatesSpace coordinatesSpace = default;
        [SerializeField] private StartEndVector3Property value = default;

        [Header(FeedbackSectionsUtils.TimingSection)]
        [SerializeField] [Min(0)] private float delay = default;
        [SerializeField] [Min(0)] private float duration = default;

        [Header(FeedbackSectionsUtils.EasingSection)]
        [SerializeField] private EasingProperty easing = default;

        [Header(FeedbackSectionsUtils.LoopSection)]
        [SerializeField] private LoopProperty loop = default;

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
            string info = $"{duration}s";

            if (value.UseStartValue)
            {
                info += $" | Start: x:{value.StartValueX} y:{value.StartValueY} z: {value.StartValueZ}";
            }

            info += $" | End: x:{value.EndValueX} y:{value.EndValueY} z: {value.EndValueZ}";

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

        public override ExecuteResult OnExecute(FlowContext context, SequenceTween sequenceTween)
        {
            Tween.Tween delayTween = null;

            if (delay > 0)
            {
                delayTween = new WaitTimeTween(delay);
                sequenceTween.Append(delayTween);
            }

            if (value.UseStartValue)
            {
                SequenceTween startSequence = new SequenceTween();

                switch (coordinatesSpace)
                {
                    case CoordinatesSpace.Local:
                        {
                            if (value.UseStartX)
                            {
                                startSequence.Join(target.TweenLocalPositionX(value.StartValueX, 0.0f));
                            }

                            if (value.UseStartY)
                            {
                                startSequence.Join(target.TweenLocalPositionY(value.StartValueY, 0.0f));
                            }

                            if (value.UseStartZ)
                            {
                                startSequence.Join(target.TweenLocalPositionZ(value.StartValueZ, 0.0f));
                            }
                        }
                        break;

                    case CoordinatesSpace.World:
                        {
                            if (value.UseStartX)
                            {
                                startSequence.Join(target.TweenPositionX(value.StartValueX, 0.0f));
                            }

                            if (value.UseStartY)
                            {
                                startSequence.Join(target.TweenPositionY(value.StartValueY, 0.0f));
                            }

                            if (value.UseStartZ)
                            {
                                startSequence.Join(target.TweenPositionZ(value.StartValueZ, 0.0f));
                            }
                        }
                        break;
                }

                sequenceTween.Append(startSequence);
            }

            SequenceTween endSequence = new SequenceTween();

            switch (coordinatesSpace)
            {
                case CoordinatesSpace.Local:
                    {
                        if (value.UseEndX)
                        {
                            endSequence.Join(target.TweenLocalPositionX(value.EndValueX, duration));
                        }

                        if (value.UseEndY)
                        {
                            endSequence.Join(target.TweenLocalPositionY(value.EndValueY, duration));
                        }

                        if (value.UseEndZ)
                        {
                            endSequence.Join(target.TweenLocalPositionZ(value.EndValueZ, duration));
                        }
                    }
                    break;

                case CoordinatesSpace.World:
                    {
                        if (value.UseEndX)
                        {
                            endSequence.Join(target.TweenPositionX(value.EndValueX, duration));
                        }

                        if (value.UseEndY)
                        {
                            endSequence.Join(target.TweenPositionY(value.EndValueY, duration));
                        }

                        if (value.UseEndZ)
                        {
                            endSequence.Join(target.TweenPositionZ(value.EndValueZ, duration));
                        }
                    }
                    break;
            }

            Tween.Tween progressTween = endSequence;

            sequenceTween.Append(endSequence);

            EasingUtils.SetEasing(sequenceTween, easing);
            LoopUtils.SetLoop(sequenceTween, loop);

            ExecuteResult result = new ExecuteResult();
            result.DelayTween = delayTween;
            result.ProgresTween = progressTween;

            return result;
        }
    }
}
