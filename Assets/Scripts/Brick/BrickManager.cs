using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
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

            foreach (var brick in m_bricks)
            {
                if (brick.IsDestroyable)
                {
                    ++m_bricksRemaining;
                }
                brick.Spawn();

                // Ensure that we don't register twice
                brick.onDie -= OnBrickDestroyed;
                brick.onDie += OnBrickDestroyed;
            }
        }

        public void EndLevel()
        {
            m_bricksRemaining = 0;

            foreach (var brick in m_bricks)
            {
                if (brick.gameObject.activeSelf)
                {
                    brick.Despawn();
                }
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