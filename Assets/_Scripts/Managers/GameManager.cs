using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // UI Animations
    public Animator _menuController;

    [HideInInspector] public List<ArrowData> arrows;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);

            return;
        }

        Instance = this;

        // Load arrows
        arrows = Resources.LoadAll<ArrowData>("Arrows").ToList();
    }

}
