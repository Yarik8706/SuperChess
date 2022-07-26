using System.Collections;
using UnityEngine;

namespace ActionFigures.Pawn
{
    public class HorseFigure : Figure
    {
        [SerializeField] protected float pushForce = 35;

        protected void Move(Vector3 releaseRetractionForce, float force)
        {
            Rigidbody.AddForce((releaseRetractionForce + transform.up) * force, ForceMode.VelocityChange);
        }

        protected IEnumerator WaitForEnd()
        {
            yield return null;
            while (Rigidbody.velocity.magnitude != 0)
            {
                yield return null;
            }
            isTurnEnded = true;
        }
    }
}