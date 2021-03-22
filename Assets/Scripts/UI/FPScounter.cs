using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caballol
{
    public class FPScounter : MonoBehaviour
    {
        [SerializeField] private Text m_text;

        private void Update()
        {
            m_text.text = (1f / Time.deltaTime).ToString("F0");
        }
    }
}