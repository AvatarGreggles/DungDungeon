using System;
using UnityEngine;
using UnityEngine.UI;

public class StatPanel : MonoBehaviour
{

    public Button button;

    public int cost;
    [SerializeField] Text costText;
    [SerializeField] Image image;
    public Transform statHolder;

    [SerializeField] Sprite statBlockSprite;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCostText(cost.ToString());

    }

    public void AddStatPoint()
    {
        GameObject NewObj = new GameObject();
        Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
        NewImage.sprite = statBlockSprite;
        NewObj.transform.SetParent(statHolder, false);
    }

    public void ClearStatPoints()
    {
        Image[] statPoints = statHolder.GetComponentsInChildren<Image>();
        foreach (Image child in statPoints)
        {
            GameObject.Destroy(child.gameObject);

        }
    }


    public void UpdateCostText(string newText)
    {
        costText.text = newText;
    }

    public void UpdateSprite(Sprite newSprite)
    {
        statBlockSprite = newSprite;
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

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
