using System.Collections;
using System.Linq;
using ActionFigures;
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
        foreach (var figure in _gameController.enemyPawns)
        {
            if (figure is IEnemyPawn enemyPawn)
            {
                enemyPawn.EnemyController = this;
            }
        }
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
        var selectionPawn = _gameController.enemyPawns[Random.Range(0, _gameController.enemyPawns.Count)];
        if (selectionPawn is IEnemyPawn enemyPawn)
        {
            enemyPawn.Active();
        }
        robotMoveText.enabled = true;
        yield return new WaitUntil(() => selectionPawn.isTurnEnded);
        _gameController.playerTurn = true;
        robotMoveText.enabled = false;
        _isActive = true;
    }

    public Vector3 GetNearestPlayerPiece(Vector3 currentObjectPosition)
    {
        var pawnDistances = _gameController.playerPawns
            .Select(playerPawn => 
                Vector2.Distance(
                    new Vector2(playerPawn.transform.position.x, playerPawn.transform.position.z), 
                    new Vector2(currentObjectPosition.x, currentObjectPosition.z))
            ).ToList();
        return _gameController.playerPawns[pawnDistances.IndexOf(pawnDistances.Min())].transform.position;
    }
}
