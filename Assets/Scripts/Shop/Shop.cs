using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    [SerializeField] Text critRatioStatText;

    [SerializeField] int shopItemOfferCount = 3;

    int currentItemSelected = -1;

    private Vector2 navigateMovement;

    Player player;

    PlayerInput playerInput;

    List<Item> randomShopItems;

    public void CloseShop()
    {
        gameObject.SetActive(false);
        GameController.Instance.currentState = State.Active;
        playerInput.SwitchCurrentActionMap("Player");
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

    public void UpdateCritRatioText()
    {
        critRatioStatText.text = player.criticalHitRatio.ToString();
    }




    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<Player>();
        playerInput = player.GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("LevelUpMenu");

        UpdateCurrency();
        UpdateHPText();
        UpdateShieldText();
        UpdateAttackText();
        UpdateSpeedText();
        UpdateCritRatioText();

        randomShopItems = HelperMethods.GetRandomItemsFromList<Item>(shopItems, shopItemOfferCount);

        foreach (Item shopItem in randomShopItems)
        {
            GameObject newShopItem = Instantiate(shopItemUI, shopItemUI.transform.position, shopItemUI.transform.rotation);
            if (newShopItem)
            {
                newShopItem.GetComponent<ShopItem>().item = shopItem;
                newShopItem.GetComponent<ShopItem>().SetPriceText();
                newShopItem.GetComponent<ShopItem>().SetItemNameText();
                newShopItem.GetComponent<ShopItem>().SetItemDescriptionText();
                newShopItem.GetComponent<ShopItem>().SetItemIcon();
                newShopItem.transform.SetParent(playerShopUI);
            }

        }
    }

    public void HandleNavigation(InputValue value)
    {

        navigateMovement = value.Get<Vector2>();



        // if (navigateMovement.x == 0f)
        // {
        //     return;
        // }

        ShopItem[] currentItems = playerShopUI.GetComponentsInChildren<ShopItem>();


        if (navigateMovement.y < 0f)
        {
            foreach (ShopItem item in currentItems)
            {
                item.UnsetItemAsSelected();
            }
            currentItemSelected++;


            if (currentItemSelected < 0)
            {
                currentItemSelected = currentItems.Length - 1;
            }

            if (currentItemSelected > currentItems.Length - 1)
            {
                currentItemSelected = 0;
            }
        }
        else if (navigateMovement.y > 0f)
        {
            foreach (ShopItem item in currentItems)
            {
                item.UnsetItemAsSelected();
            }
            currentItemSelected--;


            if (currentItemSelected < 0)
            {
                currentItemSelected = currentItems.Length - 1;
            }

            if (currentItemSelected > currentItems.Length - 1)
            {
                currentItemSelected = 0;
            }
        }


        currentItems[currentItemSelected].SetItemAsSelected();

    }

    public void HandleInteract()
    {
        if (currentItemSelected != -1)
        {
            Debug.Log(currentItemSelected);
            ShopItem[] currentItems = playerShopUI.GetComponentsInChildren<ShopItem>();
            currentItems[currentItemSelected].PurchaseItem();

        }
    }
}
