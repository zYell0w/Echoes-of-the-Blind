using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnContinue()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Time.timeScale = 1;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void OnEnable() {
        Time.timeScale = 0;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        
    }

    public void OnExit()
    {
        //sahbe değişimi
    }
}
