using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid
{
    [CreateAssetMenu(fileName = "PowerUp", menuName = "Arkanoid/PowerUp", order = 0)]
    public class PowerUp : ScriptableObject
    {
        public GameObject Visual => m_visual;
        [SerializeField] private GameObject m_visual;

        public PowerUpType Type => m_type;
        [SerializeField] private PowerUpType m_type;
        public float FloatValue => m_floatValue;
        [SerializeField] public float m_floatValue;

        public float Duration => m_duration;
        [SerializeField] public float m_duration;

        [Flags] public enum PowerUpFlags
        {
            PLAYER = 0x01,
            BALL = 0x02,

            STATS_BONUS = 0x10
        }

        public enum PowerUpType
        {
            PLAYER_LENGTH = PowerUpFlags.PLAYER | PowerUpFlags.STATS_BONUS,
            BALL_SPEED = PowerUpFlags.BALL | PowerUpFlags.STATS_BONUS
        }
    }
}