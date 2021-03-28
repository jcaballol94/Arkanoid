using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class DraggableBehaviour : MonoBehaviour
    {
        public bool Enabled { get; set; }

        private float m_offset;

        private Camera m_camera;
        private Rigidbody2D m_rigidbody;

        private float Pos
        {
            get => transform.position.x;
            set
            {
                var pos = transform.position;
                pos.x = value;
                m_rigidbody.MovePosition(pos);
            }
        }

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
                    m_offset = Pos - mousePos;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (GetMousePosition(out var newPos))
                {
                    Pos = newPos + m_offset;
                }
            }
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