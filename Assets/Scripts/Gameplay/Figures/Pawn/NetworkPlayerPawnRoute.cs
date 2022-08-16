using System.Collections;
using ActionFigures;
using ActionFigures.Pawn;
using Gameplay.Controllers;
using Mirror;
using UnityEngine;

namespace Figures.Pawn
{
    public class NetworkPlayerPawnRoute : NetworkBehaviour,  IPlayerFigureRoute
    {
        private NetworkPlayerController _networkPlayerController;
        private PawnRouteController _pawnRouteController;

        private void Awake()
        {
            _pawnRouteController = GetComponent<PawnRouteController>();
            _networkPlayerController = FindObjectOfType<NetworkPlayerController>();
        }

        public override void OnStartServer()
        {
            Spawn(); 
            ChangePawnRouteActive(false);
        }

        [Command (requiresAuthority = false)]
        public virtual void Select(Vector3 startClickPosition)
        {
            _pawnRouteController.Select(_networkPlayerController.activePawnRoute, startClickPosition);
             ChangePawnRouteActive(false);
        }

        private void Spawn()
        {
            if( _networkPlayerController.activePawnRoute != null) return;
            var newRoute = _pawnRouteController.Spawn();
            NetworkServer.Spawn(newRoute);
            _networkPlayerController.activePawnRoute = newRoute;
        }

        [Command (requiresAuthority = false)]
        public void UpdateData(Vector3 retractionForce)
        {
            _pawnRouteController.UpdateData(_networkPlayerController.activePawnRoute, retractionForce, ChangePawnRouteActive);
        }

        [Command (requiresAuthority = false)]
        public void ReleaseSelecting(Vector3 releaseRetractionForce)
        {
            _pawnRouteController.ReleaseSelecting(_networkPlayerController.activePawnRoute, releaseRetractionForce);
            ChangePawnRouteActive(false);
        }

        [ClientRpc]
        private void ChangePawnRouteActive(bool state)
        {
            if (_networkPlayerController.activePawnRoute == null)
            {
                StartCoroutine(WaitSpawnPawnRoute(state));
                return;
            }
            if(_networkPlayerController.activePawnRoute.activeSelf == state) return;
            _networkPlayerController.activePawnRoute.SetActive(state);
        }

        private IEnumerator WaitSpawnPawnRoute(bool state)
        {
            yield return new WaitUntil(() => _networkPlayerController.activePawnRoute != null);
            _networkPlayerController.activePawnRoute.SetActive(state);
        }
    }
}