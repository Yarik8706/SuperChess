using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionFigures;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class EnemyController : MonoBehaviour, IEnemyController
    {
        public IList<GameObject> Pawns { get; set; }
        public bool IsActive { get; set; }
     
        private IEnemyPawn _selectionPawn;

        private void Awake()
        {
            IsActive = true;
            Pawns = new List<GameObject>();
            GameSettings.EnemyController = this;
        }

        private void Start()
        {
            GameController.Instance.Controllers.Add(this);
        }

        public void GameOver()
        {
            Debug.Log("Enemy game over!");
        }

        public void Active()
        {
            if (Pawns.Count == 0)
            {
                GameController.Instance.isGameOver = true;
                return;
            }
            IsActive = false;
            GameUIManager.Instance.robotThinkText.enabled = true;
            Invoke(nameof(MoveRandomEnemy), Random.Range(1, 3));
        }

        private IEnumerator TurnOver()
        {
            GameUIManager.Instance.robotThinkText.enabled = false;
            _selectionPawn.Active();
            GameUIManager.Instance.robotMoveText.enabled = true;
            yield return new WaitUntil(() => _selectionPawn.IsTurnEnded);
            GameUIManager.Instance.robotMoveText.enabled = false;
            IsActive = true;
        }

        private void MoveRandomEnemy()
        {
            IEnemyPawn selectionPawn = null;
            while (selectionPawn == null && Pawns.Count != 0)
            {
                var pawn = (Pawns.Count == 1 ? Pawns[0] : Pawns[Random.Range(0, Pawns.Count)]).GetComponent<IEnemyPawn>();
                if (pawn.Availabled())
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

            _selectionPawn = selectionPawn;
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
}
