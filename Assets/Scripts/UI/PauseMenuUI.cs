using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject container;
    public TimeManager timeManager;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (container.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        timeManager.OnLoadTimeSpeed(); // Resume the game
        container.SetActive(false); // Hide the pause menu
    }

    void Pause()
    {
        timeManager.OnSaveTimeSpeed(); // Pause the game
        timeManager.OnTimeStop();
        container.SetActive(true); // Show the pause menu
    }

    public void MainMenuButton()
    {
        // Implement main menu logic here
        timeManager.OnTimeResume(); // Ensure time is resumed before loading the menu
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        Debug.Log("Main Menu Button Clicked");
    }
}
