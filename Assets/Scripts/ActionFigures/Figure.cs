﻿using System.Collections;
using UnityEngine;

namespace ActionFigures
{
    public class Figure : MonoBehaviour
    {
        [SerializeField] protected GameObject explosionEffect;
        [SerializeField] protected float pushForce = 35;
        public GameObject controllerGameobject;
        protected IList MyPawns;
        protected Rigidbody Rigidbody;
        internal bool IsTurnEnded = true;

        protected virtual void Start()
        {
            MyPawns.Add(this);
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (transform.position.y < GameSettings.HeightDeath)
            {
                Died();
            }
        }
        
        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (!(collision.collider.GetComponent<Figure>() is { } figure)) return;
            if (!IsTurnEnded)
            {
                Instantiate(explosionEffect, collision.contacts[0].point, Quaternion.identity);
            }
            else if (figure is IPlayerPawn && this is IPlayerPawn || 
                     figure is IEnemyPawn && this is IEnemyPawn && Rigidbody.velocity.magnitude != 0)
            {
                Instantiate(explosionEffect, collision.contacts[0].point, Quaternion.identity);
            }
        }
        
        protected virtual IEnumerator WaitForEnd()
        {
            yield return null;
            while (Rigidbody.velocity.magnitude != 0)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.3f);
            IsTurnEnded = true;
        }

        protected virtual void Died()
        {
            IsTurnEnded = true;
            MyPawns.Remove(this);
            Destroy(gameObject);
        }
    }
}