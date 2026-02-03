using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class settings_menu : MonoBehaviour
{
    [SerializeField] GameObject entryMenu;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioMixer audioMixer;
    
    [Header("Debug")]
    [SerializeField] Text debugText; // Optional: Assign a UI Text element to see debug messages

    void Start()
    {
        
      
        audioMixer.GetFloat("MasterVolume", out float currentVolume);

        volumeLevel = Mathf.Pow(10, currentVolume / 20f);
        volumeSlider.value = volumeLevel;
        
        fullscreenToggle.isOn = Screen.fullScreen;
        isFullscreen = Screen.fullScreen;
        gameObject.SetActive(false);

    }

    bool isFullscreen = false;
    float volumeLevel = 0.8f;

    public void OnVolumeSlider(float volume)
    {
        volumeLevel = Mathf.Clamp01(volume);
        
        
    }

    // Keep your other methods the same...
    public void OnApplyButton()
    {
        gameObject.SetActive(false);
        Screen.fullScreen = isFullscreen;
        float dB = volumeLevel > 0.0001f ? Mathf.Log10(volumeLevel) * 20 : -80f;
        audioMixer.SetFloat("MasterVolume", dB);

        
    }

    public void OnFulscreenToggle(bool toggle)
    {
        isFullscreen = toggle;
    }

    void OnDisable()
    {
        if (entryMenu != null)
            entryMenu.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }
}