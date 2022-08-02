using System.Collections;
using UnityEngine;

namespace ActionFigures.Horse
{
    public class HorseFigure : Figure
    {
        protected void Move(Vector3 releaseRetractionForce, float force)
        {
            Rigidbody.AddForce(GetMovingForce(releaseRetractionForce, force), ForceMode.VelocityChange);
        }

        protected Vector3 GetMovingForce(Vector3 retractionForce, float force)
        {
            return (retractionForce + transform.up * 2.5f) * force ;
        }
    }
}