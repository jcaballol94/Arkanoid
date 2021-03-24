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
        private Camera m_camera;

        private bool m_move = false;
        private float m_target;
        private float m_touchPoint;
        private float m_initialPosition;

        private Vector2 m_averageVelocity;
        private bool m_active;

        // Size powerup
        private float m_baseSize;
        private float m_powerUpTimer;
        private bool m_powerUpActive;

        public bool IsCentered => Mathf.Approximately(0f, transform.position.x);

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_camera = Camera.main;

            m_target = transform.position.x;
            m_averageVelocity = Vector3.zero;

            m_active = false;

            m_powerUpActive = false;
            m_baseSize = transform.localScale.x;
        }

        private void Update()
        {
            if (m_powerUpActive)
            {
                m_powerUpTimer -= Time.deltaTime;
                if (m_powerUpTimer <= 0f)
                {
                    m_powerUpActive = false;
                    SetLengthBonus(1f);
                }
            }

            // Don't process input if not active
            if (!m_active) return;

            if (Input.GetMouseButtonDown(0))
            {
                // Store the initial position
                m_initialPosition = transform.position.x;

                if (GetMousePosition(out m_touchPoint))
                {
                    m_move = true;
                }
                else
                {
                    m_move = false;
                }
            }
            else if (m_move && Input.GetMouseButton(0))
            {
                if (GetMousePosition(out var newTouch))
                {
                    m_target = m_initialPosition + newTouch - m_touchPoint;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_move = false;
            }
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
            Stop();

            // Move to the center
            m_target = 0f;
            m_move = true;
        }

        public void Stop()
        {
            m_active = false;
            m_move = false;
        }

        public void Release()
        {
            // Return control to the player
            m_active = true;
        }

        private bool GetMousePosition (out float position)
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            var gamePlane = new Plane(Vector3.back, Vector3.zero);

            if (gamePlane.Raycast(ray, out var dist))
            {
                var clickedPos = ray.GetPoint(dist);
                position = clickedPos.x;
                return true;
            }

            position = 0f;
            return false;
        }

        public void ApplyPowerUp(PowerUp powerUp)
        {
            if (powerUp.Type == PowerUp.PowerUpType.PLAYER_LENGTH)
            {
                m_powerUpActive = true;
                m_powerUpTimer = powerUp.Duration;
                SetLengthBonus(powerUp.FloatValue);
            }
        }

        public void CancelPowerUps()
        {
            SetLengthBonus(1f);
            m_powerUpActive = false;
        }

        private void SetLengthBonus(float bonus)
        {
         
            var scale = transform.localScale;
            scale.x = m_baseSize * bonus;
            transform.localScale = scale;
        }
    }
}