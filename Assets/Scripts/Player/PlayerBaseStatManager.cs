using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseStatManager : MonoBehaviour
{

    public int bonusMaxHP = 0;
    public int bonusMaxShield = 0;
    public int bonusAttackPower = 0;
    public int bonusDefense = 0;
    public int bonusMoveSpeed = 0;
    public int bonusMaxDung = 0;

    public static PlayerBaseStatManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }

    void Start() { DontDestroyOnLoad(this); }
}
