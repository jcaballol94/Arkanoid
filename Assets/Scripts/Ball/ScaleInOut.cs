using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.Gameplay
{
    public class ScaleInOut : MonoBehaviour
    {
        [SerializeField] private float m_time = 0.5f;

        public Vector3 BaseScale { get; private set; }

        // Sinc with routines
        public bool Fading { get; private set; }

        private void OnValidate()
        {
            m_time = Mathf.Max(0.001f, m_time);
        }

        private void Awake()
        {
            BaseScale = transform.localScale;
            Fading = false;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void ScaleIn()
        {
            StopAllCoroutines();
            transform.localScale = Vector3.zero;
            StartCoroutine(Scale(BaseScale));
        }

        public void ScaleOut()
        {
            StopAllCoroutines();
            StartCoroutine(Scale(Vector3.zero));
        }

        private IEnumerator Scale(Vector3 a_target)
        {
            Fading = true;
            var initial = transform.localScale;
            var t = 0f;
            while (t < m_time)
            {
                transform.localScale = Vector3.Lerp(initial, a_target, t / m_time);
                yield return null;
                t += Time.deltaTime;
            }
            transform.localScale = a_target;
            Fading = false;
        }
    }
}