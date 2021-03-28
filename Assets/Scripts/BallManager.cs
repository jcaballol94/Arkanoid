using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class BallManager : MonoBehaviour
    {
        public event System.Action onPlayerDied;

        [SerializeField] private int m_numBalls;
        [SerializeField] private GameObject m_ballPrefab;

        private List<Ball> m_inactiveBalls = new List<Ball>();
        private List<Ball> m_activeBalls = new List<Ball>();
        
        private void Awake()
        {
            m_inactiveBalls.Capacity = m_numBalls;
            m_activeBalls.Capacity = m_numBalls;

            for (int i = 0; i < m_numBalls; ++i)
            {
                var go = Instantiate(m_ballPrefab);
                go.transform.SetParent(transform);
                go.SetActive(false);
                var ball = go.GetComponent<Ball>();
                ball.onKilled += () => OnBallKilled(ball);
                m_inactiveBalls.Add(ball);
            }
        }

        public void SpawnBall()
        {
            if (m_activeBalls.Count != 0) return;

            var ball = m_inactiveBalls[m_inactiveBalls.Count - 1];
            m_inactiveBalls.Remove(ball);

            m_activeBalls.Add(ball);
            ball.Spawn(transform.position);
        }

        public void DespawnBalls()
        {
            foreach (var ball in m_activeBalls)
            {
                ball.Despawn();
                m_inactiveBalls.Add(ball);
            }

            m_activeBalls.Clear();
        }

        public void KickOff (Vector3 direction)
        {
            if (m_activeBalls.Count == 0) return;
            m_activeBalls[0].KickOff(direction);
        }

        public void OnBallKilled (Ball ball)
        {
            m_activeBalls.Remove(ball);
            m_inactiveBalls.Add(ball);

            if (m_activeBalls.Count == 0)
            {
                onPlayerDied?.Invoke();
            }
        }

        public void ApplyPowerUp(PowerUp powerUp)
        {
            if (powerUp.Type == PowerUp.PowerUpType.MULTI_BALL)
            {
                if (m_activeBalls.Count == 0) return;

                var refBall = m_activeBalls[0];
                foreach (var ball in m_inactiveBalls)
                {
                    ball.Clone(refBall);
                    m_activeBalls.Add(ball);
                }

                m_inactiveBalls.Clear();
            }
            else
            {
                foreach (var ball in m_activeBalls)
                {
                    ball.ApplyPowerUp(powerUp);
                }
            }
        }

        public void CancelPowerUps()
        {
            foreach (var ball in m_activeBalls)
            {
                ball.CancelPowerUps();
            }
        }
    }
}