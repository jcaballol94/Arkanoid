using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class Ball : MonoBehaviour
    {
        public event System.Action onKilled;

        [SerializeField] private float m_speed = 5f;

        private Rigidbody2D m_rigidbody;
        private bool m_kickedOff;

        public void KickOff(Vector3 direction)
        {
            m_kickedOff = true;
            m_rigidbody.isKinematic = false;
            m_rigidbody.velocity = direction * m_speed;
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

        private void FixedUpdate()
        {
            if (!m_kickedOff) return;

            // Keep the speed constant
            var velocity = m_rigidbody.velocity.normalized * m_speed;
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
    }
}