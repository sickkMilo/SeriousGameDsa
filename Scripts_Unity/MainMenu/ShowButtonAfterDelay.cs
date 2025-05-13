using System.Collections;
using UnityEngine;

public class ShowButtonAfterDelay : MonoBehaviour
{
    public GameObject buttonToShow;
    public float delay = 2.0f; // Aspettiamo 2 secondi

    void Start()
    {
        buttonToShow.SetActive(false); // All'inizio nascondiamo il bottone
        StartCoroutine(ShowButton());
    }

    IEnumerator ShowButton()
    {
        yield return new WaitForSeconds(delay);
        buttonToShow.SetActive(true); // Dopo il delay mostriamo il bottone
    }
}
