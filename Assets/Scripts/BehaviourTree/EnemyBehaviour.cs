using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    public Vector3 target;
    public Vector3 destination = Vector3.zero;

    ActionNode pickRandomDestination;
    ActionNode checkIfDestinationReached;
    ActionNode moveToDestination;
    Sequence decideDestination;

    Selector rootNode;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pickRandomDestination = new ActionNode(PickRandomDestination);
        checkIfDestinationReached = new ActionNode(CheckIfDestinationReached);
        decideDestination = new Sequence(new List<BTNode>()
        {
            checkIfDestinationReached,
            pickRandomDestination

        });

        moveToDestination = new ActionNode(MoveToDestination);

        rootNode = new Selector(new List<BTNode>()
        {
            decideDestination,
            moveToDestination
        });

    }

    // Update is called once per frame
    void Update()
    {
        rootNode.Evaluate();
    }

    BTNodeStates PickRandomDestination()
    {
        Vector3 dest = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        this.destination = dest;
        return BTNodeStates.SUCCESS;
    }

    BTNodeStates CheckIfDestinationReached()
    {
        if (Vector3.Distance(transform.position, destination) <= agent.stoppingDistance)
        {
            return BTNodeStates.SUCCESS;
        }
        else
        {
            return BTNodeStates.FAILURE;
        }
    }

    BTNodeStates MoveToDestination()
    {
        agent.SetDestination(this.destination);
        return BTNodeStates.SUCCESS;
    }


}
