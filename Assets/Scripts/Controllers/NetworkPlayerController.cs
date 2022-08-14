using System.Collections;
using System.Collections.Generic;
using ActionFigures;
using Figures;
using Mirror;
using UI;
using UnityEngine;

public class NetworkPlayerController : NetworkBehaviour, IPawnController
{
    public float retractionForce = 4;

    private IPlayerPawn _selectionPawn;
    private Vector3 _clickPosition;

    [HideInInspector] [SyncVar] public bool isGameStart;
    [HideInInspector] [SyncVar] public GameObject activePawnRoute;
    
    public IList<GameObject> Pawns { get; set; }
    [field: SyncVar] public bool IsActive { get; set; }
    
    [SyncVar] private int _controllerIndex;

    [ClientCallback]
    private void Awake()
    {
        Pawns = new List<GameObject>();
    }

    public override void OnStartServer()
    {
        IsActive = true;
        NetworkGameController.Instance.Controllers.Add(this);
        _controllerIndex = NetworkGameController.Instance.Controllers.IndexOf(this);
    }

    public override void OnStartLocalPlayer()
    {
        GameSettings.NetworkPlayerController = this;
        GameSettings.PawnController = this;
    }

    [ServerCallback]
    public void SpawnFigures()
    {
        for (int i = 0; i < 4; i ++)
        {
            var pawn = Instantiate(NetworkGameController.Instance.pawnFigure, 
                transform.position + Vector3.right * i * 2,
                Quaternion.identity);
            Pawns.Add(pawn);
            NetworkServer.Spawn(pawn);
            var pawnScript = pawn.GetComponent<NetworkPlayerFigureManager>();
            pawnScript.ControllerIndex = _controllerIndex;
            pawnScript.UpdatePawnState(_controllerIndex);
        }
    }
    
    private void Update()
    {
        if (IsActive || !isLocalPlayer || !isGameStart) return;
        if (Pawns.Count == 0)
        {
            GameController.Instance.isGameOver = true;
            TurnOver();
            return;
        }
        GameUIManager.Instance.yourTurnText.enabled = true;
        if (_selectionPawn == null)
        {
            if (!Input.GetMouseButton(0)) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, 5000, LayerMask.GetMask("Pawn"))) return;
            if (!raycastHit.rigidbody.gameObject.TryGetComponent<IPlayerPawn>(out var playerPawn)) return;
            if (playerPawn.ControllerIndex != _controllerIndex) return;
            _selectionPawn = playerPawn;
            _clickPosition = Input.mousePosition;
            playerPawn.Select(_clickPosition);
        }
        else
        {
            var vector = _clickPosition - Input.mousePosition;
            var vector3 = Vector3.ClampMagnitude(new Vector3(
                vector.x / Screen.width, 0, vector.y / Screen.height
            ) * retractionForce, 1);
            _selectionPawn.UpdateData(vector3);
            if (!Input.GetMouseButtonUp(0)) return;
            _selectionPawn.ReleaseSelecting(vector3);
            StartCoroutine(TurnOverEnumerator());
        }
    }
    
    [TargetRpc]
    public void TargetGameOver()
    {
        GameOver();
    }

    [ClientCallback]
    public void GameOver()
    {
        if (Pawns.Count == 0)
        {
            GameUIManager.Instance.Lose();
        }
        else
        {
            GameUIManager.Instance.Win();
        }
    }

    [TargetRpc]
    public void TargetActive()
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
        IsActive = false;
    }

    [Command]
    private void TurnOver()
    {
        IsActive = true;
    }
    
    [ClientCallback]
    private IEnumerator TurnOverEnumerator()
    {
        var pawn = _selectionPawn;
        _selectionPawn = null;
        yield return new WaitUntil(() => pawn.IsTurnEnded);
        GameUIManager.Instance.yourTurnText.enabled = false;
        TurnOver();
    }
}