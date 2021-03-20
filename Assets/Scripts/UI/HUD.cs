using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caballol.Arkanoid.UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private GameController m_game;
        [SerializeField] private Text m_score;
        
        // Update is called once per frame
        void Update()
        {
            m_score.text = "Lives: " + m_game.Lives.ToString();
        }
    }
}