using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    [SerializeField] GameObject shopItemUI;
    public List<Item> shopItems;

    [SerializeField] Transform playerShopUI;


    [SerializeField] Text currencyText;

    [SerializeField] Text healthStatText;
    [SerializeField] Text shieldStatText;
    [SerializeField] Text attackStatText;
    [SerializeField] Text speedStatText;

    Player player;

    public void CloseShop()
    {
        gameObject.SetActive(false);
        GameController.Instance.currentState = State.Active;
    }

    public void UpdateCurrency()
    {
        currencyText.text = GameController.Instance.totalCurrency.ToString();
    }

    public void UpdateSpeedText()
    {
        speedStatText.text = player.attackSpeedBonus.ToString();
    }

    public void UpdateHPText()
    {
        healthStatText.text = player.health.ToString() + " / " + player.maxHealth.ToString();
    }

    public void UpdateShieldText()
    {
        shieldStatText.text = player.shield.ToString() + " / " + player.maxShield.ToString();
    }

    public void UpdateAttackText()
    {
        attackStatText.text = player.attack.ToString();
    }


    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<Player>();

        UpdateCurrency();
        UpdateHPText();
        UpdateShieldText();
        UpdateAttackText();
        UpdateSpeedText();

        foreach (Item shopItem in shopItems)
        {
            GameObject newShopItem = Instantiate(shopItemUI, shopItemUI.transform.position, shopItemUI.transform.rotation);
            if (newShopItem)
            {
                Debug.Log(newShopItem);
                newShopItem.GetComponent<ShopItem>().item = shopItem;
                newShopItem.GetComponent<ShopItem>().SetPriceText();
                newShopItem.GetComponent<ShopItem>().SetItemNameText();
                newShopItem.GetComponent<ShopItem>().SetItemDescriptionText();
                newShopItem.GetComponent<ShopItem>().SetItemIcon();
                newShopItem.transform.SetParent(playerShopUI);
            }

        }
    }
}
