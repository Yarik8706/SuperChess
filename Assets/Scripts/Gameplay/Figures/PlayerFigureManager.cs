﻿using System.Collections;
using ActionFigures;
using Gameplay.Controllers;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Figures
{
    public class PlayerFigureManager : MonoBehaviour, IPlayerPawn
    {
        public bool IsTurnEnded { get; set; }
        public IPlayerFigureRoute PlayerFigureRoute { get; set; }
        public IPawnController PawnController { get; set; }
        public PlayerPawnsController sPawnController;
        public FigureController FigureController { get; set; }
        public int ControllerIndex { get; set; }

        protected virtual void Start()
        {
            IsTurnEnded = true;
            sPawnController.AddPawn(gameObject);
            PlayerFigureRoute = GetComponent<IPlayerFigureRoute>();
            FigureController = GetComponent<FigureController>();
        }

        public virtual void Select(Vector3 startClickPosition)
        {
            IsTurnEnded = false;
            PlayerFigureRoute.Select(startClickPosition);
        }

        public virtual void UpdateData(Vector3 retractionForce)
        {
            PlayerFigureRoute.UpdateData(retractionForce);
        }

        public virtual void ReleaseSelecting(Vector3 releaseRetractionForce)
        {
            PlayerFigureRoute.ReleaseSelecting(releaseRetractionForce);
            FigureController.Move(releaseRetractionForce, FigureController.pushForce);
            StartCoroutine(WaitForEnd());
        }
        
        public virtual IEnumerator WaitForEnd()
        {
            yield return new WaitUntil(() => FigureController.isMove);
            IsTurnEnded = true;
        }

        protected virtual void OnDestroy()
        {
            IsTurnEnded = true;
            sPawnController.RemovePawn(gameObject);
        }
    }
}