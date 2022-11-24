using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatBlockManager
{
    public int cost;
    public Text costText;
    public Button upgradeButton;
    public Action callback;
}
