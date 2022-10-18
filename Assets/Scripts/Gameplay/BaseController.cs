using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public interface IPawnController
    {
        public IList<GameObject> Pawns { get; set; }
        public bool IsActive { get; set; }
        public void AddPawn(GameObject pawn);
        public void RemovePawn(GameObject pawn);
        public void GameOver();
        public void Active();
    }
}