using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class ParoleCasualiManager : MonoBehaviour
{
    [Header("UI")]
    public Image[] slotImages; // slot1, slot2, slot3, slot4
    public Button verificaButton;
    public Button menuButton;
    public GameObject livelloCompletatoPanel;

    [Header("Lettere")]
    public GameObject letterButtonPrefab;
    public Transform[] spawnPositions; // Pos1 - Pos4
    public Sprite[] alphabetSprites;
    public Sprite emptySlotSprite;

    private string parolaPrecedente = "";

    private string[] parole = { "SOLE", "LUNA", "CANE", "VELA", "LAGO","ALPI","ARCA","ARPA","AURA","ARMA","BICI","BUCA",
                                "GODO","GATE","GASA","GELA","FUMO","GEMO","IDRA","IDEM","IDEA","FETI"};
    private string parolaCorrente;
    private List<GameObject> lettereAttive = new List<GameObject>();
    private char[] lettereInserite;
    
    private int currentSlotIndex = 0;
    private int paroleIndovinate = 0;

    public TMPro.TextMeshProUGUI counterText;

    public string nomeScenaSuccessiva = "ParoleCasuali_5Lettere"; // cambia qui



    void Start()
    {
        livelloCompletatoPanel.SetActive(false);
        lettereInserite = new char[slotImages.Length];
        parolaCorrente = "";
        paroleIndovinate = 0;
        AggiornaContatore();
        ResetLivello();
    }

    void GeneraLettere()
    {
        //  Estrai lettere della parola corrente
        List<char> lettereFinali = new List<char>(parolaCorrente.ToCharArray());

        // Mescola l’ordine delle lettere
        lettereFinali = Mischia(lettereFinali);

        // Genera oggetti lettera
        for (int i = 0; i < lettereFinali.Count; i++)
        {
            GameObject letterObj = Instantiate(letterButtonPrefab, spawnPositions[i].position, Quaternion.identity, spawnPositions[i]);
            letterObj.GetComponentInChildren<Image>().sprite = GetSprite(lettereFinali[i]);
            LetteraUI script = letterObj.GetComponent<LetteraUI>();
            script.Setup(lettereFinali[i], this, letterObj.transform.position);
            lettereAttive.Add(letterObj);
        }
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


    void AggiornaContatore()
    {
        if (counterText != null)
            counterText.text = paroleIndovinate + " / 5";
    }


    public void InserisciLettera(char lettera, Sprite sprite, GameObject letterObj)
    {
        // Trova il primo slot libero disponibile
        int indexLibero = -1;
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (lettereInserite[i] == '\0')
            {
                indexLibero = i;
                break;
            }
        }

        if (indexLibero == -1) return; // Nessuno slot disponibile

        lettereInserite[indexLibero] = lettera;
        slotImages[indexLibero].sprite = sprite;

        // Rimuove eventuali vecchi listener e imposta quello per rimuovere
        slotImages[indexLibero].GetComponent<Button>().onClick.RemoveAllListeners();
        slotImages[indexLibero].GetComponent<Button>().onClick.AddListener(() =>
        {
            RimuoviLettera(indexLibero);
        });

        letterObj.SetActive(false);
    }
    void RimuoviLettera(int slotIndex)
{
    if (slotIndex < 0 || slotIndex >= slotImages.Length) return;

    char lettera = lettereInserite[slotIndex];

    GameObject letteraTrovata = lettereAttive.Find(obj =>
    {
        var img = obj.GetComponent<Image>();
        var spriteLettera = GetSprite(lettera);
        return img != null && img.sprite == spriteLettera && !obj.activeSelf;
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
        verificaButton.interactable = true;
    }



    IEnumerator CaricaProssimoLivello()
    {
        yield return new WaitForSeconds(2f);
        // Qui puoi cambiare scena se vuoi
        SceneManager.LoadScene("ParoleCasuali_5Lettere");
    }

    void ResetLivello()
    {
        currentSlotIndex = 0;
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

        // Scegli una nuova parola diversa dalla precedente
        do
        {
            parolaCorrente = parole[Random.Range(0, parole.Length)];
        } while (parolaCorrente == parolaPrecedente);

        parolaPrecedente = parolaCorrente;

        GeneraLettere();
    }



    Sprite GetSprite(char lettera)
    {
        lettera = char.ToUpper(lettera);
        return alphabetSprites[lettera - 'A'];
    }
}
