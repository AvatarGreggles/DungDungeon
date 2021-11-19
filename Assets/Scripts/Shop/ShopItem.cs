using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    [SerializeField] public Item item;
    [SerializeField] Text priceText;
    [SerializeField] Text itemNameText;
    [SerializeField] Text itemDescriptionText;

    [SerializeField] Image itemIcon;

    Player player;

    Shop shop;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectsOfType<Player>()[0];

        shop = FindObjectOfType<Shop>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItemNameText()
    {
        itemNameText.text = item.name;
    }

    public void SetItemDescriptionText()
    {
        itemDescriptionText.text = item.itemDescription;
    }

    public void SetPriceText()
    {
        priceText.text = item.price.ToString();
    }

    public void SetItemIcon()
    {
        itemIcon.sprite = item.itemIcon;
    }

    public void PurchaseItem()
    {
        int currencyAfterPurchase = GameController.Instance.totalCurrency - item.price;

        if (currencyAfterPurchase >= 0)
        {
            GameController.Instance.RemoveCurrency(item.price);
            shop.UpdateCurrency();
            item.IncreaseStat(player, shop);
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }
}
