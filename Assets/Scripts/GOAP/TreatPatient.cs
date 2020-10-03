using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatPatient : GOAPAction
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
        GOAPWorld.Instance.GetWorldStates().ModifyState("Treating Patient", 1);

        inventory.RemoveItem(target);
        GOAPWorld.Instance.AddCubicle(target);
        GOAPWorld.Instance.GetWorldStates().ModifyState("freeCubicle", 1);

        return true;
    }
}
