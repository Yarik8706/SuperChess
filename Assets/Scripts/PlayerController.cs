using System.Collections;
using ActionFigures;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text yourTurnText;
    
    private GameController _gameController;
    private Figure _selectionPawn;
    private Vector3 _clickPosition;
    private bool _isActive = true;

    private void Start()
    {
        _gameController = GetComponent<GameController>();
    }

    private void Update()
    {
        if(!_gameController.playerTurn || !_isActive) return;
        yourTurnText.enabled = true;
        if (_selectionPawn == null)
        {
            if(!Input.GetMouseButton(0)) return;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var raycastHit, 5000, LayerMask.GetMask("Pawn"))) return;
            if (!raycastHit.rigidbody.gameObject.TryGetComponent<IPlayerPawn>(out var playerPawn)) return;
            _selectionPawn = raycastHit.rigidbody.gameObject.GetComponent<Figure>();
            _clickPosition = Input.mousePosition;
            playerPawn.Select(_clickPosition);
        }
        else
        {
            if (!(_selectionPawn is IPlayerPawn playerPawn)) return;
            var vector = _clickPosition - Input.mousePosition;
            var retractionForce = Vector3.ClampMagnitude(new Vector3(
                vector.x / Screen.width, 0, vector.y / Screen.height
            ) * _gameController.retractionForce, 1);
            playerPawn.UpdateData(retractionForce);
            if (!Input.GetMouseButtonUp(0)) return;
            playerPawn.ReleaseSelecting(retractionForce);
            _isActive = false;
            StartCoroutine(TurnOver());
        }
    }

    private IEnumerator TurnOver()
    {
        yield return new WaitUntil(() => _selectionPawn.isTurnEnded);
        yourTurnText.enabled = false;
        _gameController.playerTurn = false;
        _selectionPawn = null;
        _isActive = true;
    }
}
