using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Settings")]
    public bool firendlyFire;

    private void Awake()
    {
        Instance = this;
    }
}