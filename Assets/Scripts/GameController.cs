using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    public class GameController : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private Ball m_ball;
        [SerializeField] private PlayerController m_player;
        [SerializeField] private BrickManager m_bricks;

        [Header("UI")]
        [SerializeField] private UI.KickOffPanel m_kickoffPanel;

        private Vector3 m_initialBallPosition;

        private void Start()
        {
            // Prepare the bricks
            m_bricks.onLevelCleared += OnLevelCleared;

            // Prepare the ball
            m_initialBallPosition = m_ball.transform.position;
            m_ball.onKilled += OnBallKilled;

            // Prepare the kickof panel
            m_kickoffPanel.SetBallPosition(m_initialBallPosition);
            m_kickoffPanel.onKickoffPressed += KickOffPressed;

            // First kickoff
            StartCoroutine(StartGameRoutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnBallKilled()
        {
            // Kick off again
            StartCoroutine(KickOffRoutine());
        }

        private void OnLevelCleared()
        {
            m_ball.Despawn();
            // Start a new game
            StartCoroutine(StartGameRoutine());
        }

        private IEnumerator KickOffRoutine()
        {
            // Recenter the player
            m_player.Recenter();

            yield return new WaitUntil(() => m_player.IsCentered);

            // Respawn the ball and open the kick off panel
            m_ball.transform.position = m_initialBallPosition;
            m_ball.Spawn();
            m_kickoffPanel.gameObject.SetActive(true);
        }

        private IEnumerator StartGameRoutine()
        {
            // Load the bricks in the level
            m_bricks.StartLevel();

            // Proceed to kickoff
            yield return KickOffRoutine();
        }

        private void KickOffPressed(Vector3 direction)
        {
            // Close the kickoffpanel and kick off the ball
            m_kickoffPanel.gameObject.SetActive(false);
            m_ball.KickOff(direction);

            // Return control to the player
            m_player.Release();
        }
    }
}