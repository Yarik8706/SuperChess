using UnityEngine;

namespace ActionFigures.Pawn
{
    public class PlayerPawnFigure : PawnFigure, IPlayerPawn
    {
        public GameObject aimLine;
        private static GameObject _activeAimLine;

        protected override void Start()
        {
            MyPawns = controllerGameobject.GetComponent<IBaseController>().Pawns;
            base.Start();
        }

        public void Select(Vector3 startClickPosition)
        {
            IsTurnEnded = false;
            if (_activeAimLine != null) return;
            _activeAimLine = Instantiate(aimLine, Vector3.zero, Quaternion.identity);
            _activeAimLine.SetActive(false);
        }

        public void UpdateData(Vector3 retractionForce)
        {
            if (retractionForce.normalized == Vector3.zero)
            {
                _activeAimLine.SetActive(false);
                return;
            }
            _activeAimLine.SetActive(true);
            _activeAimLine.transform.forward = retractionForce.normalized;
            _activeAimLine.transform.position = transform.position + Vector3.up * 0.3f;
            _activeAimLine.transform.localScale = new Vector3(1.3f, 1, retractionForce.magnitude*4);
        }

        public void ReleaseSelecting(Vector3 releaseRetractionForce)
        {
            _activeAimLine.SetActive(false);
            Move(releaseRetractionForce.normalized, pushForce);
            StartCoroutine(WaitForEnd());
        }
    }
}