using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    private int coinCount = 1, cointCountInChest = new System.Random().Next(10, 20);
    private int totalCoins;

    private void Awake() => coinText = GetComponent<TextMeshProUGUI>();

    private void OnEnable()
    {
        Coin.OnCoinCollected += AddCoin;
        Chest.OnChestCollected += AddChestCoin;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= AddCoin;
        Chest.OnChestCollected -= AddChestCoin;
    }

    private void AddCoin()
    {
        totalCoins += coinCount;
        coinText.text = totalCoins.ToString();
    }

    private void AddChestCoin()
    {
        totalCoins += cointCountInChest;
        coinText.text = totalCoins.ToString();
    }
}
