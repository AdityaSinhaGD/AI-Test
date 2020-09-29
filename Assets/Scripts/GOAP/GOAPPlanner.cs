using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GOAPAction action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, GOAPAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);//want this to just be a copy and not point to the dict in the args
        this.action = action;
    }
}

public class GOAPPlanner
{
   public Queue<GOAPAction> plan(List<GOAPAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GOAPAction> usableActions = new List<GOAPAction>();
        foreach(GOAPAction action in actions)
        {
            if (action.isAchievable())
            {
                usableActions.Add(action);
            }
        }

        List<Node> nodes = new List<Node>();
        Node start = new Node(null, 0f, GOAPWorld.Instance.GetWorldStates().GetStates(), null);

        bool success = BuildGraph(start, nodes, usableActions, goal);

        if (!success)
        {
            Debug.Log("NO PLAN FOUND");
            return null;
        }

        Node leastCostNode = null;
        foreach(Node node in nodes)
        {
            if(leastCostNode == null)
            {
                leastCostNode = node;
            }
            else
            {
                if (node.cost < leastCostNode.cost)
                {
                    leastCostNode = node;
                }
            }
        }

        List<GOAPAction> result = new List<GOAPAction>();
        Node discoveredLeastCodeNode = leastCostNode;

        while (discoveredLeastCodeNode != null)
        {
            if (discoveredLeastCodeNode.action != null)
            {
                result.Insert(0, discoveredLeastCodeNode.action);
            }
            discoveredLeastCodeNode = discoveredLeastCodeNode.parent;
        }

        Queue<GOAPAction> actionQueue = new Queue<GOAPAction>();
        foreach(GOAPAction action in result)
        {
            actionQueue.Enqueue(action);
        }

        Debug.Log("The plan is:");
        foreach(GOAPAction action in actionQueue)
        {
            Debug.Log(action.actionCost);
        }

        return actionQueue;
    }

    private bool BuildGraph(Node parent, List<Node> nodes, List<GOAPAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GOAPAction action in usableActions)
        {
            if (action.isAchievableUnderConditions(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string,int> effects in action.afterEffects)
                {
                    if (!currentState.ContainsKey(effects.Key))
                    {
                        currentState.Add(effects.Key, effects.Value);
                    }
                }

                Node node = new Node(parent, parent.cost + action.actionCost, currentState, action);

                if(GoalAchieved(goal, currentState))
                {
                    nodes.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GOAPAction> subset = ActionSubset(usableActions, action);//to prevent circular path creation in graph
                    bool found = BuildGraph(node, nodes, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }

                }
            }
        }
        return foundPath;
    }

    private List<GOAPAction> ActionSubset(List<GOAPAction> usableActions, GOAPAction action)
    {
        List<GOAPAction> subset = new List<GOAPAction>();
        foreach(GOAPAction act in usableActions)
        {
            if (!act.Equals(action))
            {
                subset.Add(act);
            }
        }
        return subset;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> currentState)
    {
        foreach(KeyValuePair<string,int> state in goal)
        {
            if (!currentState.ContainsKey(state.Key))
            {
                return false;
            }
        }
        return true;
    }
}
