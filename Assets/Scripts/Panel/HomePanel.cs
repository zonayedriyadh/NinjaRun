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
        [SerializeField] private Button SoundButtonOnOff;
        [SerializeField] private Button ButtonRateUs;
        [SerializeField] private Button ButtonMoveList;
        [SerializeField] private Image SoundButtonOn;
        [SerializeField] private Image SoundButtonOff;
        // Start is called before the first frame update
        void Start()
        {
            ButtonEnterInGame.onClick.AddListener(OnCLick_EnterInGame);
            SoundButtonOnOff.onClick.AddListener(SoundOnOff);
            ButtonRateUs.onClick.AddListener(OnClickRateUs);
            ButtonMoveList.onClick.AddListener(OnClickMoveListButton);
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
            currentBackground.SetPause(true);
            StartBlickTapHere(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnCLick_EnterInGame()
        {
            AudioManager.Instance.PlayOneShot("TapButton");
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
            //SoundOnOff(PlayerPrefs.GetInt("SoundOnOff",1)==1?true:false);
            SoundButtonOn.gameObject.SetActive((PlayerPrefs.GetInt("SoundOnOff", 1) == 1 ? true : false));
            SoundButtonOff.gameObject.SetActive(!(PlayerPrefs.GetInt("SoundOnOff", 1) == 1 ? true : false));
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

        private void SoundOnOff()
        {
            bool isSoundOnOff = (PlayerPrefs.GetInt("SoundOnOff",1)==1 ? true : false);
            PlayerPrefs.SetInt("SoundOnOff", !isSoundOnOff ? 1 : 0);            
            AudioManager.Instance.SetSFXOnOff(!isSoundOnOff);

            SoundButtonOn.gameObject.SetActive(!isSoundOnOff);
            SoundButtonOff.gameObject.SetActive(isSoundOnOff);
        }

        private void OnClickRateUs()
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.casualgamestudiox.runninjarun");
        }
        private void OnClickMoveListButton()
        {
            OpenPanelWithoutClosing(PanelId.MoveList);
        }

    }
}
