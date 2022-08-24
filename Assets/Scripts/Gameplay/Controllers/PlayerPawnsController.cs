using System.Collections;
using System.Collections.Generic;
using ActionFigures;
using Gameplay.Figures;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPawnsController : MonoBehaviour, IPawnController
{
    public float retractionForce = 4;
    private Vector3 _clickPosition;
    public IList<GameObject> Pawns { get; set; }
    private PlayerFigureManager SelectionPawn { get; set; }
    public bool IsActive { get; set; }

    private void Awake()
    {
        IsActive = true;
        Pawns = new List<GameObject>();
        GameSettings.PawnController = this;
    }

    private void Start()
    {
        GameController.Instance.Controllers.Add(this);
    }

    private void Update()
    {
        if(IsActive) return;
        if (Pawns.Count == 0)
        {
            GameController.Instance.isGameOver = true;
            IsActive = true;
            return;
        }
        GameUIManager.Instance.yourTurnText.enabled = true;
        if (SelectionPawn == null)
        {
            if(!Input.GetMouseButton(0)) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, 5000, LayerMask.GetMask("Pawn"))) return;
            if (!raycastHit.rigidbody.gameObject.TryGetComponent<IPlayerPawn>(out _)) return;
            SelectionPawn = raycastHit.rigidbody.gameObject.GetComponent<PlayerFigureManager>();
            _clickPosition = Input.mousePosition;
            SelectionPawn.Select(_clickPosition);
        }
        else
        {
            var vector = _clickPosition - Input.mousePosition;
            var vector3 = Vector3.ClampMagnitude(new Vector3(
                vector.x / Screen.width, 0, vector.y / Screen.height
            ) * retractionForce, 1);
            SelectionPawn.UpdateData(vector3);
            if (!Input.GetMouseButtonUp(0)) return;
            SelectionPawn.ReleaseSelecting(vector3);
            StartCoroutine(TurnOver());
        }
    }

    public void GameOver()
    {
        if (Pawns.Count == 0)
        {
            GameUIManager.Instance.Lose();
        }
        else
        {
            GameUIManager.Instance.Win();
        }
    }

    public void Active()
    {
        if (Pawns.Count == 0)
        {
            GameController.Instance.isGameOver = true;
            return;
        }
        IsActive = false;
    }

    private IEnumerator TurnOver()
    {
        var pawn = SelectionPawn;
        SelectionPawn = null;
        yield return new WaitUntil(() => pawn.IsTurnEnded);
        GameUIManager.Instance.yourTurnText.enabled = false;
        IsActive = true;
    }
}
