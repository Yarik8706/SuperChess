using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGameController : NetworkBehaviour
{
    public Text yourTurnText;
    public Text winIndexText;
    public Text loseIndexText;
    public GameObject pawnFigure;
    public GameObject winContainerCanvas;
    public GameObject loseContainerCanvas;
    [SyncVar]public bool isGameOver;
    public static NetworkGameController Instance;
    public readonly SyncList<NetworkPlayerController> Controllers = new SyncList<NetworkPlayerController>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            loseIndexText.text = "Поражений: " + PlayerPrefs.GetInt("LoseIndex", 0);
            winIndexText.text = "Побед: " + PlayerPrefs.GetInt("WinIndex", 0);
        }
        if(isServer) StartCoroutine(Playing());
    }

    [ServerCallback]
    private IEnumerator Playing()
    {
        yield return new WaitUntil(() => NetworkServer.connections.Count >= 2);
        Controllers[1].Active();
        yield return new WaitUntil(() => Controllers[1].isActive);
        while (!isGameOver)
        {
            for (int i = 0; i < Controllers.Count; i++)
            {
                Controllers[i].TargetActive(Controllers[i].networkIdentity.connectionToClient);
                yield return new WaitUntil(() => Controllers[i].isActive);
                if(isGameOver) break;
            }
        }

        foreach (var controller in Controllers)
        {
            controller.TargetGameOver(controller.networkIdentity.connectionToClient);
        }
    }

    // public List<NetworkGameController> GetControllersWithoutCurrentController(NetworkGameController controller)
    // {
    //     return new List<NetworkGameController>(Controllers.Where(i => i != controller));
    // }

    public void Lose()
    {
        isGameOver = true;
        PlayerPrefs.SetInt("LoseIndex", PlayerPrefs.GetInt("LoseIndex", 0) + 1);
        loseContainerCanvas.SetActive(true);
    }

    public void Win()
    {
        isGameOver = true;
        PlayerPrefs.SetInt("WinIndex", PlayerPrefs.GetInt("WinIndex", 0) + 1);
        winContainerCanvas.SetActive(true);
    }
}