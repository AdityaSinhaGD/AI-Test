using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : GOAPAgent
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Goal s1 = new Goal("isWaiting", 1, true);
        goals.Add(s1, 3);
    }

    
}
