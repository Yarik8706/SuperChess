using System;
using ActionFigures.Pawn;
using UnityEngine;

namespace ActionFigures
{
    public class PlayerPawnRoute : MonoBehaviour, IPlayerFigureRoute
    {
        private static GameObject _activeAimLine;
        private PawnRouteController _pawnRouteController;

        private void Start()
        {
            _pawnRouteController = GetComponent<PawnRouteController>();
        }

        public void Select(Vector3 startClickPosition)
        {
            if (_activeAimLine == null) _activeAimLine = _pawnRouteController.Spawn();
            _activeAimLine.SetActive(false);
        }

        public void UpdateData(Vector3 retractionForce)
        {
            _pawnRouteController.UpdateData(_activeAimLine, retractionForce,  _activeAimLine.SetActive);
        }

        public void ReleaseSelecting(Vector3 releaseRetractionForce)
        {
            _activeAimLine.SetActive(false);
        }
    }
}