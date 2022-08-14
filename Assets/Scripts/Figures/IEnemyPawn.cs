﻿using UnityEngine;

namespace ActionFigures
{
    public interface IEnemyPawn : IPawn
    {
        public void Active();
        public bool Availabled();
    }

    public interface IEnemyController : IPawnController
    {
        public Vector3 GetNearestPlayerPiece(Vector3 currentObjectPosition, out float distance);
    }
}