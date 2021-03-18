using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float m_speed = 10f;
        [SerializeField] [Range(0f, 1f)] private float m_stickiness = 0.5f;
        [SerializeField] [Range(0.001f, 1f)] private float m_stickinessInertia = 0.5f;

        private Rigidbody2D m_rigidbody;
        private float m_target;
        private bool m_move = false;
        private Vector2 m_averageVelocity;
        private bool m_active;

        public bool IsCentered => Mathf.Approximately(0f, transform.position.x);

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_target = transform.position.x;
            m_averageVelocity = Vector3.zero;

            m_active = false;
        }

        private void Update()
        {
            if (!m_active) return;

            if (Input.GetMouseButton(0))
            {
                var camera = Camera.main;
                var ray = camera.ScreenPointToRay(Input.mousePosition);
                var gamePlane = new Plane(Vector3.back, Vector3.zero);

                if (gamePlane.Raycast(ray, out var dist))
                {
                    var clickedPos = ray.GetPoint(dist);
                    m_target = clickedPos.x;
                    m_move = true;
                    return;
                }
            }
            m_move = false;
        }

        private void FixedUpdate()
        {
            var velocity = 0f;

            if (m_move)
            {
                var dist = m_target - transform.position.x;
                velocity = Mathf.Clamp(dist / Time.deltaTime, -m_speed, m_speed);
            }

            m_rigidbody.velocity = Vector2.right * velocity;
            m_averageVelocity = Vector2.Lerp(m_averageVelocity, m_rigidbody.velocity, Time.deltaTime / m_stickinessInertia);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.rigidbody) return;

            // Combine the speed of the other object
            collision.rigidbody.velocity += m_averageVelocity * m_stickiness;
        }

        public void Recenter()
        {
            // Remove control from the player
            m_active = false;

            // Move to the center
            m_target = 0f;
            m_move = true;
        }

        public void Release()
        {
            // Return control to the player
            m_active = true;
        }
    }
}