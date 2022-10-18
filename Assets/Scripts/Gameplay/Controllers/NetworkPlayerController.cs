using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionFigures;
using Gameplay.Figures;
using Mirror;
using UI;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class NetworkPlayerController : NetworkBehaviour, IPawnController
    {
        public float retractionForce = 4;

        private IPlayerPawn _selectionPawn;
        private Vector3 _clickPosition;
        [HideInInspector] [SyncVar] public GameObject activePawnRoute;

        public IList<GameObject> Pawns { get; set; } = new SyncList<GameObject>();
        [field: SyncVar] public bool IsActive { get; set; }
        [SyncVar] public int controllerIndex;

        public override void OnStartServer()
        {
            IsActive = true;
            NetworkGameController.Instance.Controllers.Add(this);
            controllerIndex = NetworkGameController.Instance.Controllers.Count - 1;
            Initialize();
        }

        [ClientRpc]
        private void Initialize()
        {
            IsActive = true;
            NetworkGameController.Instance.Controllers.Add(this);
            controllerIndex = NetworkGameController.Instance.Controllers.Count - 1;
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
                pawnScript.ControllerIndex = controllerIndex;
                pawnScript.ControllerIndexChange(controllerIndex);
                pawnScript.PawnController = this;
            }
        }

        private void Update()
        {
            if (IsActive || !isLocalPlayer || NetworkGameController.Instance.isGameOver) return;
            if (Pawns.Count == 0)
            {
                NetworkGameController.Instance.isGameOver = true;
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
                if (playerPawn.ControllerIndex != controllerIndex) return;
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

        public void AddPawn(GameObject pawn)
        {
            Pawns.Add(pawn);
        }

        public void RemovePawn(GameObject pawn)
        {
            Pawns.Remove(pawn);
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
                NetworkGameController.Instance.isGameOver = true;
                return;
            }
            IsActive = false;
        }

        [Command]
        private void TurnOver()
        {
            NetworkGameController.Instance.isWait = false;
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
}