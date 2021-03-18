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

        private Vector3 m_initialBallPosition;

        private void Start()
        {
            m_initialBallPosition = m_ball.transform.position;
            m_ball.onKilled += OnBallKilled;
        }

        private void OnBallKilled()
        {
            m_ball.transform.position = m_initialBallPosition;
            m_ball.gameObject.SetActive(true);
        }
    }
}