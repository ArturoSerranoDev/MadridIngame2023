using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] public AudioClip hitGarbageClip;
    [SerializeField] public AudioClip jumpSoundClip;
    [SerializeField] public AudioClip CollectGarbageClip;
    
    [SerializeField] public AudioClip WinRoundClip;
    
    [SerializeField] public AudioClip safeFromHeatClip;
    [SerializeField] public AudioClip heatClip;
    
    [SerializeField] public AudioClip waterClip;
    
    [SerializeField] public AudioClip grabCloudClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        // Play the audio clip using AudioSource or any other audio system of your choice
        // For simplicity, let's assume you have an AudioSource component attached to the AudioManager game object
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
