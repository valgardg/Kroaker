using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip sniperShot;
    [SerializeField] private AudioClip pistolShot;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip sniperReload;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySniperShot()
    {
        sfxSource.PlayOneShot(sniperShot);
    }

    public void PlayPlayerJump()
    {
        sfxSource.PlayOneShot(playerJump);
    }

    public void PlayReload()
    {
        sfxSource.PlayOneShot(sniperReload);
    }

    public void PlayPistolShot()
    {
        sfxSource.PlayOneShot(pistolShot);
    }
}
