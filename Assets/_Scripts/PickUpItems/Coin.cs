using System;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    public static event Action OnCoinCollected;

    public void Collect()
    {
        Destroy(gameObject);
        OnCoinCollected?.Invoke();
    }

}
