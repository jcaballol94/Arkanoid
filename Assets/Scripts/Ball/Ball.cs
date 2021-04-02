using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class Ball : MonoBehaviour
    {
        public event System.Action onKilled;

        [SerializeField] [HideInInspector] private ScaleInOut m_scaleInOut;
        [SerializeField] [HideInInspector] private BallMovement m_movement;
        [SerializeField] [HideInInspector] private GoThrough m_goThrough;
        [SerializeField] [HideInInspector] private Destroyable m_destroyable;

        public float Speed { get => m_movement.Speed; set => m_movement.Speed = value; }
        public bool SuperBall { get => m_goThrough.Active; set => m_goThrough.Active = value; }
        public Vector2 Direction => m_movement.Direction;

        private void OnValidate()
        {
            m_scaleInOut = GetComponentInChildren<ScaleInOut>();
            m_movement = GetComponentInChildren<BallMovement>();
            m_goThrough = GetComponentInChildren<GoThrough>();
            m_destroyable = GetComponentInChildren<Destroyable>();
        }

        private void Awake()
        {
            // Register the killed notification
            m_destroyable.onDestroyed += () =>
            {
                onKilled?.Invoke();
                gameObject.SetActive(false);
            };
        }

        public void KickOff(Vector3 a_direction)
        {
            m_movement.Kickoff(a_direction);
        }

        public void Spawn(Vector3 a_position)
        {
            // Setup the position
            var radius = m_scaleInOut.BaseScale.y;
            transform.position = a_position + radius * Vector3.up;

            // Enable the ball
            gameObject.SetActive(true);

            // Don't move until kickoff
            m_movement.Stop();

            // Become destroyable again
            m_destroyable.Spawn();

            // Put the ball in
            m_scaleInOut.ScaleIn();
        }

        public void InsertImmediate(Vector3 a_position, Vector2 a_direction)
        {
            // Setup the position
            transform.position = a_position;

            // Enable the ball
            gameObject.SetActive(true);

            // Start moving immediately
            m_movement.Kickoff(a_direction);

            // Become destroyable again
            m_destroyable.Spawn();
        }

        public void Despawn()
        {
            StartCoroutine(DespawnRoutine());
        }

        private IEnumerator DespawnRoutine()
        {
            m_scaleInOut.ScaleOut();
            yield return new WaitWhile(() => m_scaleInOut.Fading);
            gameObject.SetActive(false);
        }
    }
}