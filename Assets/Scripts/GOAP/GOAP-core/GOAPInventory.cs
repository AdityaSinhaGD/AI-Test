using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPInventory
{
    List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject obj)
    {
        items.Add(obj);
    }

    public GameObject FindItemWithTag(string tag)
    {
        foreach(GameObject item in items)
        {
            if(item.tag == tag)
            {
                return item;
            }
        }
        return null;
    }

    public void RemoveItem(GameObject obj)
    {
        int indexToRemove = -1;
        foreach(GameObject item in items)
        {
            indexToRemove++;
            if (item.Equals(obj))
            {
                break;
            }
        }
        if (indexToRemove > -1)
        {
            items.RemoveAt(indexToRemove);
        }
    }
}
