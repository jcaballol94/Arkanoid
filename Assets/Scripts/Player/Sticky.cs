using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Sticky : MonoBehaviour
    {
        [SerializeField] [Range(0f, 1f)] private float m_stickiness = 0.5f;

        private Rigidbody2D m_rigidbody;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.rigidbody) return;

            // Combine the speed of the other object
            collision.rigidbody.velocity += m_rigidbody.velocity * m_stickiness;
        }
    }
}