using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Goal
{
    public Dictionary<string, int> subGoals;
    public bool remove;

    public Goal(string key, int value, bool remove)
    {
        subGoals = new Dictionary<string, int>();
        subGoals.Add(key, value);
        this.remove = remove;
    }
}

public class GOAPAgent : MonoBehaviour
{
    public List<GOAPAction> actions = new List<GOAPAction>();
    public Dictionary<Goal, int> goals = new Dictionary<Goal, int>();
    public GOAPInventory inventory = new GOAPInventory();
    public WorldStates beliefs = new WorldStates();//local world state only available to this agent

    GOAPPlanner planner;
    public Queue<GOAPAction> actionQueue;
    public GOAPAction currentAction;
    public Goal currentGoal;

    // Start is called before the first frame update
    public void Start()
    {
        GOAPAction[] allActions = this.GetComponents<GOAPAction>();//Get all actions this agent can perform
        foreach (GOAPAction action in allActions)
        {
            actions.Add(action);
        }
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
            //float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, this.transform.position);
            if (currentAction.navAgent.hasPath &&  currentAction.navAgent.remainingDistance < 2.0f)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }
        if (planner == null || actionQueue == null)
        {
            planner = new GOAPPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;//sort goals in order of priority
            
            foreach (KeyValuePair<Goal, int> sgoal in sortedGoals)
            {
                actionQueue = planner.plan(actions, sgoal.Key.subGoals, beliefs);
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
            }
            planner = null;
        }
        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if(currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindGameObjectWithTag(currentAction.targetTag);
                    
                }
                if (currentAction.target != null)
                {
                    currentAction.isActionRunning = true;
                    currentAction.navAgent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
