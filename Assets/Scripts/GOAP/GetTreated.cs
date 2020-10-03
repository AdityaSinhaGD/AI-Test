using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTreated : GOAPAction
{
   

    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
        {
            return false;
        }
        return true;
    }

    public override bool PostPerform()
    {
        GOAPWorld.Instance.GetWorldStates().ModifyState("treated", 1);
        inventory.RemoveItem(target);
        agentBeliefs.ModifyState("isTreatedAtHospital", 1);
        return true;
    }

}
