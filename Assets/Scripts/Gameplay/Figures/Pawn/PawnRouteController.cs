using System;
using UnityEngine;

namespace ActionFigures.Pawn
{
    public class PawnRouteController : MonoBehaviour
    {
        public GameObject pawnRoute;
        
        public GameObject Spawn()
        {
            return Instantiate(pawnRoute, Vector3.zero, Quaternion.identity);
        }
        
        public void Select(GameObject route, Vector3 startClickPosition) {}
        
        public void UpdateData(GameObject route, Vector3 retractionForce, Action<bool> changeAimLineState)
        {
            if (retractionForce.normalized == Vector3.zero)
            {
                changeAimLineState.Invoke(false);
                return;
            }
            changeAimLineState.Invoke(true);
            route.transform.forward = retractionForce.normalized;
            route.transform.position = transform.position + Vector3.up * 0.1f;
            route.transform.localScale = new Vector3(1.25f, 1, retractionForce.magnitude*4);
        }

        public void ReleaseSelecting(GameObject route, Vector3 retractionForce) {}
    }
}