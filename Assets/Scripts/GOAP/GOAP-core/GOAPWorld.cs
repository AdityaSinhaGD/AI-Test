using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GOAPWorld
{
    private static readonly GOAPWorld instance = new GOAPWorld();
    
    private static WorldStates world;

    static GOAPWorld()
    {
        world = new WorldStates();
    }

    private GOAPWorld()
    {

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
