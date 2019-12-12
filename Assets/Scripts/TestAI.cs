using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        
    }
}
