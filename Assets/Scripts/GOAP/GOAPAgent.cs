using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, int> subGoals;
    public bool remove;

    public SubGoal(string key, int value, bool remove)
    {
        subGoals = new Dictionary<string, int>();
        subGoals.Add(key, value);
        this.remove = remove;
    }
}

public class GOAPAgent : MonoBehaviour
{
    public List<GOAPAction> actions = new List<GOAPAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    GOAPPlanner planner;
    public Queue<GOAPAction> actionQueue;
    public GOAPAction currentAction;
    public SubGoal currentGoal;

    // Start is called before the first frame update
    void Start()
    {
        GOAPAction[] allActions = this.GetComponents<GOAPAction>();//Get all actions this agent can perform
        foreach(GOAPAction action in allActions)
        {
            actions.Add(action);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        
    }
}
