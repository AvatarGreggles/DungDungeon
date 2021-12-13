using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVariant : MonoBehaviour
{

    [SerializeField] DoorManager door;
    void Start()
    {
        LevelManager.Instance.door = door;
    }

}
