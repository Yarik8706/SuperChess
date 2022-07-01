using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text yourTurnText;
    public GameObject aimLine;
    
    private GameController _gameController;
    private GameObject _selectionPawn;
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
            if (!_gameController.playerPawns.Contains(raycastHit.rigidbody.gameObject)) return;
            _selectionPawn = raycastHit.rigidbody.gameObject;
            _clickPosition = Input.mousePosition;
        }
        else
        {
            aimLine.SetActive(true);
            var vector = _clickPosition - Input.mousePosition;
            var launchPosition = Vector3.ClampMagnitude(new Vector3(
                    vector.x / Screen.width, 0, vector.y / Screen.height
                    ) * _gameController.retractionForce, 1);
            aimLine.transform.forward = launchPosition.normalized;
            aimLine.transform.localScale = new Vector3(1.4f, 1, launchPosition.magnitude*4);
            aimLine.transform.position = _selectionPawn.transform.position;
            if (!Input.GetMouseButtonUp(0)) return;
            aimLine.SetActive(false);
            var selectionPawnRigidbody = _selectionPawn.GetComponent<Rigidbody>();
            selectionPawnRigidbody.AddForce(launchPosition * _gameController.pushForce, ForceMode.VelocityChange);
            _selectionPawn = null;
            _isActive = false;
            StartCoroutine(TurnOver(selectionPawnRigidbody));
        }
    }

    private IEnumerator TurnOver(Rigidbody selectionPawn)
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => selectionPawn.velocity.magnitude < 0.000001);
        yourTurnText.enabled = false;
        _gameController.playerTurn = false;
        _isActive = true;
    }
}
