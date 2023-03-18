using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules;
using TMPro;
using UnityEngine.UI;
using System;

namespace NinjaRun
{
    public class GameOverProperties : PanelProperties
    {
        public int score;
        public Action OnClickReplay;
        public Action OnClickHome;
    }
    public class GameOverPanel : PopUp
    {
        private enum ButtonId
        {
            Home,
            Replay,
            Leaderboard
        }
        private GameOverProperties property;
        [SerializeField] private Button buttonHome;
        [SerializeField] private Button buttonReplay;
        [SerializeField] private Button buttonLeaderBoard;

        [SerializeField] private TextMeshProUGUI textScoreAmount;
        [SerializeField] private TextMeshProUGUI textHighestScoreAmount;
        // Start is called before the first frame update
        void Start()
        {
            buttonHome.onClick.AddListener(() => OnClick_Butttons(ButtonId.Home));
            buttonReplay.onClick.AddListener(() => OnClick_Butttons(ButtonId.Replay));
            buttonLeaderBoard.onClick.AddListener(() => OnClick_Butttons(ButtonId.Leaderboard));

            buttonHome.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
            buttonReplay.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
            buttonLeaderBoard.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void SetPropeties(PanelProperties _properties)
        {
            
            property = (GameOverProperties)_properties;
            SetScoreText();
        }

        private void SetScoreText()
        {
            textScoreAmount.text = property.score.ToString();

            int highestScore = PlayerPrefs.GetInt("HighestScore", 0);

            if (property.score >= highestScore)
            {
                PlayerPrefs.SetInt("HighestScore", property.score);
            }
            textHighestScoreAmount.text = PlayerPrefs.GetInt("HighestScore", 0).ToString();
        }
        private void OnClick_Butttons(ButtonId buttonId)
        {
            switch (buttonId)
            {
                case ButtonId.Home:
                    ClosePanelWithTransition(PanelId.Home, property.OnClickHome);
                    //property.OnClickHome?.Invoke();
                    break;
                case ButtonId.Replay:
                    ClosePanelWithTransition(PanelId.GamePlay, property.OnClickReplay);
                    break;
                case ButtonId.Leaderboard:
                    //ClosePanelWithTransition(PanelId.GamePlay);
                    break;
            }
        }
    }
}