using UnityEngine;
using UnityEngine.SceneManagement; // Importante per cambiare scena

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("SelectMiniGames");
    }
}
