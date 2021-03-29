using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CenterableBehaviour : MonoBehaviour
    {
        [SerializeField] private float m_centerSpeed = 10f;
        [SerializeField] private float m_acceleration = 0.2f;

        [SerializeField] [HideInInspector] private Rigidbody2D m_rigidbody;

        private Vector3 m_center;
        private bool m_centering;

        private void OnValidate()
        {
            m_centerSpeed = Mathf.Max(0.1f, m_centerSpeed);
            m_acceleration = Mathf.Max(0.1f, m_acceleration);

            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            m_center = transform.position;
            m_centering = false;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void FixedUpdate()
        {
            if (!m_centering) return;


            // Finish the center
            var distance = Vector3.Distance(m_center, transform.position);
            if (distance < 0.05f)
            {
                m_centering = false;
                m_rigidbody.velocity = Vector2.zero;
                return;
            }

            // Calculate the max speed to stop in time
            var stopTime = Mathf.Sqrt((2f * distance) / m_acceleration);
            var maxSpeed = m_acceleration * stopTime;

            var speed = Mathf.Min(maxSpeed, m_centerSpeed);
            speed = Mathf.MoveTowards(m_rigidbody.velocity.magnitude, speed, m_acceleration * Time.deltaTime);
            m_rigidbody.velocity = (m_center - transform.position).normalized * speed;
        }

        public IEnumerator CenterRoutine()
        {
            m_centering = true;
            yield return new WaitWhile(() => m_centering);
        }
    }
}