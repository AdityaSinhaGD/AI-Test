using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingRoom : GOAPAction
{
    public override bool PostPerform()
    {
        GOAPWorld.Instance.GetWorldStates().ModifyState("patientWaiting", 1);
        GOAPWorld.Instance.AddPatient(this.gameObject);//adding a waiting patient game object to world resources.
        agentBeliefs.ModifyState("atHospital", 1);//cannot be a global world state. needs ot be a local belief.
        return true;
    }

    public override bool PrePerform()
    {
        return true;
    }
}
