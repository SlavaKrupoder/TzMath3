using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour
{
    public void ScenesManager()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitToGame()
    {
        Application.Quit();
    }
}