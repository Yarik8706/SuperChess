using UnityEngine;

namespace ActionFigures.Pawn
{
    public class EnemyPawnFigure : PawnFigure, IEnemyPawn
    {
        public EnemyController EnemyController { get; set; }

        protected override void Start()
        {
            EnemyController = controllerGameobject.GetComponent<EnemyController>();
            MyPawns = EnemyController.Pawns;
            base.Start();
        }

        public void Active()
        {
            IsTurnEnded = false;
            var playerPawnPosition = EnemyController.GetNearestPlayerPiece(
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