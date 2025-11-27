using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Entry_menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject settingsMenu;
    [SerializeField] Image Loading;
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlay()
    {
        //SceneManager.LoadSceneAsync(1);
        StartCoroutine(LoadScene(1));
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnSettings()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    IEnumerator LoadScene(int id)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(id);
        Loading.gameObject.SetActive(true);
        while(!op.isDone)
        {
            float progressValue = Mathf.Clamp01(op.progress / 0.5f);
            Loading.fillAmount = progressValue;
            yield return null;

        }
    }
}
