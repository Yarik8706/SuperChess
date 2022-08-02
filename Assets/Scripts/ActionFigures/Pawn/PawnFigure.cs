using System;
using System.Collections;
using UnityEngine;

namespace ActionFigures.Pawn
{
    public class PawnFigure : Figure
    {
        protected void Move(Vector3 releaseRetractionForce, float force)
        {
            Rigidbody.AddForce(releaseRetractionForce * force, ForceMode.VelocityChange);
        }
    }
}