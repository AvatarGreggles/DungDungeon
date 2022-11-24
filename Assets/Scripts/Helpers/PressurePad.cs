using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour, IPlayerTriggerable
{

    public bool isActivated = false;
    [SerializeField] ActivatorDoor activatable;

    public void OnPlayerTriggered(Player player)
    {
        if (!isActivated)
        {
            Debug.Log("You stood on a pressure pad");
            activatable.Activate();
            isActivated = true;
        }
    }
}
