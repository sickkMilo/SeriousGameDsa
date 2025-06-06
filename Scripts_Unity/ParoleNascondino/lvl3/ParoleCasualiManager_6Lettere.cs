using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ParoleCasualiManager_6Lettere : MonoBehaviour
{
    [Header("UI")]
    public Image[] slotImages; // 6 slot
    public Button verificaButton;
    public Button menuButton;
    public GameObject livelloCompletatoPanel;
    public TextMeshProUGUI counterText;

    [Header("Lettere")]
    public GameObject letterButtonPrefab;
    public Transform[] spawnPositions; // 7 posizioni (6 lettere + 1 extra)
    public Sprite[] alphabetSprites;
    public Sprite emptySlotSprite;

    [Header("Gestione")]
    public string nomeScenaSuccessiva = "TerminaGioco";

    private string[] parole = {
        "FUMATA", "GROTTA", "TARZAN", "PIETRA", "TRONCO", "PALCHI", "SCOGLI", "STERNO", "TAVOLO", "VIAGGI"};

    private string parolaCorrente = "";
    private string parolaPrecedente = "";
    private List<GameObject> lettereAttive = new List<GameObject>();
    private char[] lettereInserite;
    private int paroleIndovinate = 0;

    void Start()
    {
        livelloCompletatoPanel.SetActive(false);
        lettereInserite = new char[slotImages.Length];
        paroleIndovinate = 0;
        AggiornaContatore();
        ResetLivello();
    }

    void ResetLivello()
    {
        lettereInserite = new char[slotImages.Length];

        foreach (var slot in slotImages)
        {
            slot.sprite = emptySlotSprite;
            slot.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        foreach (var lettera in lettereAttive)
        {
            Destroy(lettera);
        }

        lettereAttive.Clear();

        do
        {
            parolaCorrente = parole[Random.Range(0, parole.Length)];
        } while (parolaCorrente == parolaPrecedente);

        parolaPrecedente = parolaCorrente;

        GeneraLettere();
    }

    void GeneraLettere()
    {
        List<char> lettereFinali = new List<char>(parolaCorrente.ToCharArray());

        char extra;
        do
        {
            extra = (char)Random.Range('A', 'Z' + 1);
        } while (lettereFinali.Contains(extra));

        lettereFinali.Add(extra);
        lettereFinali = Mischia(lettereFinali);

        for (int i = 0; i < lettereFinali.Count; i++)
        {
            GameObject letterObj = Instantiate(letterButtonPrefab, spawnPositions[i].position, Quaternion.identity, spawnPositions[i]);
            letterObj.GetComponentInChildren<Image>().sprite = GetSprite(lettereFinali[i]);
            LetteraUI3 script = letterObj.GetComponent<LetteraUI3>();
            script.Setup(lettereFinali[i], this, letterObj.transform.position);
            lettereAttive.Add(letterObj);
        }
    }

    public void InserisciLettera(char lettera, Sprite sprite, GameObject letterObj)
    {
        int indexLibero = -1;
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (lettereInserite[i] == '\0')
            {
                indexLibero = i;
                break;
            }
        }

        if (indexLibero == -1) return;

        lettereInserite[indexLibero] = lettera;
        slotImages[indexLibero].sprite = sprite;

        slotImages[indexLibero].GetComponent<Button>().onClick.RemoveAllListeners();
        int capturedIndex = indexLibero;

        slotImages[capturedIndex].GetComponent<Button>().onClick.AddListener(() =>
        {
            RimuoviLettera(capturedIndex);
        });

        letterObj.SetActive(false);
    }

    void RimuoviLettera(int slotIndex)
    {
        char lettera = lettereInserite[slotIndex];

        GameObject letteraTrovata = lettereAttive.Find(obj =>
        {
            var img = obj.GetComponent<Image>();
            return img != null && img.sprite == GetSprite(lettera) && !obj.activeSelf;
        });

        if (letteraTrovata != null)
            letteraTrovata.SetActive(true);

        slotImages[slotIndex].sprite = emptySlotSprite;
        slotImages[slotIndex].GetComponent<Button>().onClick.RemoveAllListeners();
        lettereInserite[slotIndex] = '\0';
    }

    public void VerificaParola()
    {
        // Impedisce verifiche multiple rapide
        verificaButton.interactable = false;

        foreach (char c in lettereInserite)
        {
            if (c == '\0')
            {
                verificaButton.interactable = true; // Riabilita se incompleta
                return;
            }
        }

        string parola = new string(lettereInserite);

        if (parola == parolaCorrente)
        {
            paroleIndovinate++;
            AggiornaContatore();
            if (paroleIndovinate >= 5)
                {
                    foreach (GameObject lettera in lettereAttive)
                    {
                        lettera.SetActive(false);
                    }

                    livelloCompletatoPanel.SetActive(true);
                    StartCoroutine(CaricaProssimoLivello());
                }
            else
            {
                StartCoroutine(ProssimaParola());
            }
        }
        else
        {
            // Parola errata → riabilita il pulsante
            verificaButton.interactable = true;
            Debug.Log("Parola sbagliata!");
        }
    }

    IEnumerator ProssimaParola()
    {
        yield return new WaitForSeconds(2f);
        livelloCompletatoPanel.SetActive(false);
        ResetLivello();
        verificaButton.interactable = true; // ✅ Riabilita il pulsante per la prossima parola
    }
    

    IEnumerator CaricaProssimoLivello()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nomeScenaSuccessiva);
    }

    void AggiornaContatore()
    {
        if (counterText != null)
            counterText.text = paroleIndovinate + " / 5";
    }

    List<char> Mischia(List<char> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            char temp = lista[i];
            int randomIndex = Random.Range(i, lista.Count);
            lista[i] = lista[randomIndex];
            lista[randomIndex] = temp;
        }
        return lista;
    }

    Sprite GetSprite(char lettera)
    {
        lettera = char.ToUpper(lettera);
        return alphabetSprites[lettera - 'A'];
    }
}
