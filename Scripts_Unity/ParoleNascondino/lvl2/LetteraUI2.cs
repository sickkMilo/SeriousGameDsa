using UnityEngine;
using UnityEngine.UI;

public class LetteraUI2 : MonoBehaviour
{
    private char carattere;
    private ParoleCasualiManager_5Lettere manager;
    private Vector3 posizioneIniziale;

    public void Setup(char c, ParoleCasualiManager_5Lettere m, Vector3 posizione)
    {
        carattere = c;
        manager = m;
        posizioneIniziale = posizione;

        GetComponent<Button>().onClick.AddListener(() =>
        {
            manager.InserisciLettera(carattere, GetComponent<Image>().sprite, this.gameObject);
        });
    }

    public void ResetPosition()
    {
        transform.position = posizioneIniziale;
        gameObject.SetActive(true);
    }
}
