using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;//contains a concatenation of world and local world(agent belief) states.
    public GOAPAction action;

    public Node(Node parent, float cost, Dictionary<string, int> allStates, GOAPAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);//want this to just be a copy and not point to the dict in the args
        this.action = action;
    }

    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> belieftStates, GOAPAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);//want this to just be a copy and not point to the dict in the args

        foreach(KeyValuePair<string,int> belief in belieftStates)
        {
            if (!this.state.ContainsKey(belief.Key))
            {
                this.state.Add(belief.Key, belief.Value);
            }
        }

        this.action = action;
    }
}

public class GOAPPlanner
{
    public Queue<GOAPAction> plan(List<GOAPAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<GOAPAction> usableActions = new List<GOAPAction>();
        foreach (GOAPAction action in actions)
        {
            if (action.isAchievable())
            {
                usableActions.Add(action);
            }
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0f, GOAPWorld.Instance.GetWorldStates().GetStates(), beliefStates.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("NO PLAN FOUND");
            return null;
        }

        Node leastCostNode = null;
        foreach (Node leaf in leaves)
        {
            if (leastCostNode == null)
            {
                leastCostNode = leaf;
            }
            else
            {
                if (leaf.cost < leastCostNode.cost)
                {
                    leastCostNode = leaf;
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
        foreach (GOAPAction action in result)
        {
            actionQueue.Enqueue(action);
        }

        Debug.Log("The plan is:");
        foreach (GOAPAction action in actionQueue)
        {
            Debug.Log("Q:" + action.actionName);
            //Debug.Log(action.actionCost);
        }

        return actionQueue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GOAPAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach (GOAPAction action in usableActions)
        {
            if (action.isAchievableUnderConditions(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach (KeyValuePair<string, int> effects in action.afterEffects)
                {
                    if (!currentState.ContainsKey(effects.Key))
                    {
                        currentState.Add(effects.Key, effects.Value);
                    }
                }

                Node node = new Node(parent, parent.cost + action.actionCost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GOAPAction> subset = ActionSubset(usableActions, action);//to prevent circular path creation in graph
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }

                }
            }
        }
        return foundPath;
    }

    private List<GOAPAction> ActionSubset(List<GOAPAction> usableActions, GOAPAction removeThisAction)
    {
        List<GOAPAction> subset = new List<GOAPAction>();
        foreach (GOAPAction act in usableActions)
        {
            if (!act.Equals(removeThisAction))
            {
                subset.Add(act);
            }
        }
        return subset;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> currentState)
    {
        foreach (KeyValuePair<string, int> state in goal)
        {
            if (!currentState.ContainsKey(state.Key))
            {
                return false;
            }
        }
        return true;
    }
}
