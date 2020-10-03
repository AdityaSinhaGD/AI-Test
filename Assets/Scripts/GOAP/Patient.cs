using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : GOAPAgent
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Goal s1 = new Goal("isWaiting", 1, true);
        Goal s2 = new Goal("isTreated", 1, true);
        Goal s3 = new Goal("goHome", 1, true);
        goals.Add(s1, 3);
        goals.Add(s2, 5);
        goals.Add(s3, 4);
    }

    
}
