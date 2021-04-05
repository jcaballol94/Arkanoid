using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.UI
{
    public class KickOffPanel : FullscreenButtonPanel<KickOffPanel>
    {
        public event System.Action<Vector3> onKickoffPressed;

        [SerializeField] private RectTransform m_directionIndicator;
        [SerializeField] [Range(0f, 90f)] private float m_maxAngle = 45f;
        [SerializeField] [Range(0f, 360f)] private float m_speed = 90f;

        private float m_angle = 0f;

        public static KickOffPanel Instance => PanelInstance as KickOffPanel;

        public static void Open(Vector3 a_position)
        {
            Instance.SetBallPosition(a_position);
            Open();
        }

        private void SetBallPosition(Vector3 a_position)
        {
            var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, a_position);
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

        protected override void OnClosed()
        {
            onKickoffPressed?.Invoke(Quaternion.AngleAxis(m_angle, Vector3.forward) * Vector3.up);
        }
    }
}