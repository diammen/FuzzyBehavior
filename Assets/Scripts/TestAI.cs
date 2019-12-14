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

    public AnimationCurve tooFar, far, near;

    public Text inputText;
    public Text tooFarValue, farValue, nearValue;

    float input = 0;
    float[] truthValues;

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
        input = (target.position - transform.position).magnitude;

        truthValues = Fuzzy.EvaluateInput(new AnimationCurve[] { tooFar, far, near }, input);

        tooFarValue.text = truthValues[0].ToString() + " true";
        farValue.text = truthValues[1].ToString() + " true";
        nearValue.text = truthValues[2].ToString() + " true";
    }
}
