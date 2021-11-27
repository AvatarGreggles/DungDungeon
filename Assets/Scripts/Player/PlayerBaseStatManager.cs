using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseStatManager : MonoBehaviour, ISavable
{

    public int gems = 0;
    // public int rankLevel = 1;
    // public int rankEXP = 0;
    // public int[] toRankUp = new int[1];
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

    void Start()
    {
        SavingSystem.i.Load("saveSlot3");
        DontDestroyOnLoad(this);
    }

    public object CaptureState()
    {

        var saveState = new PlayerBaseStatManagerSaveState()
        {
            totalGems = gems,
            totalBonusMaxHP = bonusMaxHP,
            totalBonusMaxShield = bonusMaxShield,
            totalBonusAttackPower = bonusAttackPower,
            totalBonusDefense = bonusDefense,
            totalBonusMoveSpeed = bonusMoveSpeed,
            totalBonusMaxDung = bonusMaxDung,
        };
        Debug.Log("Player base stat manager saved");

        return saveState;
    }

    public void RestoreState(object state)
    {
        PlayerBaseStatManagerSaveState loadedData = (PlayerBaseStatManagerSaveState)state;


        gems = loadedData.totalGems;
        bonusMaxHP = loadedData.totalBonusMaxHP;
        bonusMaxShield = loadedData.totalBonusMaxShield;
        bonusAttackPower = loadedData.totalBonusAttackPower;
        bonusDefense = loadedData.totalBonusDefense;
        bonusMoveSpeed = loadedData.totalBonusMoveSpeed;
        bonusMaxDung = loadedData.totalBonusMaxDung;

        Debug.Log("Player base stat manager  loaded");
    }


    [System.Serializable]
    public class PlayerBaseStatManagerSaveState
    {
        public int totalGems;
        public int totalBonusMaxHP;

        public int totalBonusMaxShield;
        public int totalBonusAttackPower;
        public int totalBonusDefense;
        public int totalBonusMoveSpeed;
        public int totalBonusMaxDung;

    }
}


