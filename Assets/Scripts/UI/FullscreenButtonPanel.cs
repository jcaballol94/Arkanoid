using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caballol.Arkanoid.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class FullscreenButtonPanel<T> : Panel<T>
    {
        public static FullscreenButtonPanel<T> ButtonInstance => PanelInstance as FullscreenButtonPanel<T>;

        public event System.Action onPressed;

        protected override void Awake()
        {
            base.Awake();

            var button = GetComponent<Button>();
            button.onClick.AddListener(Close);
        }

        protected override void OnClosed()
        {
            base.OnClosed();

            onPressed?.Invoke();
        }
    }
}