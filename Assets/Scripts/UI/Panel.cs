using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caballol.Arkanoid.UI
{
    public abstract class Panel<T> : MonoBehaviour
    {
        [SerializeField] private bool m_startEnabled = false;

        public bool IsOpen { get; private set; }

        protected static Panel<T> PanelInstance => m_instance;
        private static Panel<T> m_instance;

        protected virtual void Awake()
        {
            m_instance = this;

            // Recenter it
            var rect = transform as RectTransform;
            rect.anchoredPosition = Vector2.zero;

            // Only keep enable if needed
            gameObject.SetActive(m_startEnabled);
            IsOpen = m_startEnabled;
        }

        public static void Open()
        {
            if (m_instance.IsOpen) return;

            m_instance.IsOpen = true;
            m_instance.gameObject.SetActive(true);
            m_instance.OnOpened();
        }

        public static void Close()
        {
            m_instance.OnClosed();
            m_instance.IsOpen = false;
            m_instance.gameObject.SetActive(false);
        }

        protected virtual void OnOpened() { }
        protected virtual void OnClosed() { }
    }
}