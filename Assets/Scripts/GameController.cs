using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class GameController : MonoBehaviour
    {
        [Header("Actors")]
        [SerializeField] private GameObject m_playerActor;

        [Header("References")]
        [SerializeField] private Transform m_playerStartPosition;

        [Header("Objects")]
        [SerializeField] private BallManager m_balls;
        [SerializeField] private BrickManager m_bricks;
        [SerializeField] private PowerUpManager m_powerUps;

        [Header("Settings")]
        [SerializeField] private int m_lives = 3;

        [Header("UI")]
        [SerializeField] private GameObject m_winPanel;
        [SerializeField] private GameObject m_losePanel;
        [SerializeField] private UI.KickOffPanel m_kickoffPanel;

        public int Lives => m_remainingLives;

        private bool m_canContinue;
        private int m_remainingLives;

        private Player m_player;

        private void Start()
        {
            var playerGO = Instantiate(m_playerActor, m_playerStartPosition.position, Quaternion.identity, transform);
            m_player = playerGO.GetComponent<Player>();

            // Prepare the powerups
            m_powerUps.onPicked += OnPowerUpPicked;

            // Prepare the bricks
            m_bricks.onLevelCleared += OnLevelCleared;
            m_bricks.onBrickDestroyed += OnBrickDestroyed;

            // Prepare the ball
            m_balls.onPlayerDied += OnBallKilled;

            // Prepare the kickof panel
            m_kickoffPanel.SetBallPosition(m_balls.transform.position);
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
            yield return m_player.RecenterRoutine();

            // Respawn the ball and open the kick off panel
            m_balls.SpawnBall();
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
            m_balls.DespawnBalls();
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
            m_balls.KickOff(direction);

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
            else if (typeFlags.HasFlag(PowerUp.PowerUpFlags.BALL))
            {
                m_balls.ApplyPowerUp(powerUp);
            }
        }
    }
}