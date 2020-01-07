using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FuzzyBehavior;
using AIBehaviors;

public class TestAI : MonoBehaviour
{
    Selector rootNode;
    Sequence searchForTarget;

    Cooldown cooldown;
    TimeLimit timeLimit;

    Idle idle;
    FindTarget findTarget;
    SeekTarget seekTarget;
    CalculateSpeed calcSpeed;

    public Transform target;

    public MembershipFunction distance;
    public MembershipFunction intimidation;
    public MembershipFunction speed;

    public Text veryFarValue, farValue, nearValue;
    public Text terrifiedValue, scaredValue, notAfraidValue;

    public Text speedValue;

    public CheckTrigger TargetDetection;

    public float targetSize;
    public float moveSpeed;
    public float speedMod;
    public float distanceInput = 0;
    public float intimidationInput = 0;
    float[] distanceTruthValues;
    float[] intimidationTruthValues;

    // Start is called before the first frame update
    void Start()
    {
        idle = new Idle(this);
        findTarget = new FindTarget(this);
        seekTarget = new SeekTarget(this);
        calcSpeed = new CalculateSpeed(this);
        timeLimit = new TimeLimit(findTarget, 1f);
        searchForTarget = new Sequence(new IBehavior[] { timeLimit, calcSpeed, seekTarget });
        rootNode = new Selector(new IBehavior[] { searchForTarget, idle });
    }

    // Update is called once per frame
    void Update()
    {
        rootNode.DoBehavior();

        distanceTruthValues = Fuzzy.EvaluateInput(distance.sets, distanceInput);

        intimidationTruthValues = Fuzzy.EvaluateInput(intimidation.sets, intimidationInput);

        nearValue.text = distanceTruthValues[0].ToString() + " true";
        farValue.text = distanceTruthValues[1].ToString() + " true";
        veryFarValue.text = distanceTruthValues[2].ToString() + " true";

        notAfraidValue.text = intimidationTruthValues[0].ToString() + " true";
        scaredValue.text = intimidationTruthValues[1].ToString() + " true";
        terrifiedValue.text = intimidationTruthValues[2].ToString() + " true";

        speedValue.text = moveSpeed.ToString();

    }
}
