using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class BrickManager : MonoBehaviour
    {
        public event System.Action onLevelCleared;
        public event System.Action<Vector3> onBrickDestroyed;

        [SerializeField] private Brick[] m_bricks;

        private int m_bricksRemaining;

        public void StartLevel()
        {
            m_bricksRemaining = 0;

            for (int i = 0; i < m_bricks.Length; ++i)
            {
                ++m_bricksRemaining;
                m_bricks[i].Spawn();

                // Ensure that we don't register twice
                m_bricks[i].onDie -= OnBrickDestroyed;
                m_bricks[i].onDie += OnBrickDestroyed;
            }
        }

        private void OnBrickDestroyed(Vector3 position)
        {
            if (--m_bricksRemaining <= 0)
            {
                onLevelCleared?.Invoke();
            }
            else
            {
                onBrickDestroyed?.Invoke(position);
            }
        }
    }
}