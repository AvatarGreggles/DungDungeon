using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    [SerializeField] Image itemCardSpriteRenderer;
    [SerializeField] Sprite selectedItemCard;
    [SerializeField] Sprite defaultItemCard;

    //Todo: common, rare, epic?

    [SerializeField] public Item item;
    [SerializeField] Text priceText;
    [SerializeField] Text itemNameText;
    [SerializeField] Text itemDescriptionText;

    [SerializeField] Image itemIcon;

    Player player;

    Shop shop;

    public bool isSelected = false;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectsOfType<Player>()[0];

        shop = FindObjectOfType<Shop>();
    }


    public void SetItemAsSelected()
    {

        itemCardSpriteRenderer.sprite = selectedItemCard;
        isSelected = true;
    }

    public void UnsetItemAsSelected()
    {

        itemCardSpriteRenderer.sprite = defaultItemCard;
        isSelected = false;
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

    public bool PurchaseItem()
    {
        if (item.itemType == Item.ItemType.Heal && player.health >= player.maxHealth)
        {
            Debug.Log("Your health is already full");
            return false;
        }

        int currencyAfterPurchase = GameController.Instance.totalCurrency - item.price;

        if (currencyAfterPurchase >= 0)
        {
            GameController.Instance.RemoveCurrency(item.price);
            shop.UpdateCurrency();
            if (item.itemType == Item.ItemType.Boost)
            {
                item.IncreaseStat(player, shop);
            }

            if (item.itemType == Item.ItemType.Heal)
            {
                item.HealStat(player, shop);
            }

            if (item.itemType == Item.ItemType.Equip)
            {
                item.EnableAbility(player.GetComponent<PlayerAbilities>());
            }

            Debug.Log("You purchased " + item.itemName);

            return true;

        }
        else
        {
            Debug.Log("Not enough money");
            return false;
        }
    }
}
