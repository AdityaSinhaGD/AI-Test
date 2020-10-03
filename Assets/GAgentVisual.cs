using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GAgentVisual : MonoBehaviour
{
    public GOAPAgent thisAgent;

    // Start is called before the first frame update
    void Start()
    {
        thisAgent = this.GetComponent<GOAPAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
