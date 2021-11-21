using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMethods : MonoBehaviour
{
    public static List<T> GetRandomItemsFromList<T>(List<T> list, int number)
    {
        // this is the list we're going to remove picked items from
        List<T> tmpList = new List<T>(list);
        // this is the list we're going to move items to
        List<T> newList = new List<T>();

        // make sure tmpList isn't already empty
        while (newList.Count < number && tmpList.Count > 0)
        {
            int index = Random.Range(0, tmpList.Count);
            newList.Add(tmpList[index]);
            tmpList.RemoveAt(index);
        }

        return newList;
    }
}
