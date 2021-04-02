using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DropBehaviour : MonoBehaviour
    {
        [SerializeField] private float m_speed = 1f;

        [SerializeField] [HideInInspector] private Rigidbody2D m_rigidbody;

        private void OnValidate()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();

            m_speed = Mathf.Max(0.1f, m_speed);
        }

        private void FixedUpdate()
        {
            m_rigidbody.velocity = Vector3.down * m_speed;
        }
    }
}