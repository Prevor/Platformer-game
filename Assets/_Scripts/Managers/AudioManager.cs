using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    AudioSource audioSource;

    [SerializeField] AudioClip coinSound, healthSound, chestSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Coin.OnCoinCollected += PlayCoinSound;
        HealthPotion.OnHealthPotionUsed += PlayHealthPotionound;
        Chest.OnChestCollected += PlayChestSound;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= PlayCoinSound;
        HealthPotion.OnHealthPotionUsed -= PlayHealthPotionound;
        Chest.OnChestCollected -= PlayChestSound;
    }

    private void PlayAudioClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void PlayCoinSound()
    {
        PlayAudioClip(coinSound);
    }

    private void PlayChestSound()
    {
        PlayAudioClip(chestSound);
    }

    private void PlayHealthPotionound()
    {
        if (!UpdateHealthBar.isMaxHealthBar)
        {
            PlayAudioClip(healthSound);
        }
    }
}
