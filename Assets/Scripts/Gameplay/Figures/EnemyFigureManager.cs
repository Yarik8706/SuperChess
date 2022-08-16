using System;
using System.Collections;
using Gameplay.Figures;
using UnityEngine;

namespace ActionFigures
{
    public class EnemyFigureManager : MonoBehaviour, IEnemyPawn
    {
        public bool IsTurnEnded { get; set; }
        public FigureController FigureController { get; set; }

        private void Start()
        {
            IsTurnEnded = true;
            GameSettings.EnemyController.Pawns.Add(gameObject);
            FigureController = GetComponent<FigureController>();
        }

        public virtual void Active()
        {
            IsTurnEnded = false;
            var pawn = GameSettings.EnemyController.GetNearestPlayerPiece(transform.position, out var distance);
            var retractionForce = Vector3.Normalize(pawn - transform.position);
            FigureController.Move(retractionForce, 
                Mathf.Clamp(
                    FigureController.pushForce * distance / 5, 
                    FigureController.pushForce - FigureController.pushForce / 2, 
                    FigureController.pushForce - FigureController.pushForce / 10));
            StartCoroutine(WaitForEnd());
        }

        public virtual IEnumerator WaitForEnd()
        {
            yield return new WaitUntil(() => FigureController.isMove);
            IsTurnEnded = true;
        }

        private void OnDestroy()
        {
            IsTurnEnded = true;
            GameSettings.EnemyController.Pawns.Remove(gameObject);
        }
    }
}