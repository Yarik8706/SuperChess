using UnityEngine;

namespace ActionFigures.Pawn
{
    public class EnemyPawnFigure : PawnFigure, IEnemyPawn
    {
        public EnemyController EnemyController { get; set; }

        public void Active()
        {
            isTurnEnded = false;
            var playerPawnPosition = EnemyController.GetNearestPlayerPiece(transform.position);
            var retractionForce = Vector3.Normalize(playerPawnPosition - transform.position);
            Move(retractionForce, Random.Range(pushForce - pushForce / 3, pushForce - pushForce / 6));
            StartCoroutine(WaitForEnd());
        }

        protected override void Died()
        {
            gameController.enemyPawns.Remove(this);
            base.Died();
        }
    }
}