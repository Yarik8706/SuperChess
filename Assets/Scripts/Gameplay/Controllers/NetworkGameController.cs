using System.Collections;
using Mirror;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class NetworkGameController : NetworkBehaviour
    {
        public GameObject pawnFigure;
        public int requiredConnectionPlayers;
        [SyncVar] public bool isGameOver;
        public bool isWait;
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
            }
            while (!isGameOver)
            {
                foreach (var t in Controllers)
                {
                    Debug.Log("Activity Start --- " + t.name);
                    t.TargetActive();
                    isWait = true;
                    yield return new WaitUntil(() => !isWait);
                    if(isGameOver) break;
                }
            }
        
            foreach (var controller in Controllers)
            {
                controller.TargetGameOver();
            }
        }

        [Command(requiresAuthority = false)]
        public void TurnOvet()
        {
            isWait = false;
        }

        // public List<NetworkGameController> GetControllersWithoutCurrentController(NetworkGameController controller)
        // {
        //     return new List<NetworkGameController>(Controllers.Where(i => i != controller));
        // }
    }
}