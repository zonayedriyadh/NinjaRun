using Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace NinjaRun
{
    public class HomePanel : BasePanel
    {
        [SerializeField]
        private Button ButtonEnterInGame;
        [SerializeField] private List<ParallaxBackgrounds> listOfParallaxBackGround;
        private ParallaxBackgrounds currentBackground;
        [SerializeField] private PlayerController player;

        [SerializeField] private GameObject TapHereText;
        // Start is called before the first frame update
        void Start()
        {
            ButtonEnterInGame.onClick.AddListener(OnCLick_EnterInGame);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            Initialize();
        }

        public override void OnCompleteTransition()
        {
            base.OnCompleteTransition();
            player.Initialize();
        }
        public override void OnDisable()
        {
            base.OnDisable();
            currentBackground.gameObject.SetActive(false);
            currentBackground.SetPause();
            StartBlickTapHere(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnCLick_EnterInGame()
        {
            StartBlickTapHere(false);
            ClosePanelWithTransition(PanelId.GamePlay);
        }

        private void Initialize()
        {
            int rand = Random.Range(0, listOfParallaxBackGround.Count);
            currentBackground = listOfParallaxBackGround[rand];
            currentBackground.gameObject.SetActive(true);
            currentBackground.ReInitialize();
            
            StartBlickTapHere(true);
        }

        private void StartBlickTapHere(bool isAnimationOn)
        {
            if(isAnimationOn)
            {
                StartBlickTapHere(false);
                TapHereText.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() => {
                    TapHereText.GetComponent<TextMeshProUGUI>().color = new Color(Random.Range(0,1f), Random.Range(0, 1f), Random.Range(0, 1f));
                    TapHereText.GetComponent<CanvasGroup>().DOFade(1, 1f).OnComplete(() => StartBlickTapHere(true));
                });
            }
            else
            {
                TapHereText.GetComponent<CanvasGroup>().DOKill();
            }
            
        }

    }
}
