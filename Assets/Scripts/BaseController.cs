using System.Collections.Generic;
using UnityEngine;

public interface IPawnController
{
    public IList<GameObject> Pawns { get; set; }
    public bool IsActive { get; set; }

    public void GameOver();
    public void Active();
}