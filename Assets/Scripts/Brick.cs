using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class Brick : MonoBehaviour
    {
        public event System.Action<Vector3> onDie;


        public bool IsDestroyable => m_destroyable;
        [SerializeField] private bool m_destroyable = true;
        [SerializeField] private int m_life = 1;
        private int m_remainingLife;

        public void Spawn()
        {
            gameObject.SetActive(true);
            m_remainingLife = m_life;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (--m_remainingLife == 0 && m_destroyable)
            {
                gameObject.SetActive(false);

                var ball = collision.gameObject.GetComponent<Ball>();
                ball.BrickDestroyed();

                // Notify with the position, to spawn power ups
                onDie?.Invoke(transform.position);
            }
        }
    }
}