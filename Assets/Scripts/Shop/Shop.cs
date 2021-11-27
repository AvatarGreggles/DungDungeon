using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class Shop : MonoBehaviour
{

    [SerializeField] GameObject shopItemUI;
    public List<Item> shopItems;

    [SerializeField] Transform playerShopUI;

    public AudioClip switchItemSound;
    public AudioClip selectItemSound;
    AudioSource audioSource;


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

    [SerializeField] GameObject outOfStockObject;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void CloseShop()
    {
        currentItemSelected = -1;
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




    // Start is called before the first frame updates

    private void OnEnable()
    {
        ClearItems();
        GenerateItems();
    }

    public void GenerateItems()
    {
        player = FindObjectOfType<Player>();
        playerInput = player.GetComponent<PlayerInput>();

        UpdateCurrency();
        UpdateHPText();
        UpdateShieldText();
        UpdateAttackText();
        UpdateSpeedText();
        UpdateCritRatioText();

        playerInput.SwitchCurrentActionMap("LevelUpMenu");
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

    public void ClearItems()
    {
        ShopItem[] currentItems = playerShopUI.GetComponentsInChildren<ShopItem>();

        foreach (ShopItem shopItem in currentItems)
        {
            shopItem.gameObject.SetActive(false);
        }
        currentItemSelected = -1;
    }



    public void HandleNavigation(InputValue value)
    {
        ShopItem[] currentItems = playerShopUI.GetComponentsInChildren<ShopItem>();
        if (currentItems.Length == 0)
        {
            return;
        }

        navigateMovement = value.Get<Vector2>();


        if (navigateMovement.y < 0f)
        {
            audioSource.PlayOneShot(switchItemSound, 1f);
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

            currentItems[currentItemSelected].SetItemAsSelected();

        }
        else if (navigateMovement.y > 0f)
        {
            audioSource.PlayOneShot(switchItemSound, 1f);
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

            currentItems[currentItemSelected].SetItemAsSelected();

        }

    }

    public void HandleInteract()
    {
        if (currentItemSelected == -1)
        {
            return;
        }

        ShopItem[] currentItems = playerShopUI.GetComponentsInChildren<ShopItem>();

        audioSource.PlayOneShot(selectItemSound, 1f);
        // ShopItem[] items = FindObjectsOfType<ShopItem>();

        if (currentItemSelected != -1)
        {


            bool successfulPurchase = currentItems[currentItemSelected].PurchaseItem();

            if (successfulPurchase)
            {
                Debug.Log("purchased, should remove");
                currentItems[currentItemSelected].gameObject.SetActive(false);
                currentItemSelected = 0;
                currentItems[currentItemSelected].SetItemAsSelected();

                ShopItem[] availableItems = playerShopUI.GetComponentsInChildren<ShopItem>();
                if (availableItems.Length == 0)
                {
                    outOfStockObject.SetActive(true);
                    currentItemSelected = -1;
                }

            }
        }
    }
}
