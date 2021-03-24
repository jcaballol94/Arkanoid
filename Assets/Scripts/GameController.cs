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
        [SerializeField] private PowerUpManager m_powerUps;

        [Header("Settings")]
        [SerializeField] private int m_lives = 3;

        [Header("UI")]
        [SerializeField] private UI.KickOffPanel m_kickoffPanel;
        [SerializeField] private GameObject m_winPanel;
        [SerializeField] private GameObject m_losePanel;

        public int Lives => m_remainingLives;

        private Vector3 m_initialBallPosition;
        private bool m_canContinue;
        private int m_remainingLives;

        private void Start()
        {
            // Prepare the powerups
            m_powerUps.onPicked += OnPowerUpPicked;

            // Prepare the bricks
            m_bricks.onLevelCleared += OnLevelCleared;
            m_bricks.onBrickDestroyed += OnBrickDestroyed;

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
            // Lose a life
            if (--m_remainingLives == 0)
            {
                // Lose
                StartCoroutine(LoseRoutine());
            }
            else
            {
                // Kickoff again
                StartCoroutine(KickOffRoutine());
            }
        }

        private void OnLevelCleared()
        {
            // Start a new game
            StartCoroutine(WinRoutine());
        }

        private void OnBrickDestroyed (Vector3 position)
        {
            // Notify the power ups that a brick has been destroyed, to spawn if needed
            m_powerUps.SpawnIfNeeded(position);
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
            m_remainingLives = m_lives;

            // Hide the panels
            m_winPanel.SetActive(false);
            m_losePanel.SetActive(false);

            // Load the bricks in the level
            m_bricks.StartLevel();

            // Proceed to kickoff
            yield return KickOffRoutine();
        }

        private void StopPlaying()
        {
            m_powerUps.Clear();
            m_ball.Despawn();
            m_player.Stop();
            m_player.CancelPowerUps();
        }

        private IEnumerator WinRoutine()
        {
            StopPlaying();

            // Show the win panel
            m_winPanel.SetActive(true);

            // Give time to avoid skiping inadvertently
            yield return new WaitForSecondsRealtime(0.5f);

            // Wait for the user to close the panel
            m_canContinue = false;
            yield return new WaitUntil(() => m_canContinue);

            // Start the game
            yield return StartGameRoutine();
        }

        private IEnumerator LoseRoutine()
        {
            StopPlaying();

            // Show the lose panel
            m_losePanel.SetActive(true);

            // Give time to avoid skiping inadvertently
            yield return new WaitForSecondsRealtime(0.5f);

            // Wait for the user to close the panel
            m_canContinue = false;
            yield return new WaitUntil(() => m_canContinue);

            // Start the game
            yield return StartGameRoutine();
        }

        private void KickOffPressed(Vector3 direction)
        {
            // Close the kickoffpanel and kick off the ball
            m_kickoffPanel.gameObject.SetActive(false);
            m_ball.KickOff(direction);

            // Return control to the player
            m_player.Release();
        }

        public void Continue()
        {
            m_canContinue = true;
        }

        private void OnPowerUpPicked(PowerUp powerUp)
        {
            // Direct the power ups to the right target
            var typeFlags = (PowerUp.PowerUpFlags)powerUp.Type;
            if (typeFlags.HasFlag(PowerUp.PowerUpFlags.PLAYER))
            {
                m_player.ApplyPowerUp(powerUp);
            }
        }
    }
}