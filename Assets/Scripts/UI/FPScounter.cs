using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caballol.Arkanoid.UI
{
    public class FPScounter : Panel<FPScounter>
    {
        [SerializeField] private Text m_text;

        private void Update()
        {
            m_text.text = (1f / Time.unscaledDeltaTime).ToString("F0");
        }
    }
}