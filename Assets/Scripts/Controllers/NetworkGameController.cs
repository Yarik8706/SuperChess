using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGameController : NetworkBehaviour
{
    public GameObject pawnFigure;
    public int requiredConnectionPlayers;
    [SyncVar] public bool isGameOver;
    public static NetworkGameController Instance;
    public readonly SyncList<NetworkPlayerController> Controllers = new SyncList<NetworkPlayerController>();

    private void Awake()
    {
        Instance = this;
    }

    public override void OnStartServer()
    {
        StartCoroutine(Playing());
    }

    [ServerCallback]
    private IEnumerator Playing()
    {
        yield return new WaitUntil(() => Controllers.Count >= requiredConnectionPlayers);
        foreach (var controller in Controllers)
        {
            controller.SpawnFigures();
            controller.isGameStart = true;
        }
        Controllers[0].IsActive = false;
        Controllers[0].Active();
        yield return new WaitUntil(() => Controllers[0].IsActive);
        while (!isGameOver)
        {
            foreach (var t in Controllers)
            {
                t.IsActive = false;
                t.TargetActive();
                yield return new WaitUntil(() => t.IsActive);
                if(isGameOver) break;
            }
        }
        
        foreach (var controller in Controllers)
        {
            controller.TargetGameOver();
        }
    }

    // public List<NetworkGameController> GetControllersWithoutCurrentController(NetworkGameController controller)
    // {
    //     return new List<NetworkGameController>(Controllers.Where(i => i != controller));
    // }
}