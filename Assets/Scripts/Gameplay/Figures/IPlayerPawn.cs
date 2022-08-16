using System.Collections;
using Gameplay.Figures;
using UnityEngine;

namespace ActionFigures
{
    public interface IPlayerPawn : IPawn, IPlayerInput
    {
        public int ControllerIndex { get; set; }
        public IPlayerFigureRoute PlayerFigureRoute { get; set; }
    }

    public interface IPawn : ILimb
    {
        public FigureController FigureController { get; set; }
    }

    public interface IPlayerInput
    {
        public void Select(Vector3 startClickPosition);
        public void UpdateData(Vector3 retractionForce);
        public void ReleaseSelecting(Vector3 releaseRetractionForce);
    }

    public interface IPlayerFigureRoute : IPlayerInput{}
    
    public interface ILimb {
        public bool IsTurnEnded { get; set; }
        public IEnumerator WaitForEnd();
    }
}