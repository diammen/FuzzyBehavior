using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FuzzyBehavior;

namespace AIBehaviors
{
    public class TargetInRange : IBehavior
    {
        Transform target;

        public BehaviorResult DoBehavior()
        {
            return BehaviorResult.Success;
        }
    }
    public class SeekTarget : IBehavior
    {
        Transform target;

        public BehaviorResult DoBehavior()
        {
            return BehaviorResult.Success;
        }
    }
}