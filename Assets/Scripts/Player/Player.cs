using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class Player : MonoBehaviour
    {
        public Vector3 BallSpawnPosition => m_ballSpawnReference ? m_ballSpawnReference.position : Vector3.zero;
        [SerializeField] private Transform m_ballSpawnReference;

        private CenterableBehaviour m_centerable;
        private DraggableBehaviour m_draggable;
        private LengthStat m_length;

        private void Awake()
        {
            m_centerable = GetComponentInChildren<CenterableBehaviour>();
            m_draggable = GetComponentInChildren<DraggableBehaviour>();
            m_length = GetComponentInChildren<LengthStat>();
        }

        public IEnumerator RecenterRoutine()
        {
            yield return m_centerable.CenterRoutine();
        }

        public void Stop()
        {
            m_draggable.Enabled = false;
        }

        public void Release()
        {
            m_draggable.Enabled = true;
        }

        public void ApplyPowerUp(PowerUp powerUp)
        {
            if (powerUp.Type == PowerUp.PowerUpType.PLAYER_LENGTH)
            {
                m_length.PowerUp(powerUp.FloatValue, powerUp.Duration);
            }
        }

        public void CancelPowerUps()
        {
            m_length.CancelPowerUps();
        }
    }
}