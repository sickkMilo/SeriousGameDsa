using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Non distruggere quando cambio scena
        }
        else
        {
            Destroy(gameObject); // Evita duplicati se torno al menu
        }
    }
}
