using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatient : GOAPAction
{
    GameObject resource;

    public override bool PrePerform()
    {
        target = GOAPWorld.Instance.RemovePatient();//assinging a patient to a nurse
        if (target == null)
        {
            return false;
        }

        resource = GOAPWorld.Instance.RemoveCubicle();
        if (resource != null)
        {
            inventory.AddItem(resource);//adding a free cubicle to nurse's inventory
        }
        else
        {
            GOAPWorld.Instance.AddPatient(target);
            target = null;
            return false;
        }

        GOAPWorld.Instance.GetWorldStates().ModifyState("freeCubicle", -1);
        return true;
    }

    public override bool PostPerform()
    {
        GOAPWorld.Instance.GetWorldStates().ModifyState("patientWaiting", -1);
        if (target)
        {
            if (target.GetComponent<GOAPAgent>())
            {
                target.GetComponent<GOAPAgent>().inventory.AddItem(resource);//Nurse agent adds cubicle resource to patient's inventory
            }
        }
        return true;
    }
}
