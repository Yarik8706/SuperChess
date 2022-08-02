using UnityEngine;

namespace ActionFigures.Pawn
{
    public class EnemyPawnFigure : PawnFigure, IEnemyPawn
    {
        public void Active()
        {
            IsTurnEnded = false;
            var playerPawnPosition = ((EnemyController)MyController).GetNearestPlayerPiece(
                transform.position, 
                out var distance);
            var retractionForce = Vector3.Normalize(playerPawnPosition - transform.position);
            Move(retractionForce, Random.Range(pushForce - pushForce / 3, pushForce - pushForce / 6));
            StartCoroutine(WaitForEnd());
        }

        public bool Availabled()
        {
            return Rigidbody.velocity.magnitude == 0;
        }
    }
}