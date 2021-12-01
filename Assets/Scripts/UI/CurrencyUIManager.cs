using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CurrencyUIManager : MonoBehaviour
{
    [SerializeField] Text currencyText;

    [SerializeField] Text gemText;

    [SerializeField] Text controlsText;

    [SerializeField] GameObject wallHitTip;

    bool tipIsShown = false;

    public bool controlsAreShown = true;


    void Start()
    {
        UpdateCurrency();
        UpdateGems();

    }

    public void UpdateCurrency()
    {
        currencyText.text = GameController.Instance.totalCurrency.ToString();
    }

    public void UpdateGems()
    {
        gemText.text = PlayerBaseStatManager.instance.gems.ToString();
    }

    public void ShowWallHitTip()
    {
        StartCoroutine(HandleShowAnimation());
    }

    public void ShowControls()
    {
        controlsAreShown = true;
        controlsText.gameObject.SetActive(true);
    }

    public void HideControls()
    {
        controlsAreShown = false;
        controlsText.gameObject.SetActive(false);
    }

    public void ToggleControls()
    {
        if (controlsAreShown)
        {
            HideControls();
        }
        else
        {
            ShowControls();
        }
    }
    public IEnumerator HandleShowAnimation()
    {
        if (!tipIsShown)
        {
            tipIsShown = true;
            wallHitTip.SetActive(true);
            yield return new WaitForSeconds(10f);
            wallHitTip.SetActive(false);
            tipIsShown = false;
        }
    }
}
