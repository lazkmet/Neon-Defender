using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Canvas[] screens = { };
    public bool pausable;
    public Canvas pauseScreen;
    public bool isPaused { get; private set; }
    public void DeactivateAll() {
        foreach (Canvas c in screens)
        {
            c.gameObject.SetActive(false);
        }
    }
    public void SetActiveScreen(int i) {
        DeactivateAll();
        i = Mathf.Clamp(i, 0, screens.Length - 1);
        screens[i].gameObject.SetActive(true);
    }
    public void SetScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
    public void TogglePause()
    {
        if (pausable) {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                if (pauseScreen != null) {
                    pauseScreen.gameObject.SetActive(true);
                }
            }
            else {
                Time.timeScale = 1;
                if (pauseScreen != null)
                {
                    pauseScreen.gameObject.SetActive(false);
                }
            }
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Reset()
    {
        if (screens.Length > 0)
        {
            SetActiveScreen(0);
        }
        isPaused = false;
        Time.timeScale = 1;
        if (pausable) { 
            pauseScreen.gameObject.SetActive(false);
        }
    }
}
