using UnityEngine;
using UnityEngine.UI;

public class CurrencyUIManager : MonoBehaviour
{
    [SerializeField] Text currencyText;


    void Start()
    {
        UpdateCurrency();

    }

    public void UpdateCurrency()
    {
        currencyText.text = GameController.Instance.totalCurrency.ToString();
    }
}
