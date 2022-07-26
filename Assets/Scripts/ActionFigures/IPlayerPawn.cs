using System.Collections;
using UnityEngine;

namespace ActionFigures
{
    public interface IPlayerPawn
    {
        public void Select(Vector3 startClickPosition);
        public void UpdateData(Vector3 retractionForce);
        public void ReleaseSelecting(Vector3 releaseRetractionForce);
    }
}