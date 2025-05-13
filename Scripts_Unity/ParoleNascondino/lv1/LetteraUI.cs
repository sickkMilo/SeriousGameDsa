using UnityEngine;
using UnityEngine.UI;

public class LetteraUI : MonoBehaviour
{
    private char carattere;
    private ParoleCasualiManager manager;
    private Vector3 posizioneIniziale;

    public void Setup(char c, ParoleCasualiManager m, Vector3 posizione)
    {
        carattere = c;
        manager = m;
        posizioneIniziale = posizione;

        GetComponent<Button>().onClick.AddListener(() => manager.InserisciLettera(carattere, GetComponent<Image>().sprite, this.gameObject));
    }

    public void ResetPosition()
    {
        transform.position = posizioneIniziale;
        gameObject.SetActive(true);
    }
}
