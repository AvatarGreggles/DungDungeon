using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;

    // Instance of gamelayers so any script can access it. (Singleton pattern)
    public static GameLayers Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public LayerMask PlayerLayer
    {
        get => playerLayer;
    }

    public LayerMask EnemyLayer
    {
        get => enemyLayer;
    }
}
