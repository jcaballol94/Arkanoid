using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    [RequireComponent(typeof(Collider2D))]
    public class Destroyable : MonoBehaviour
    {
        public event System.Action onDestroyed;

        [SerializeField] private int m_lives = 1;
        [SerializeField] private string m_killedByTag = "Kill";

        private int m_livesRemaining;

        private void OnValidate()
        {
            m_lives = Mathf.Max(1, m_lives);
        }

        public void Spawn()
        {
            m_livesRemaining = m_lives;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(m_killedByTag))
            {
                --m_livesRemaining;
                if (m_livesRemaining == 0)
                {
                    if (collision.gameObject.TryGetComponent<DestroyObjectHandler>(out var handler))
                    {
                        handler.OnObjectDestroyed(this);
                    }

                    onDestroyed?.Invoke();
                }
            }
        }
    }
}