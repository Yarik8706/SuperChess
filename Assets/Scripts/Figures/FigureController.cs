using System;
using System.Collections;
using UnityEngine;

namespace ActionFigures
{
    public class FigureController : MonoBehaviour
    {
        [SerializeField] protected GameObject explosionEffect;
        public float pushForce = 35;
        [HideInInspector] public bool isMove;
        [HideInInspector] public Rigidbody rigidbody3D;

        private void Start()
        {
            rigidbody3D = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (transform.position.y < GameSettings.HeightDeath)
            {
                Died();
            }
        }
        
        protected virtual IEnumerator WaitForEnd()
        {
            yield return null;
            while (rigidbody3D.velocity.magnitude != 0)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.3f);
            isMove = false;
        }

        public virtual void Move(Vector3 releaseRetractionForce, float force)
        {
            isMove = true;
            rigidbody3D.AddForce(GetMovingForce(releaseRetractionForce, force), ForceMode.VelocityChange);
            StartCoroutine(WaitForEnd());
        }
        
        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (!collision.collider.TryGetComponent<ILimb>(out var otherState)) return;
            var thisState = GetComponent<ILimb>();
            if (thisState is IPlayerPawn && otherState is IPlayerPawn || 
                thisState is IEnemyPawn && otherState is IEnemyPawn || isMove)
            {
                Instantiate(explosionEffect, collision.contacts[0].point, Quaternion.identity);
            }
        }

        public virtual Vector3 GetMovingForce(Vector3 releaseRetractionForce, float force)
        {
            return Vector3.zero;
        }

        protected virtual void Died()
        {
            isMove = false;
            Destroy(gameObject);
        }
    }
}