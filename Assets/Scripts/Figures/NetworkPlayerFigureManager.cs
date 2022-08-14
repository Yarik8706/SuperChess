using System.Collections;
using ActionFigures;
using Mirror;
using UnityEngine;

namespace Figures
{
    public class NetworkPlayerFigureManager : NetworkBehaviour, IPlayerPawn
    {
        [field: SyncVar] public bool IsTurnEnded { get; set; }
        public IPlayerFigureRoute PlayerFigureRoute { get; set; }
        public FigureController FigureController { get; set; }

        public Material enemyMaterial;

        [field: SyncVar] public int ControllerIndex { get; set; }
        private bool _isLocalFigure;

        private void Start()
        {
            PlayerFigureRoute = GetComponent<IPlayerFigureRoute>();
            FigureController = GetComponent<FigureController>();
            IsTurnEnded = true;
        }

        [ClientRpc]
        public void UpdatePawnState(int newState)
        {
            if (NetworkGameController.Instance.Controllers.Count <= newState)
            {
                NetworkGameController.Instance.Controllers.Callback 
                    += (op, index, item, newItem) =>
                    {
                        if (op != SyncList<NetworkPlayerController>.Operation.OP_ADD) return;
                        UpdateLocalState();
                    };
                return;
            }
            UpdateLocalState();
        }

        private void UpdateLocalState()
        {
            _isLocalFigure = NetworkGameController.Instance.Controllers[ControllerIndex].isLocalPlayer;
            if (!_isLocalFigure)
            {
                SetEnemyMaterial();
                return;
            }
            GameSettings.NetworkPlayerController.Pawns.Add(gameObject);
        }

        public virtual void Select(Vector3 startClickPosition)
        {
            SelectCommand();
            PlayerFigureRoute.Select(startClickPosition);
        }

        [Command(requiresAuthority = false)]
        private void SelectCommand()
        {
            IsTurnEnded = false;
        }

        public virtual void UpdateData(Vector3 retractionForce)
        {
            PlayerFigureRoute.UpdateData(retractionForce);
        }
        
        public virtual void ReleaseSelecting(Vector3 releaseRetractionForce)
        {
            if (releaseRetractionForce == Vector3.zero)
            {
                transform.position += Vector3.up * 10;
                return;
            }
            PlayerFigureRoute.ReleaseSelecting(releaseRetractionForce);
            ReleaseSelectingCommand(releaseRetractionForce);
        } 
        
        [Command(requiresAuthority = false)]
        private void ReleaseSelectingCommand(Vector3 releaseRetractionForce)
        {
            FigureController.Move(releaseRetractionForce, FigureController.pushForce);
            StartCoroutine(WaitForEnd());
        }
        
        public virtual IEnumerator WaitForEnd()
        {
            yield return new WaitUntil(() => FigureController.isMove);
            IsTurnEnded = true;
        }
        
        protected virtual void OnDestroy()
        {
            IsTurnEnded = true;
            if(!_isLocalFigure) return;
            GameSettings.PawnController.Pawns.Remove(gameObject);
            NetworkServer.Destroy(gameObject);
        }

        private void SetEnemyMaterial()
        {
            GetComponent<MeshRenderer>().materials = new[] {enemyMaterial};
        }
    }
}