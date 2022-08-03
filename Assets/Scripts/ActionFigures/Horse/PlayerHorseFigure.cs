using ActionFigures.Pawn;
using UnityEngine;

namespace ActionFigures.Horse
{
    public class PlayerHorseFigure : HorseFigure, IPlayerPawn
    {
        public TrajectoryRenderer trajectoryRenderer;

        public void Select(Vector3 startClickPosition)
        {
           
        }

        public void UpdateData(Vector3 retractionForce)
        {
            trajectoryRenderer.ShowTrajectory(transform.position, GetMovingForce(retractionForce, pushForce), Rigidbody.mass);
        }

        public void ReleaseSelecting(Vector3 releaseRetractionForce)
        {
            Move(releaseRetractionForce.normalized, pushForce);
            StartCoroutine(WaitForEnd());
            trajectoryRenderer.ClearTrajectoty();
        }
    }
}