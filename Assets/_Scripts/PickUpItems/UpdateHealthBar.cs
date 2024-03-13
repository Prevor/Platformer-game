using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthBar : MonoBehaviour
{
    private Slider hitpointBarSlider;
    private int countHealthPoint = 3;

    [SerializeField] private TextMeshProUGUI healthText;
    public static bool isMaxHealthBar;

    [SerializeField] private Player player;

    private void Awake()
    {
        hitpointBarSlider = GetComponent<Slider>();
    }
    
    private void Start()
    {
        hitpointBarSlider.maxValue = player._maxHealth;
        hitpointBarSlider.value = player._health;
        healthText.text = hitpointBarSlider.value.ToString();
    }

    private void OnEnable() => HealthPotion.OnHealthPotionUsed += IncreaseHealth;

    private void OnDisable() => HealthPotion.OnHealthPotionUsed -= IncreaseHealth;

    private void Update()
    {
        if (CheakHealth())
        {
            player._health = player._maxHealth;
        }
    }

    public void IncreaseHealth()
    {
        if (!CheakHealth())
        {
            hitpointBarSlider.value += countHealthPoint;
            healthText.text = hitpointBarSlider.value.ToString();
            player._health += countHealthPoint;
        }
    }

    private bool CheakHealth()
    {
        if(hitpointBarSlider.value >= hitpointBarSlider.maxValue)
        {
            isMaxHealthBar = true;
            healthText.text = "max";
        }
        else isMaxHealthBar = false;

        return isMaxHealthBar;
    }
}
