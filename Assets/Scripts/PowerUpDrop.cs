using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class PowerUpDrop : MonoBehaviour
    {
        public event System.Action<PowerUp> onPicked;

        public PowerUp PowerUp { get; set; }
        public float Speed { get; set; }

        private Rigidbody2D m_rigidbody;

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_rigidbody.velocity = Vector2.down * Speed;
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.CompareTag("Kill"))
            {
                Destroy(gameObject);
            }
            else if (coll.gameObject.CompareTag("Player"))
            {
                onPicked?.Invoke(PowerUp);
                Destroy(gameObject);
            }
        }
    }
}