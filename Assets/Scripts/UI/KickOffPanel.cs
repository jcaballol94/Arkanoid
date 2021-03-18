using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.UI
{
    public class KickOffPanel : MonoBehaviour
    {
        public event System.Action<Vector3> onKickoffPressed;

        [SerializeField] private RectTransform m_directionIndicator;
        [SerializeField] [Range(0f, 90f)] private float m_maxAngle = 45f;
        [SerializeField] [Range(0f, 360f)] private float m_speed = 90f;

        private float m_angle = 0f;

        public void SetBallPosition(Vector3 position)
        {
            var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
            m_directionIndicator.position = screenPos;
        }

        private void Update()
        {
            // Update the angle
            m_angle += m_speed * Time.deltaTime;

            // Change direction if we reach an end
            if (Mathf.Abs(m_angle) > m_maxAngle)
            {
                m_angle = Mathf.Clamp(m_angle, -m_maxAngle, m_maxAngle);
                m_speed *= -1f;
            }

            // Update the indicator
            m_directionIndicator.transform.rotation = Quaternion.AngleAxis(m_angle, Vector3.forward);
        }

        public void OnPressed()
        {
            onKickoffPressed?.Invoke(Quaternion.AngleAxis(m_angle, Vector3.forward) * Vector3.up);
        }
    }
}