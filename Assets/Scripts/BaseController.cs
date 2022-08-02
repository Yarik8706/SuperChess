using System.Collections;
using System.Collections.Generic;
using ActionFigures;

public interface IBaseController
{
    public List<Figure> Pawns { get; set; }
    public Figure SelectionPawn { get; set; }
    public bool IsActive { get; set; }

    public void GameOver();
    public void Active();
    public IEnumerator TurnOver();
}