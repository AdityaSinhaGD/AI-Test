using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurse : GOAPAgent
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("treatPatient", 1, true);
        goals.Add(s1, 3);
    }

    
}
