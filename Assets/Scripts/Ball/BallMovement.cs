using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallMovement : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private Rigidbody2D m_rigidbody;
        [SerializeField] private float m_minVerticalSpeed = 0.1f;

        public float Speed { get; set; }
        public Vector2 Direction 
        { 
            get => m_rigidbody.velocity.normalized; 
            private set => m_rigidbody.velocity = value; 
        }

        private bool m_move;

        private void OnValidate()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();

            m_minVerticalSpeed = Mathf.Max(0f, m_minVerticalSpeed);
        }

        public void Kickoff (Vector2 a_direction)
        {
            m_rigidbody.isKinematic = false;
            Direction = a_direction;
            m_move = true;
        }

        public void Stop ()
        {
            m_rigidbody.isKinematic = true;
            m_move = false;
        }

        private void FixedUpdate()
        {
            if (!m_move) return;

            var vel = Direction * Speed;
            if (Mathf.Abs(vel.y) < m_minVerticalSpeed)
            {
                vel.y = -m_minVerticalSpeed;
            }

            m_rigidbody.velocity = vel;
        }
    }
}