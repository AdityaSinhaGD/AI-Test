using System;
using System.Collections.Generic;
public class Selector : BTNode
{
    private List<BTNode> childNodes = new List<BTNode>();

    public Selector(List<BTNode> childNodes)
    {
        this.childNodes = childNodes;
    }
    public override BTNodeStates Evaluate()
    {
        foreach(BTNode node in childNodes)
        {
            switch (node.Evaluate())
            {
                case BTNodeStates.FAILURE:
                    continue;
                case BTNodeStates.SUCCESS:
                    currentNodeState = BTNodeStates.SUCCESS;
                    return currentNodeState;
                case BTNodeStates.RUNNING:
                    currentNodeState = BTNodeStates.RUNNING;
                    return currentNodeState;
                default:
                    continue;
            }
        }
        currentNodeState = BTNodeStates.FAILURE;
        return currentNodeState;
    }
}
