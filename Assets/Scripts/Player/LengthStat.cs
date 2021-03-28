using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class LengthStat : MonoBehaviour
    {
        [SerializeField] private float m_length = 2f;
        [SerializeField] private float m_blend = 0.2f;

        private float Length
        {
            get => transform.localScale.x;
            set
            {
                var scale = transform.localScale;
                scale.x = value;
                transform.localScale = scale;
            }
        }

        private void OnValidate()
        {
            m_length = Mathf.Max(m_length, 0f);
            m_blend = Mathf.Max(m_blend, 0.001f);
        }

        private void Awake()
        {
            Length = m_length;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void CancelPowerUps()
        {
            StopAllCoroutines();
        }

        public void PowerUp (float a_bonus, float a_duration)
        {
            // Cancel the previous bonus
            CancelPowerUps();
            // Enable the new one
            StartCoroutine(PowerUpRoutine(a_bonus, a_duration));
        }

        private IEnumerator PowerUpRoutine (float a_bonus, float a_duration)
        {
            yield return FadeRoutine(m_length * a_bonus);
            yield return new WaitForSeconds(a_duration);
            yield return FadeRoutine(m_length);
        }

        private IEnumerator FadeRoutine(float a_target)
        {
            var t = 0f;
            var initial = Length;
            do
            {
                Length = Mathf.Lerp(initial, a_target, t / m_blend);
                yield return null;
                t += Time.deltaTime;
            }
            while (t < m_blend);

            Length = a_target;
        }
    }
};