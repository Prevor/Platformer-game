using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthBar : MonoBehaviour
{
    private Slider hitpointBarSlider;
    private int countHealthPoint = 3;

    [SerializeField] private TextMeshProUGUI healthText;
    public static bool isMaxHealthBar;

    [SerializeField] private PlayerHealth _playerHealth;

    private void Awake()
    {
        hitpointBarSlider = GetComponent<Slider>();
    }
    
    private void Start()
    {
        hitpointBarSlider.maxValue = _playerHealth.maxHealth;
        hitpointBarSlider.value = _playerHealth.health;
        healthText.text = hitpointBarSlider.value.ToString();
    }

    private void OnEnable() => HealthPotion.OnHealthPotionUsed += IncreaseHealth;

    private void OnDisable() => HealthPotion.OnHealthPotionUsed -= IncreaseHealth;

    private void Update()
    {
        hitpointBarSlider.value = _playerHealth.health;
        healthText.text = hitpointBarSlider.value.ToString();
        if (CheakHealth())
        {
            _playerHealth.health = _playerHealth.maxHealth;
        }
    }

    public void IncreaseHealth()
    {
        if (!CheakHealth())
        {
            hitpointBarSlider.value += countHealthPoint;
            healthText.text = hitpointBarSlider.value.ToString();
            _playerHealth.health += countHealthPoint;
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
