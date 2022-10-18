using System.Collections;
using System.Collections.Generic;
using ActionFigures;
using Gameplay.Controllers;
using Mirror;
using UnityEngine;

namespace Gameplay.Figures
{
    public class NetworkPlayerFigureManager : NetworkBehaviour, IPlayerPawn
    {
        public bool IsTurnEnded { get; set; }
        [field: SyncVar] public int ControllerIndex { get; set; }
        public IPlayerFigureRoute PlayerFigureRoute { get; set; }
        public IPawnController PawnController { get; set; }
        public FigureController FigureController { get; set; }

        public Material enemyMaterial;
        public bool isLocalFigure;

        private void Start()
        {
            PlayerFigureRoute = GetComponent<IPlayerFigureRoute>();
            FigureController = GetComponent<FigureController>();
            IsTurnEnded = true;
        }

        [ClientRpc]
        public void ControllerIndexChange(int newValue)
        {
            StartCoroutine(ControllerIndexChangeIEnumerator(newValue));
        }

        private IEnumerator ControllerIndexChangeIEnumerator(int index)
        {
            yield return new WaitUntil(() => NetworkGameController.Instance.Controllers.Count  > index);
            isLocalFigure = NetworkGameController.Instance.Controllers[index].isLocalPlayer;
            if(!isLocalFigure) SetEnemyColor();
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
            StartCoroutine(WaitForEnd());
        } 
        
        [Command(requiresAuthority = false)]
        private void ReleaseSelectingCommand(Vector3 releaseRetractionForce)
        {
            FigureController.Move(releaseRetractionForce, FigureController.pushForce);
        }

        public virtual IEnumerator WaitForEnd()
        {
            yield return new WaitUntil(() => FigureController.isMove);
            IsTurnEnded = true;
        }
        
        [ServerCallback]
        protected virtual void OnDestroy()
        {
            IsTurnEnded = true;
            NetworkServer.Destroy(gameObject);
        }

        private void SetEnemyColor()
        {
            GetComponent<MeshRenderer>().materials = new[] {enemyMaterial};
        }
    }
}