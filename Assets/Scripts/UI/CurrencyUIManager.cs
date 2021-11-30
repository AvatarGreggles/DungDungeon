using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CurrencyUIManager : MonoBehaviour
{
    [SerializeField] Text currencyText;

    [SerializeField] Text gemText;

    [SerializeField] GameObject wallHitTip;

    bool tipIsShown = false;


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

    public IEnumerator HandleShowAnimation()
    {
        if (!tipIsShown)
        {
            tipIsShown = true;
            wallHitTip.SetActive(true);
            yield return new WaitForSeconds(2f);
            wallHitTip.SetActive(false);
            tipIsShown = false;
        }
    }
}
