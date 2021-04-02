using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class BallManager : MonoBehaviour
    {
        public event System.Action onPlayerDied;

        [SerializeField] private int m_numBalls = 3;
        [SerializeField] private float m_ballSpeed = 10f;
        [SerializeField] private float m_ballSpeedBlend = 0.2f;
        [SerializeField] private float m_cloneMaxAngle = 10f;
        [SerializeField] private GameObject m_ballPrefab;

        private List<Ball> m_inactiveBalls = new List<Ball>();
        private List<Ball> m_activeBalls = new List<Ball>();

        private float m_currentSpeed;
        private float Speed
        {
            get => m_currentSpeed;
            set
            {
                m_currentSpeed = value;
                foreach (var b in m_activeBalls)
                {
                    b.Speed = m_currentSpeed;
                }
            }
        }

        private bool m_superBall;
        private bool SuperBall
        {
            get => m_superBall;
            set
            {
                m_superBall = value;
                foreach (var b in m_activeBalls)
                {
                    b.SuperBall = m_superBall;
                }
            }
        }
        
        private void Awake()
        {
            m_inactiveBalls.Capacity = m_numBalls;
            m_activeBalls.Capacity = m_numBalls;

            for (int i = 0; i < m_numBalls; ++i)
            {
                var go = Instantiate(m_ballPrefab);
                go.SetActive(false);
                var ball = go.GetComponent<Ball>();
                ball.onKilled += () => OnBallKilled(ball);
                m_inactiveBalls.Add(ball);
            }
        }

        private void OnValidate()
        {
            m_numBalls = Mathf.Max(1, m_numBalls);
            m_ballSpeed = Mathf.Max(0.1f, m_ballSpeed);
            m_ballSpeedBlend = Mathf.Max(0.001f, m_ballSpeedBlend);
            m_cloneMaxAngle = Mathf.Max(1f, m_cloneMaxAngle);
        }

        private void OnDisable()
        {
            CancelPowerUps();
        }

        public void SpawnBall(Vector3 a_position)
        {
            // Only spawn when there were no balls active
            if (m_activeBalls.Count != 0) return;

            // Remove it from the inactive pool
            var ball = m_inactiveBalls[m_inactiveBalls.Count - 1];
            m_inactiveBalls.Remove(ball);

            // Put it in the active list to keep it under control
            m_activeBalls.Add(ball);

            // Start without powerups
            CancelPowerUps();

            // Setup the data
            ball.Speed = Speed;
            ball.SuperBall = SuperBall;

            // Spawn the ball
            ball.Spawn(a_position);
        }

        public void DespawnBalls()
        {
            // Disable all the active balls and return them to the pool
            foreach (var ball in m_activeBalls)
            {
                ball.Despawn();
                m_inactiveBalls.Add(ball);
            }

            // No ball is active
            m_activeBalls.Clear();
        }

        public void KickOff (Vector3 direction)
        {
            if (m_activeBalls.Count == 0) return;
            m_activeBalls[0].KickOff(direction);
        }

        public void OnBallKilled (Ball ball)
        {
            // Put it back to the pool
            m_activeBalls.Remove(ball);
            m_inactiveBalls.Add(ball);

            // If it was the last ball notify the game
            if (m_activeBalls.Count == 0)
            {
                onPlayerDied?.Invoke();
            }
        }

        public void ApplyPowerUp(PowerUp powerUp)
        {
            switch (powerUp.Type)
            {
                case PowerUp.PowerUpType.MULTI_BALL:
                    {
                        if (m_activeBalls.Count == 0) return;

                        // Gather data from the active ball
                        var refBall = m_activeBalls[0];
                        var baseDir = refBall.Direction;
                        var basePos = refBall.transform.position;

                        foreach (var ball in m_inactiveBalls)
                        {
                            // Get a rotation so it isn't exacly equal to the existing one
                            var angle = Random.Range(-m_cloneMaxAngle, m_cloneMaxAngle);
                            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                            // Setup ball data
                            ball.Speed = Speed;
                            ball.SuperBall = SuperBall;

                            // Spawn it (immediately kicked off)
                            ball.InsertImmediate(basePos, rotation * baseDir);
                            m_activeBalls.Add(ball);
                        }

                        // No ball left in the pool
                        m_inactiveBalls.Clear();
                    }
                    break;
                case PowerUp.PowerUpType.BALL_SPEED:
                    StartCoroutine(SpeedRoutine(powerUp.FloatValue, powerUp.Duration));
                    break;
                case PowerUp.PowerUpType.SUPER_BALL:
                    StartCoroutine(SuperBallRoutine(powerUp.Duration));
                    break;
            }
        }

        private IEnumerator SpeedRoutine (float a_bonus, float a_duration)
        {
            StartCoroutine(BlendSpeed(m_ballSpeed * a_bonus));
            yield return new WaitForSeconds(a_duration);
            yield return BlendSpeed(m_ballSpeed);
        }

        private IEnumerator BlendSpeed (float a_target)
        {
            var startSpeed = Speed;
            var t = 0f;

            while (t < m_ballSpeedBlend)
            {
                Speed = Mathf.Lerp(startSpeed, a_target, t / m_ballSpeedBlend);
                yield return null;
                t += Time.deltaTime;
            }
            Speed = a_target;
        }

        private IEnumerator SuperBallRoutine(float a_duration)
        {
            SuperBall = true;
            yield return new WaitForSeconds(a_duration);
            SuperBall = false;
        }

        public void CancelPowerUps()
        {
            StopAllCoroutines();
            SuperBall = false;
            Speed = m_ballSpeed;
        }
    }
}