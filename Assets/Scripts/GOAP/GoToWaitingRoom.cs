using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingRoom : GOAPAction
{
    public override bool PostPerform()
    {
        GOAPWorld.Instance.GetWorldStates().ModifyState("patientWaiting", 1);
        return true;
    }

    public override bool PrePerform()
    {
        return true;
    }
}
