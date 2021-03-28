using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class DraggableBehaviour : MonoBehaviour
    {
        [SerializeField] private float m_maxSpeed = 100f;

        public bool Enabled { get; set; }

        private float m_offset;
        private float m_target;
        private bool m_move;

        private Camera m_camera;
        private Rigidbody2D m_rigidbody;

        private void Awake()
        {
            m_camera = Camera.main;
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!Enabled) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (GetMousePosition(out var mousePos))
                {
                    m_move = true;
                    m_offset = transform.position.x - mousePos;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (GetMousePosition(out var newPos))
                {
                    m_target = newPos + m_offset;
                }
            }
            else
            {
                m_move = false;
            }
        }

        private void FixedUpdate()
        {
            if (!Enabled) return;

            var pos = transform.position;
            var dist = m_target - pos.x;
            if (!m_move || Mathf.Abs(dist) < 0.01f)
            {
                m_rigidbody.velocity = Vector2.zero;
                return;
            }

            m_rigidbody.velocity = Vector2.right * Mathf.Clamp(dist / Time.deltaTime, - m_maxSpeed, m_maxSpeed);
        }

        private bool GetMousePosition(out float ar_position)
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            var gamePlane = new Plane(Vector3.back, Vector3.zero);

            if (gamePlane.Raycast(ray, out var dist))
            {
                var clickedPos = ray.GetPoint(dist);
                ar_position = clickedPos.x;
                return true;
            }

            ar_position = 0f;
            return false;
        }
    }
}