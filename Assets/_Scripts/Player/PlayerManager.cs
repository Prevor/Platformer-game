using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player;
    public static PlayerManager instance;
    private void Awake()
    {
            instance = this;
    }
}
