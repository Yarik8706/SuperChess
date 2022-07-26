using System;
using UnityEngine;

namespace ActionFigures
{
    public class Figure : MonoBehaviour
    {
        public GameController gameController;
        protected Rigidbody Rigidbody;
        public bool isTurnEnded;

        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();        
        }

        private void Update()
        {
            if (transform.position.y < GameController.HeightDeath)
            {
                Died();
                Destroy(gameObject);
            }
        }

        protected virtual void Died()
        {
            
        }
    }
}