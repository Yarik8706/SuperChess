using UnityEngine;

namespace ActionFigures.Pawn
{
    public class PawnController : FigureController
    {
        public override Vector3 GetMovingForce(Vector3 releaseRetractionForce, float force)
        {
            return releaseRetractionForce * force;
        }
    }
}