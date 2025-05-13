using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void CaricaParoleNascondino()
    {
        SceneManager.LoadScene("ParoleCasuali_4Lettere");
    }
    public void TornaAlMenu()
    {
        SceneManager.LoadScene("SelectMiniGames");
    }
}
