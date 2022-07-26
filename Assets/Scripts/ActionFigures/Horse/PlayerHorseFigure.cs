using UnityEngine;

namespace ActionFigures.Pawn
{
    public class PlayerHorseFigure : HorseFigure, IPlayerPawn
    {

        public GameController GameController { get; set; }

        public void Select(Vector3 startClickPosition)
        {
           
        }

        public void UpdateData(Vector3 retractionForce)
        {
            
        }

        public void ReleaseSelecting(Vector3 releaseRetractionForce)
        {
            Move(releaseRetractionForce.normalized, pushForce);
            StartCoroutine(WaitForEnd());
        }

        protected override void Died()
        {
            gameController.playerPawns.Remove(this);
            base.Died();
        }
    }
}