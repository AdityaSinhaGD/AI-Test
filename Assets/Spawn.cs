using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject patientPrefab;
    public int patientNumber;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPatient", 0f, 2f);
    }

    void SpawnPatient()
    {
        Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
    }
}
