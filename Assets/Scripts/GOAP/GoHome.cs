using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHome : GOAPAction
{
    public override bool PostPerform()
    {
        return true;
    }

    public override bool PrePerform()
    {
        target = GameObject.FindGameObjectWithTag("Home");
        if (target == null)
        {
            return false;
        }
        return true;
    }

}
