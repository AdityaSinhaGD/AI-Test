using System;
public class ActionNode : BTNode
{
    public delegate BTNodeStates ActionNodeDelegate();

    private ActionNodeDelegate nodeDelegate;

    public ActionNode(ActionNodeDelegate nodeDelegate)
    {
        this.nodeDelegate = nodeDelegate;
    }

    public override BTNodeStates Evaluate()
    {
        switch (nodeDelegate())
        {
            case BTNodeStates.SUCCESS:
                currentNodeState = BTNodeStates.SUCCESS;
                return currentNodeState;
            case BTNodeStates.FAILURE:
                currentNodeState = BTNodeStates.FAILURE;
                return currentNodeState;
            case BTNodeStates.RUNNING:
                currentNodeState = BTNodeStates.RUNNING;
                return currentNodeState;
            default:
                currentNodeState = BTNodeStates.FAILURE;
                return currentNodeState;
        }
    }
}
