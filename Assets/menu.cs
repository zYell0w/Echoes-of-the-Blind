using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerController controller;
    void Start()
    {
        gameObject.SetActive(false);
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
        if(controller!=null)
            controller.look_enabled = true;
        
    }

    private void OnEnable() {
        Time.timeScale = 0;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        if(controller!=null)
            controller.look_enabled = false;
    }

    public void OnExit()
    {
        SceneManager.LoadScene(0);
    }
}
