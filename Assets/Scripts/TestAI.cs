using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FuzzyBehavior;

public class TestAI : MonoBehaviour
{
    Selector rootNode;
    Sequence actionSequence;

    Cooldown cooldown;
    TimeLimit timeLimit;

    Action action1;
    Action action2;
    Action action3;

    public Transform target;

    public MembershipFunction distance;
    public MembershipFunction intimidation;
    public MembershipFunction speed;

    public Text veryFarValue, farValue, nearValue;
    public Text terrifiedValue, scaredValue, notAfraidValue;

    public Text speedValue;

    public float targetSize;
    float distanceInput = 0;
    float intimidationInput = 0;
    float moveSpeed;
    float[] distanceTruthValues;
    float[] intimidationTruthValues;

    // Start is called before the first frame update
    void Start()
    {
        action1 = new Action();
        action2 = new Action();
        action3 = new Action();
        timeLimit = new TimeLimit(action2, 1f);
        actionSequence = new  Sequence(new IBehavior[] { action1, timeLimit });
        rootNode = new Selector(new IBehavior[] { actionSequence, action3 });
    }

    // Update is called once per frame
    void Update()
    {
        distanceInput = (target.position - transform.position).magnitude;
        distanceTruthValues = Fuzzy.EvaluateInput(distance.sets, distanceInput);

        intimidationInput = targetSize;
        intimidationTruthValues = Fuzzy.EvaluateInput(intimidation.sets, intimidationInput);


        moveSpeed = Fuzzy.Defuzzify(new float[] { Fuzzy.EvaluateInput(speed.sets[0], Fuzzy.AND(distance.sets[0], intimidation.sets[0], distanceInput, intimidationInput)) }, new float[] { Fuzzy.Maxima(speed.sets[0]) });

        nearValue.text = distanceTruthValues[0].ToString() + " true";
        farValue.text = distanceTruthValues[1].ToString() + " true";
        veryFarValue.text = distanceTruthValues[2].ToString() + " true";

        notAfraidValue.text = intimidationTruthValues[0].ToString() + " true";
        scaredValue.text = intimidationTruthValues[1].ToString() + " true";
        terrifiedValue.text = intimidationTruthValues[2].ToString() + " true";

        speedValue.text = moveSpeed.ToString();

    }
}
