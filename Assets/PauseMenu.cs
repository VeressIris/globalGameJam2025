using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject OptionsPanel;
    bool paused = false;
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!paused)
            {
                paused = true;
                pauseMenu.SetActive(true);
                OptionsPanel.SetActive(false);
                Time.timeScale = 0;
            }
            else
            {
                paused = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;

    }
    public void Resume()
    {
        pauseMenu?.SetActive(false);
        Time.timeScale = 1;
    }
    public void Options()
    {
        pauseMenu.SetActive(false);
        OptionsPanel.SetActive(true);
    }
}