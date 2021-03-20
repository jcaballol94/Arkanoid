using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class Brick : MonoBehaviour
    {
        public event System.Action onDie;

        [SerializeField] private int m_life = 1;
        private int m_remainingLife;

        public void Spawn()
        {
            gameObject.SetActive(true);
            m_remainingLife = m_life;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (--m_remainingLife == 0)
            {
                gameObject.SetActive(false);
                onDie?.Invoke();
            }
        }
    }
}