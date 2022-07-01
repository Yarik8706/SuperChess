using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public Text robotMoveText;
    public Text robotThinkText;
    
    private GameController _gameController;
    private bool _isActive = true;

    private void Start()
    {
        _gameController = GetComponent<GameController>();
    }

    private void Update()
    {
        if (!_gameController.playerTurn && _isActive && 
            _gameController.enemyPawns.Count != 0 && _gameController.playerPawns.Count != 0)
        {
            StartCoroutine(MoveRandomEnemy());
        }
    }

    private IEnumerator MoveRandomEnemy()
    {
        _isActive = false;
        robotThinkText.enabled = true;
        yield return new WaitForSeconds(Random.Range(1, 3));
        robotThinkText.enabled = false;
        var selectionPawn = _gameController.enemyPawns[Random.Range(0, _gameController.enemyPawns.Count)].transform;
        var pawnDistances = _gameController.playerPawns
            .Select(playerPawn => 
                Vector2.Distance(
                    new Vector2(playerPawn.transform.position.x, playerPawn.transform.position.z), 
                    new Vector2(selectionPawn.position.x, selectionPawn.position.z))
                ).ToList();
        var playerPawnPosition = _gameController.playerPawns[pawnDistances.IndexOf(pawnDistances.Min())].transform.position;
        var force = Vector3.Normalize(playerPawnPosition - selectionPawn.position) * 
                    Random.Range(_gameController.pushForce - _gameController.pushForce / 3, 
                        _gameController.pushForce - _gameController.pushForce / 6);
        var pawnRigidbody = selectionPawn.GetComponent<Rigidbody>();
        pawnRigidbody.AddForce(force, ForceMode.VelocityChange);
        robotMoveText.enabled = true;
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() =>
        {
            if (pawnRigidbody == null) return true;
            Debug.Log(pawnRigidbody.velocity.magnitude);
            return pawnRigidbody.velocity.magnitude < 0.00001;
        });
        _gameController.playerTurn = true;
        robotMoveText.enabled = false;
        _isActive = true;
    }
}
