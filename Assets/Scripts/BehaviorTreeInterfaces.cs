using System.Collections;
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
}