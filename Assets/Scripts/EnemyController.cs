using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionFigures;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour, IBaseController
{
    public Text robotMoveText;
    public Text robotThinkText;
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

    public void GameOver()
    {
        
    }

    public void Active()
    {
        if (Pawns.Count == 0)
        {
            GameController.Instance.isGameOver = true;
            return;
        }
        IsActive = false;
        robotThinkText.enabled = true;
        Invoke(nameof(MoveRandomEnemy), Random.Range(1, 3));
    }

    public IEnumerator TurnOver()
    {
        robotThinkText.enabled = false;
        ((IEnemyPawn)SelectionPawn).Active();
        robotMoveText.enabled = true;
        yield return new WaitUntil(() => SelectionPawn.IsTurnEnded);
        robotMoveText.enabled = false;
        IsActive = true;
    }

    private void MoveRandomEnemy()
    {
        Figure selectionPawn = null;
        while (selectionPawn == null && Pawns.Count != 0)
        {
            var pawn = Pawns.Count == 1 ? Pawns[0] : Pawns[Random.Range(0, Pawns.Count)];
            if (((IEnemyPawn)pawn).Availabled())
            {
                selectionPawn = pawn;
            }
        }
        
        if (selectionPawn == null)
        {
            GameController.Instance.isGameOver = true;
            IsActive = true;
            return;
        }

        SelectionPawn = selectionPawn;
        StartCoroutine(TurnOver());
    }
    
    public Vector3 GetNearestPlayerPiece(Vector3 currentObjectPosition)
    {
        var otherPawns = GameController.Instance.GetControllersWithoutCurrentController(this)[0].Pawns;
        var pawnDistances = otherPawns.Select(
            playerPawn => 
                Vector2.Distance(
                    new Vector2(playerPawn.transform.position.x, playerPawn.transform.position.z), 
                    new Vector2(currentObjectPosition.x, currentObjectPosition.z))
            ).ToList();
        return otherPawns[pawnDistances.IndexOf(pawnDistances.Min())].transform.position;
    }
    
    public Vector3 GetNearestPlayerPiece(Vector3 currentObjectPosition, out float distance)
    {
        var otherPawns = GameController.Instance.GetControllersWithoutCurrentController(this)[0].Pawns;
        var pawnDistances = otherPawns.Select(
            playerPawn => 
                Vector2.Distance(
                    new Vector2(playerPawn.transform.position.x, playerPawn.transform.position.z), 
                    new Vector2(currentObjectPosition.x, currentObjectPosition.z))
        ).ToList();
        distance = pawnDistances.Min();
        return otherPawns[pawnDistances.IndexOf(distance)].transform.position;
    }
}
