using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FuzzyBehavior;

namespace AIBehaviors
{
    public class TargetInRange : IBehavior
    {
        TestAI agent;

        TargetInRange(TestAI _agent)
        {
            agent = _agent;
        }
        public BehaviorResult DoBehavior()
        {
            
            return BehaviorResult.Success;
        }
    }
    public class SeekTarget : IBehavior
    {
        TestAI agent;

        public BehaviorResult DoBehavior()
        {
            return BehaviorResult.Success;
        }
    }
}