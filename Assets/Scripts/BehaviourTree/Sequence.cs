using System;
using System.Collections.Generic;

public class Sequence : BTNode
{
    private List<BTNode> childNodes = new List<BTNode>();

    public Sequence(List<BTNode> childNodes)
    {
        this.childNodes = childNodes;
    }

    public override BTNodeStates Evaluate()
    {
        bool isAnyChildNodeRunning = false;
        foreach(BTNode node in childNodes)
        {
            switch (node.Evaluate())
            {
                case BTNodeStates.FAILURE:
                    currentNodeState = BTNodeStates.FAILURE;
                    return currentNodeState;

                case BTNodeStates.SUCCESS:
                    continue;

                case BTNodeStates.RUNNING:
                    isAnyChildNodeRunning = true;
                    continue;

                default:
                    currentNodeState = BTNodeStates.SUCCESS;
                    return currentNodeState;
            }
        }
        currentNodeState = isAnyChildNodeRunning ? BTNodeStates.RUNNING : BTNodeStates.SUCCESS;
        return currentNodeState;
    }
}
