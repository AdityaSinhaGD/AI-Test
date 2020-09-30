﻿using System.Collections;
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

    bool invoked = false;
    void CompleteAction()
    {
        currentAction.isActionRunning = false;
        currentAction.PostPerform();
        invoked = false;
    }

    private void LateUpdate()
    {
        if (currentAction != null && currentAction.isActionRunning)
        {
            if (currentAction.navAgent.hasPath && currentAction.navAgent.remainingDistance < 1f)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }
        if(planner == null||actionQueue == null)
        {
            planner = new GOAPPlanner();
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach(KeyValuePair<SubGoal,int> sgoal in sortedGoals)
            {
                actionQueue = planner.plan(actions, sgoal.Key.subGoals, null);
                if (actionQueue != null)
                {
                    currentGoal = sgoal.Key;
                    break;
                }
            }
        }
        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
                planner = null;
            }
        }
    }
}
