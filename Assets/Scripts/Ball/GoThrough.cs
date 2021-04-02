using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GoThrough : DestroyObjectHandler
    {
        [SerializeField] [HideInInspector] private Rigidbody2D m_rigidbody;

        public bool Active { get; set; }

        private Vector2 m_lastVelocity;

        private void OnValidate()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            m_lastVelocity = m_rigidbody.velocity;
        }

        public override void OnObjectDestroyed(Destroyable ar_destroyedObject)
        {
            // if active, restore the velocity after the collision
            if (Active)
            {
                m_rigidbody.velocity = m_lastVelocity;
            }
        }
    }
}