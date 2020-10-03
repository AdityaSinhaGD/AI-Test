using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GOAPWorld
{
    private static readonly GOAPWorld instance = new GOAPWorld();
    
    private static WorldStates world;

    private static Queue<GameObject> patients;
    private static Queue<GameObject> cubicles;

    static GOAPWorld()
    {
        world = new WorldStates();

        patients = new Queue<GameObject>();

        cubicles = new Queue<GameObject>();
        GameObject[] cubiclesInWorld = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach(GameObject cubicle in cubiclesInWorld)
        {
            cubicles.Enqueue(cubicle);
        }
        if (cubiclesInWorld.Length > 0)
        {
            world.ModifyState("freeCubicle", cubiclesInWorld.Length);
        }
    }

    private GOAPWorld()
    {

    }

    public void AddPatient(GameObject patient)
    {
        patients.Enqueue(patient);
    }

    public GameObject RemovePatient()
    {
        if (patients.Count == 0)
        {
            return null;
        }
        return patients.Dequeue();
    }

    public void AddCubicle(GameObject cubicle)
    {
        cubicles.Enqueue(cubicle);
    }

    public GameObject RemoveCubicle()
    {
        if (cubicles.Count == 0)
        {
            return null;
        }
        return cubicles.Dequeue();
    }

    public WorldStates GetWorldStates()
    {
        return world;
    }

    public static GOAPWorld Instance
    {
        get
        {
            return instance;
        }
    }
}
