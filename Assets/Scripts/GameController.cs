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
    }
}