using UnityEngine;

namespace ActionFigures.Pawn
{
    public class PlayerHorseFigure : HorseFigure, IPlayerPawn
    {
        public GameObject aimLine;
        private static GameObject _activeAimLine;

        public GameController GameController { get; set; }

        public void Select(Vector3 startClickPosition)
        {
            if (_activeAimLine != null) return;
            _activeAimLine = Instantiate(aimLine, Vector3.zero, Quaternion.identity);
            _activeAimLine.SetActive(false);
        }

        public void UpdateData(Vector3 retractionForce)
        {
            _activeAimLine.SetActive(true);
            _activeAimLine.transform.forward = retractionForce.normalized;
            _activeAimLine.transform.localScale = new Vector3(1.4f, 1, retractionForce.magnitude*4);
            _activeAimLine.transform.position = transform.position;
        }

        public void ReleaseSelecting(Vector3 releaseRetractionForce)
        {
            _activeAimLine.SetActive(false);
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