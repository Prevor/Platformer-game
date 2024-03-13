using System;
using UnityEngine;

public class Chest : MonoBehaviour, ICollectible
{
    public static event Action OnChestCollected;

    public void Collect()
    {
        Destroy(gameObject);
        OnChestCollected?.Invoke();
    }
}
