using System.Collections;
using UnityEngine;

namespace ActionFigures
{
    public interface IPlayerPawn : IPawn 
    {
        public int ControllerIndex { get; set; }
        public IPlayerFigureRoute PlayerFigureRoute { get; set; }
        public void Select(Vector3 startClickPosition);
        public void UpdateData(Vector3 retractionForce);
        public void ReleaseSelecting(Vector3 releaseRetractionForce);
    }

    public interface IPawn : ILimb
    {
        public FigureController FigureController { get; set; }
    }

    public interface IPlayerFigureRoute
    {
        public void Select(Vector3 startClickPosition);
        public void UpdateData(Vector3 retractionForce);
        public void ReleaseSelecting(Vector3 releaseRetractionForce);
    }
    
    public interface ILimb {
        public bool IsTurnEnded { get; set; }
        public IEnumerator WaitForEnd();
    }
}