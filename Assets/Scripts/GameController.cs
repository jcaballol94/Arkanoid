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
        [SerializeField] private GameObject m_kickoffPanel;

        private Vector3 m_initialBallPosition;

        private void Start()
        {
            m_kickoffPanel.SetActive(true);

            m_initialBallPosition = m_ball.transform.position;
            m_ball.onKilled += OnBallKilled;
        }

        private void OnBallKilled()
        {
            // Respawn the ball and open the kick off panel
            m_ball.transform.position = m_initialBallPosition;
            m_ball.gameObject.SetActive(true);
            m_kickoffPanel.SetActive(true);
        }

        public void KickOffPressed()
        {
            // Close the kickoffpanel and kick off the ball
            m_kickoffPanel.SetActive(false);
            m_ball.KickOff();
        }
    }
}