using TMPro;
using UnityEngine;

public class CoinCounterScript : MonoBehaviour
{
    private TMPro.TextMeshProUGUI numberText;
    private int coinCount { get; set; } = 0;

    void Start()
    {
        numberText = GetComponent<TMPro.TextMeshProUGUI>();
        UpdateNumber();
    }

    public void AddCoin()
    {
        coinCount++;
        UpdateNumber();
    }

    private void UpdateNumber()
    {
        numberText.text = coinCount.ToString();
    }
}
