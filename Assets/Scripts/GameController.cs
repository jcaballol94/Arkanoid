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
        [SerializeField] private Brick[] m_bricks;

        [Header("UI")]
        [SerializeField] private UI.KickOffPanel m_kickoffPanel;

        private Vector3 m_initialBallPosition;

        private void Start()
        {
            m_initialBallPosition = m_ball.transform.position;
            m_ball.onKilled += OnBallKilled;

            m_kickoffPanel.SetBallPosition(m_initialBallPosition);
            m_kickoffPanel.onKickoffPressed += KickOffPressed;
            m_kickoffPanel.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnBallKilled()
        {
            StartCoroutine(RecenterRoutine());
        }

        private IEnumerator RecenterRoutine()
        {
            // Recenter the player
            m_player.Recenter();

            yield return new WaitUntil(() => m_player.IsCentered);

            // Respawn the ball and open the kick off panel
            m_ball.transform.position = m_initialBallPosition;
            m_ball.gameObject.SetActive(true);
            m_kickoffPanel.gameObject.SetActive(true);
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