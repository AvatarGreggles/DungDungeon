using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorDoor : MonoBehaviour, Activatable
{
    public void Activate()
    {
        gameObject.SetActive(false);
    }
}
