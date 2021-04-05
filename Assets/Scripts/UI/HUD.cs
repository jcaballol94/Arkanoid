using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Caballol.Arkanoid.UI
{
    public class HUD : Panel<HUD>
    {
        [SerializeField] private Gameplay.GameController m_game;
        [SerializeField] private Text m_score;
        
        // Update is called once per frame
        void Update()
        {
            m_score.text = "Lives: " + m_game.Lives.ToString();
        }
    }
}