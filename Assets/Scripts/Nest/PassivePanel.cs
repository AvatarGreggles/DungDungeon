using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassivePanel : MonoBehaviour
{

    public Button button;

    public void SetButton(Button newButton)
    {
        button = newButton;
    }

    public void SetOnClick(Action callback)
    {
        button.onClick.AddListener(() =>
          {
              callback();
          });
    }
}
