﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nurse : GOAPAgent
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Goal s1 = new Goal("treatPatient", 1, false);//non removable goal
        goals.Add(s1, 3);
    }

    
}
