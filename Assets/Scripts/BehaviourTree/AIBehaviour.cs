using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBehaviour : MonoBehaviour
{
    public Vector3 targetPosition;
    public GameObject targetObject = null;

    public float moveSpeed = 10f;
    public float rotationSpeed = 25f;

    public string faction = "empire";
    public string enemyFaction = "rebels";

    public GameObject[] targets = null;

    //Behaviour
    Selector root;
    Sequence checkReachSequence;
    Sequence moveSequence;
    Sequence SelectAdversary;
    Selector SelectTargetType;

    // Start is called before the first frame update
    void Start()
    {
        PopulateTargets();

        targetPosition = new Vector3(Random.Range(-400, 400), Random.Range(-400, 400), Random.Range(-400, 400));

        moveSequence = new Sequence(new List<BTNode>
        {
            new ActionNode(RotateTowardsTarget),
            new ActionNode(MoveTowardsTarget)
        });

        SelectAdversary = new Sequence(new List<BTNode>
        {
            new ActionNode(RandomChanceToSelectTargetObject),
            new ActionNode(FindNewTarget)
        });

        SelectTargetType = new Selector(new List<BTNode>
        {
            SelectAdversary,
            new ActionNode(SelectRandomWayPoint)
        });

        checkReachSequence = new Sequence(new List<BTNode>
        {
            new ActionNode(CheckIfTargetPositionReached),
            SelectTargetType
        });

        root = new Selector(new List<BTNode>
        {
            checkReachSequence,
            moveSequence
        });

    }

    private void PopulateTargets()
    {
        AIBehaviour[] aIs = FindObjectsOfType<AIBehaviour>();
        List<GameObject> enemyObjects = new List<GameObject>();
        foreach(AIBehaviour aI in aIs)
        {
            if(aI.faction == this.enemyFaction)
            {
                enemyObjects.Add(aI.gameObject);
            }
           
        }
        targets = enemyObjects.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }

    private BTNodeStates SelectRandomWayPoint()
    {
        this.targetObject = null;
        Vector3 position = new Vector3(Random.Range(-400, 400), Random.Range(-400, 400), Random.Range(-400, 400));
        targetPosition = position;
        return BTNodeStates.SUCCESS;
    }

    private BTNodeStates CheckIfTargetPositionReached()
    {
        if (Vector3.Distance(this.transform.position, GetTargetPosition()) <= 50f)
        {
            return BTNodeStates.SUCCESS;
        }
        else
        {
            return BTNodeStates.FAILURE;
        }
    }

    private BTNodeStates MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, GetTargetPosition(), moveSpeed * Time.deltaTime);
        return BTNodeStates.SUCCESS;
    }

    private BTNodeStates RotateTowardsTarget()
    {
        Vector3 direction = (GetTargetPosition() - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        return BTNodeStates.SUCCESS;
    }

    private BTNodeStates RandomChanceToSelectTargetObject()
    {
        int chance = Random.Range(0, 5);
        return (chance > 1 ? BTNodeStates.SUCCESS : BTNodeStates.FAILURE);
    }

    private BTNodeStates FindNewTarget()
    {
        int randomChance = Random.Range(0, targets.Length);
        var target = targets[randomChance];
        if (target != null)
        {
            SetTargetPosition(target);
            //Debug.Log("target found");
            return BTNodeStates.SUCCESS;
        }
        else
        {
            return BTNodeStates.FAILURE;
        }
    }

    public Vector3 GetTargetPosition()
    {
        Vector3 pos;
        if (targetObject != null)
        {
            pos = targetObject.transform.position;
        } else
        {
            pos = this.targetPosition;
        }
        return pos;
    }

    public void SetTargetPosition(GameObject target)
    {
        this.targetObject = target;
        this.targetPosition = target.transform.position;
    }
}
