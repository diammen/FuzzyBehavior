using System.Collections.Generic;
using UnityEngine;

namespace FuzzyBehavior
{

    public enum BehaviorResult
    {
        Success,
        Failure
    }

    public interface IBehavior
    {
        BehaviorResult DoBehavior();
    }

    // iterate through children's behaviors until there's a success, then return success otherwise return failure
    public class Selector : IBehavior
    {
        public IBehavior[] children;

        public Selector(IBehavior[] _children)
        {
            children = _children;
        }
        public BehaviorResult DoBehavior()
        {
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].DoBehavior() == BehaviorResult.Success)
                {
                    return BehaviorResult.Success;
                }
            }
            return BehaviorResult.Failure;
        }
    }

    // iterate through children's behaviors until there's a failure, then return failure otherwise return success
    public class Sequence : IBehavior
    {
        public IBehavior[] children;

        public Sequence(IBehavior[] _children)
        {
            children = _children;
        }
        public BehaviorResult DoBehavior()
        {
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].DoBehavior() == BehaviorResult.Failure)
                {
                    return BehaviorResult.Failure;
                }
            }
            return BehaviorResult.Success;
        }
    }

    // invert the result of child node, success becoming failure and vice versa
    public class Inverter : IBehavior
    {
        public IBehavior child;

        public Inverter(IBehavior _child)
        {
            child = _child;
        }
        public BehaviorResult DoBehavior()
        {
            if (child.DoBehavior() == BehaviorResult.Success)
            {
                return BehaviorResult.Failure;
            }
            return BehaviorResult.Success;
        }
    }

    // return success regardless of the result of the child node
    public class Succeeder : IBehavior
    {
        public IBehavior child;

        public Succeeder(IBehavior _child)
        {
            child = _child;
        }
        public BehaviorResult DoBehavior()
        {
            child.DoBehavior();
            return BehaviorResult.Success;
        }
    }

    // halt execution after getting result from child node for a set amount of time
    public class Cooldown : IBehavior
    {
        public IBehavior child;
        float cooldown;
        float cooldownDuration;

        public Cooldown(IBehavior _child, float _cooldownDuration)
        {
            child = _child;
            cooldown = float.MinValue;
            cooldownDuration = _cooldownDuration;
        }
        public BehaviorResult DoBehavior()
        {
            // exit early if still on cooldown
            if (Time.time < cooldown + cooldownDuration)
            {
                return BehaviorResult.Failure;
            }

            // set new timestamp
            cooldown = Time.time;
            return child.DoBehavior();
        }
    }

    // give child node a time limit to finish before stopping and failing
    public class TimeLimit : IBehavior
    {
        public IBehavior child;
        float timeLimit;
        float duration;

        public TimeLimit(IBehavior _child, float _duration)
        {
            child = _child;
            timeLimit = Time.time;
            duration = _duration;
        }
        public BehaviorResult DoBehavior()
        {
            // continue processing child node while still within time limit
            if (Time.time < timeLimit + duration)
            {
                child.DoBehavior();
                return BehaviorResult.Success;
            }

            // set new timestamp and return failure
            timeLimit = Time.time;
            return BehaviorResult.Failure;
        }
    }

    // repeat the child node's process a set number of times
    public class Loop : IBehavior
    {
        public IBehavior child;

        public Loop(IBehavior _child)
        {
            child = _child;
        }
        public BehaviorResult DoBehavior()
        {
            return BehaviorResult.Success;
        }
    }

    // repeat the child node's process until it fails
    public class LoopUntilFail : IBehavior
    {
        public IBehavior child;

        public LoopUntilFail(IBehavior _child)
        {
            child = _child;
        }
        public BehaviorResult DoBehavior()
        {

            return BehaviorResult.Success;
        }
    }

    public class Action : IBehavior
    {
        public Action()
        {

        }
        public BehaviorResult DoBehavior()
        {
            return BehaviorResult.Success;
        }
    }

    // note implement fuzzy inference

    public static class Fuzzy
    {
        public static float[] EvaluateInput(AnimationCurve[] sets, float input)
        {
            float[] membershipValues = new float[sets.Length];
            for (int i = 0; i < sets.Length; i++)
            {
                membershipValues[i] = sets[i].Evaluate(input);
            }

            return membershipValues;
        }

        public static float EvaluateInput(AnimationCurve set, float input)
        {
            return set.Evaluate(input);
        }

        public static float[] MaxMembershipPositions(AnimationCurve[] sets)
        {
            float[] maxMembershipValues = new float[sets.Length];
            for (int i = 0; i < sets.Length; i++)
            {
                maxMembershipValues[i] = Maxima(sets[i]);
            }

            return maxMembershipValues;
        }

        // Average of Maxima method
        public static float Defuzzify(float[] membershipAggregate, float[] maxMembership)
        {
            float[] valueProducts = new float[membershipAggregate.Length];
            for (int i = 0; i < valueProducts.Length; i++)
            {
                valueProducts[i] = membershipAggregate[i] * maxMembership[i];
            }

            return Sum(valueProducts) / Sum(membershipAggregate);
        }

        public static float Sum(float[] values)
        {
            float sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            return sum;
        }

        public static float Maxima(AnimationCurve curve)
        {
            float max = 1;
            for (int i = 0; i < curve.keys.Length - 1; i++)
            {
                if (curve.keys[i].value >= max)
                {
                    if (curve.keys[i].value == curve.keys[i + 1].value)
                        return Sum(new float[] { curve.keys[i].time, curve.keys[i + 1].time }) / 2;
                    else
                        return curve.keys[i].time;
                }
            }
            return 0;
        }

        public static float[] Maxima(AnimationCurve[] curves)
        {
            float[] results = new float[curves.Length];
            for (int i = 0; i < curves.Length; i++)
            {
                results[0] = Maxima(curves[i]);
            }

            return results;
        }

        public static float LeftShMaxima(AnimationCurve curve)
        {
            // end of the plateau divided 2
            return curve.keys[1].time / 2;
        }

        public static float RightShMaxima(AnimationCurve curve)
        {
            // end of the plateau divided by 2
            return curve.keys[2].time / 2;
        }

        public static float TrapezoidMaxima(AnimationCurve curve)
        {
            return (curve.keys[3].time + curve.keys[4].time) / 2;
        }

        public static float Min(AnimationCurve curve)
        {
            Keyframe min = new Keyframe();
            for (int i = 0; i < curve.keys.Length; i++)
            {
                if (curve.keys[i].value < min.value)
                {
                    min = curve.keys[i];
                }
            }
            return min.time;
        }

        public static float AND(float lhs, float rhs)
        {
            return lhs < rhs ? lhs : rhs;
        }

        public static float AND(AnimationCurve lhs, AnimationCurve rhs, float lhsInput, float rhsInput)
        {
            float mLhs = lhs.Evaluate(lhsInput);
            float mRhs = rhs.Evaluate(rhsInput);

            return mLhs < mRhs ? mLhs : mRhs;
        }

        public static float OR(float lhs, float rhs)
        {
            return lhs > rhs ? lhs : rhs;
        }

        public static float OR(AnimationCurve lhs, AnimationCurve rhs, float lhsInput, float rhsInput)
        {
            float mLhs = lhs.Evaluate(lhsInput);
            float mRhs = rhs.Evaluate(rhsInput);

            return mLhs > mRhs ? mLhs : mRhs;
        }

        public static float NOT(float f)
        {
            return 1 - f;
        }
    }

    [System.Serializable]
    public class MembershipFunction
    {
        public AnimationCurve[] sets;

        public MembershipFunction(AnimationCurve[] _sets)
        {
            sets = _sets;
        }
    }

    public static class FuzzyRule
    {
        
    }
}