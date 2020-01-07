using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FuzzyBehavior;

namespace AIBehaviors
{
    public class FindTarget : IBehavior
    {
        TestAI agent;

        public FindTarget(TestAI _agent)
        {
            agent = _agent;
        }
        public BehaviorResult DoBehavior()
        {
            if (agent.TargetDetection.isColliding)
            {
                agent.target = agent.TargetDetection.collidedObject.transform;
                return BehaviorResult.Success;
            }
            agent.target = null;
            return BehaviorResult.Failure;
        }
    }
    public class SeekTarget : IBehavior
    {
        TestAI agent;

        public SeekTarget(TestAI _agent)
        {
            agent = _agent;
        }
        public BehaviorResult DoBehavior()
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, agent.target.position, Time.deltaTime * agent.moveSpeed * agent.speedMod);
            return BehaviorResult.Success;
        }
    }
    public class Idle : IBehavior
    {
        TestAI agent;

        public Idle(TestAI _agent)
        {
            agent = _agent;
        }
        public BehaviorResult DoBehavior()
        {
            agent.moveSpeed = 0;
            return BehaviorResult.Success;
        }
    }
    public class CalculateSpeed : IBehavior
    {
        TestAI agent;

        public CalculateSpeed(TestAI _agent)
        {
            agent = _agent;
        }
        public BehaviorResult DoBehavior()
        {
            agent.distanceInput = (agent.target.position - agent.transform.position).magnitude;
            agent.intimidationInput = agent.targetSize;

            agent.moveSpeed = Fuzzy.Defuzzify(new float[] { Fuzzy.OR(agent.distance.sets[0], agent.intimidation.sets[2], agent.distanceInput, agent.intimidationInput),
                                        Fuzzy.OR(agent.distance.sets[1], agent.intimidation.sets[1], agent.distanceInput, agent.intimidationInput),
                                        Fuzzy.OR(agent.distance.sets[2], agent.intimidation.sets[0], agent.distanceInput, agent.intimidationInput) },
                            Fuzzy.Maxima(agent.speed.sets));
            return BehaviorResult.Success;
        }
    }
}