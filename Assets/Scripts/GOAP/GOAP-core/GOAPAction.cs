using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GOAPAction : MonoBehaviour
{
    public string actionName = "Action";
    public float actionCost = 1.0f;

    public GameObject target; //location where action will take place
    public string targetTag; //pickup gameobject using tag, add them from scene heirarchy
    public float duration = 0f; //time taken for action to complete

    public WorldState[] preconditions;
    public WorldState[] effects;

    public NavMeshAgent navAgent;

    public Dictionary<string, int> pconditions;
    public Dictionary<string, int> afterEffects;

    public WorldStates agentBeliefs;

    public GOAPInventory inventory;

    public bool isActionRunning = false;

    public GOAPAction()
    {
        pconditions = new Dictionary<string, int>();
        afterEffects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        navAgent = this.gameObject.GetComponent<NavMeshAgent>();

        if (preconditions != null)
        {
            foreach(WorldState worldState in preconditions)
            {
                pconditions.Add(worldState.key, worldState.value);
            }
        }

        if (effects != null)
        {
            foreach(WorldState worldState in effects)
            {
                afterEffects.Add(worldState.key, worldState.value);
            }
        }
        inventory = GetComponent<GOAPAgent>().inventory;
    }

    public bool isAchievable()
    {
        return true;
    }

    public bool isAchievableUnderConditions(Dictionary<string, int> conditions)
    {
        foreach(KeyValuePair<string, int> preCond in pconditions)
        {
            if (!conditions.ContainsKey(preCond.Key))
            {
                return false;
            }
        }
        return true;
    }

    public abstract bool PrePerform();//check is action specific resources are available etc
    public abstract bool PostPerform();//check things like if action is impacting the world state or only the agent or both
}
