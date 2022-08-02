using System.Collections;
using System.Collections.Generic;
using ActionFigures;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IBaseController
{
    public Text yourTurnText;
    public float retractionForce = 4;
    private Vector3 _clickPosition;
    public List<Figure> Pawns { get; set; }
    public Figure SelectionPawn { get; set; }
    public bool IsActive { get; set; }

    private void Awake()
    {
        IsActive = true;
        Pawns = new List<Figure>();
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
        yourTurnText.enabled = true;
        if (SelectionPawn == null)
        {
            if(!Input.GetMouseButton(0)) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, 5000, LayerMask.GetMask("Pawn"))) return;
            if (!raycastHit.rigidbody.gameObject.TryGetComponent<IPlayerPawn>(out var playerPawn)) return;
            SelectionPawn = raycastHit.rigidbody.gameObject.GetComponent<Figure>();
            _clickPosition = Input.mousePosition;
            playerPawn.Select(_clickPosition);
        }
        else
        {
            if (!(SelectionPawn is IPlayerPawn playerPawn)) return;
            var vector = _clickPosition - Input.mousePosition;
            var vector3 = Vector3.ClampMagnitude(new Vector3(
                vector.x / Screen.width, 0, vector.y / Screen.height
            ) * retractionForce, 1);
            playerPawn.UpdateData(vector3);
            if (!Input.GetMouseButtonUp(0)) return;
            Debug.Log(vector3.magnitude);
            playerPawn.ReleaseSelecting(vector3);
            StartCoroutine(TurnOver());
        }
    }

    public void GameOver()
    {
        if (Pawns.Count == 0)
        {
            GameController.Instance.Lose();
        }
        else
        {
            GameController.Instance.Win();
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
    
    public IEnumerator TurnOver()
    {
        var pawn = SelectionPawn;
        SelectionPawn = null;
        yield return new WaitUntil(() => pawn.IsTurnEnded);
        yourTurnText.enabled = false;
        IsActive = true;
    }
}
