using UnityEngine;

namespace ActionFigures.Horse
{
    public class PlayerHorseRoute : MonoBehaviour, IPlayerFigureRoute
    {
        public FigureController figureController;

        public void Select(Vector3 startClickPosition) {}

        public void UpdateData(Vector3 retractionForce)
        {
            TrajectoryRenderer.Instance.ShowTrajectory(transform.position, 
                figureController.GetMovingForce(retractionForce, figureController.pushForce));
        }

        public void ReleaseSelecting(Vector3 releaseRetractionForce)
        {
            TrajectoryRenderer.Instance.ClearTrajectoty();
        }
    }
}