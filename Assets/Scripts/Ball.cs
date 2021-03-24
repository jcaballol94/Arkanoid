using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class Ball : MonoBehaviour
    {
        public event System.Action onKilled;

        private float Speed => (m_powerUpTimer >= 0f) ? m_speed * m_powerUpAmount : m_speed;
        [SerializeField] private float m_speed = 5f;

        private Rigidbody2D m_rigidbody;
        private bool m_kickedOff;

        private float m_powerUpTimer;
        private float m_powerUpAmount;

        public void KickOff(Vector3 direction)
        {
            m_kickedOff = true;
            m_rigidbody.isKinematic = false;
            m_rigidbody.velocity = direction * m_speed;
            m_powerUpTimer = -1f;
        }

        // Start is called before the first frame update
        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Spawn()
        {
            m_rigidbody.velocity = Vector3.zero;
            m_rigidbody.isKinematic = true;
            m_kickedOff = false;
            gameObject.SetActive(true);
        }

        public void Despawn()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            m_powerUpTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (!m_kickedOff) return;

            // Keep the speed constant
            var velocity = m_rigidbody.velocity.normalized * Speed;
            // Make sure that the ball always moves vertically
            if (Mathf.Abs(velocity.y) < 0.1f) velocity.y = -0.1f;

            m_rigidbody.velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.CompareTag("Kill"))
            {
                Despawn();
                onKilled?.Invoke();
            }
        }

        public void ApplyPowerUp (PowerUp powerUp)
        {
            if (powerUp.Type == PowerUp.PowerUpType.BALL_SPEED)
            {
                m_powerUpTimer = powerUp.Duration;
                m_powerUpAmount = powerUp.FloatValue;
            }
        }
    }
}