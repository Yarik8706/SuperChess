using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionFigures;
using Gameplay.Figures;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Controllers
{
    public class EnemyController : MonoBehaviour, IPawnController
    {
        public IList<GameObject> Pawns { get; set; }
        public bool IsActive { get; set; }
     
        private IEnemyPawn _selectionPawn;

        private void Awake()
        {
            IsActive = true;
            Pawns = new List<GameObject>();
        }

        private void Start()
        {
            GameController.Instance.Controllers.Add(this);
        }

        public void AddPawn(GameObject pawn)
        {
            Pawns.Add(pawn);
        }

        public void RemovePawn(GameObject pawn)
        {
            Pawns.Remove(pawn);
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
            var random = new System.Random();
            while (selectionPawn == null && Pawns.Count != 0)
            {
                var currentPawn = Pawns.Count == 1 ? Pawns[0] : Pawns[random.Next(0, Pawns.Count)];
                var pawn = currentPawn.GetComponent<IEnemyPawn>();
                if (pawn.FigureController.IsAvailabled())
                {
                    selectionPawn = pawn;
                }
                else
                {
                    Pawns.Remove(currentPawn);
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
    
        public static Vector3 GetNearestPlayerPiece(IPawnController pawnController, Vector3 currentObjectPosition)
        {
            var otherPawns = GameController.Instance.GetControllersWithoutCurrentController(pawnController)[0].Pawns;
            var pawnDistances = otherPawns.Select(
                playerPawn => 
                    Vector2.Distance(
                        new Vector2(playerPawn.transform.position.x, playerPawn.transform.position.z), 
                        new Vector2(currentObjectPosition.x, currentObjectPosition.z))
            ).ToList();
            return otherPawns[pawnDistances.IndexOf(pawnDistances.Min())].transform.position;
        }
    
        public static Vector3 GetNearestPlayerPiece(IPawnController pawnController, Vector3 currentObjectPosition, out float distance)
        {
            var otherPawns = GameController.Instance.GetControllersWithoutCurrentController(pawnController)[0].Pawns;
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
