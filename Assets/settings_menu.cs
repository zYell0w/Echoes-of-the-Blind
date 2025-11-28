using UnityEngine;
using UnityEngine.InputSystem;

public class settings_menu : MonoBehaviour
{
    [SerializeField] GameObject entryMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
            gameObject.SetActive(false);

    }

    public void OnApplyButton()
    {
        gameObject.SetActive(false);
    }

    public void OnResChange()
    {
        
    }

    public void OnFulscreenToggle()
    {
        
    }

    public void OnVolumeSlider()
    {
        
    }

    void OnDisable()
    {
        entryMenu.SetActive(true);
    }

    
}
