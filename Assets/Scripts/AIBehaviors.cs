using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FuzzyBehavior;

namespace AIBehaviors
{
    public class Seek : IBehavior
    {
        Transform target;

        public BehaviorResult DoBehavior()
        {
            return BehaviorResult.Success;
        }
    }
}