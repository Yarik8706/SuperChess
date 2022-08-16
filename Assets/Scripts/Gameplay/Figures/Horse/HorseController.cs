using Gameplay.Figures;
using UnityEngine;

namespace ActionFigures
{
    public class HorseController : FigureController
    {
        public override Vector3 GetMovingForce(Vector3 retractionForce, float force)
        {
            return (retractionForce + transform.up * 2.5f) * force;
        }
    }
}