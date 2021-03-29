using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class ChangeRebound : MonoBehaviour
    {
        [SerializeField] [Range(0f, 1f)] private float m_effect = 0.5f;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.rigidbody) return;

            var difference = collision.gameObject.transform.position.x - transform.position.x;
            difference /= transform.localScale.x;

            // Combine the speed of the other object
            collision.rigidbody.velocity += collision.rigidbody.velocity.magnitude * Vector2.right * difference * m_effect;
        }
    }
}