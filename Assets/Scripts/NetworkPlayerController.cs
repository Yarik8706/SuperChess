using System.Collections;
using ActionFigures;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayerController : NetworkBehaviour
{
    public float retractionForce = 4;

    private Figure _selectionPawn;
    private Vector3 _clickPosition;
    
    [HideInInspector] public NetworkIdentity networkIdentity;
    [HideInInspector] public Vector3 startSpawnPosition;

    public readonly SyncList<GameObject> Pawns = new SyncList<GameObject>();

    [SyncVar] public bool isActive;

    [ServerCallback]
    private void Awake()
    {
        isActive = true;
    }

    public override void OnStartServer()
    {
        for (int i = 0; i < 6; i += 2)
        {
            var pawn = Instantiate(NetworkGameController.Instance.pawnFigure, startSpawnPosition + Vector3.forward * i,
                Quaternion.identity);
            Pawns.Add(pawn);
            NetworkServer.Spawn(pawn);
        }
    }

    private void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();
        if(isServer) NetworkGameController.Instance.Controllers.Add(this);
    }

    private void Update()
    {
        if (isActive && !isLocalPlayer) return;
        if (Pawns.Count == 0)
        {
            GameController.Instance.isGameOver = true;
            isActive = true;
            return;
        }
        NetworkGameController.Instance.yourTurnText.enabled = true;
        if (_selectionPawn == null)
        {
            if(!Input.GetMouseButton(0)) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, 5000, LayerMask.GetMask("Pawn"))) return;
            if (!raycastHit.rigidbody.gameObject.TryGetComponent<IPlayerPawn>(out var playerPawn)) return;
            _selectionPawn = raycastHit.rigidbody.gameObject.GetComponent<Figure>();
            _clickPosition = Input.mousePosition;
            playerPawn.Select(_clickPosition);
        }
        else
        {
            if (!(_selectionPawn is IPlayerPawn playerPawn)) return;
            var vector = _clickPosition - Input.mousePosition;
            var vector3 = Vector3.ClampMagnitude(new Vector3(
                vector.x / Screen.width, 0, vector.y / Screen.height
            ) * retractionForce, 1);
            playerPawn.UpdateData(vector3);
            if (!Input.GetMouseButtonUp(0)) return;
            Debug.Log(vector3.magnitude);
            playerPawn.ReleaseSelecting(vector3);
            StartCoroutine(TurnOver());
        }
    }
    
    [TargetRpc]
    public void TargetGameOver(NetworkConnection _)
    {
        GameOver();
    }
    
    [ClientCallback]
    private void GameOver()
    {
        if (Pawns.Count == 0)
        {
            GameController.Instance.Lose();
        }
        else
        {
            GameController.Instance.Win();
        }
    }

    [TargetRpc]
    public void TargetActive(NetworkConnection _)
    {
        Active();
    }
    
    [ClientCallback]
    public void Active()
    {
        if (Pawns.Count == 0)
        {
            GameController.Instance.isGameOver = true;
            return;
        }
        isActive = false;
    }
    
    [ClientCallback]
    private IEnumerator TurnOver()
    {
        var pawn = _selectionPawn;
        _selectionPawn = null;
        yield return new WaitUntil(() => pawn.IsTurnEnded);
        NetworkGameController.Instance.yourTurnText.enabled = false;
        isActive = true;
    }
}