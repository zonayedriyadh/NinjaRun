using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules;
using UnityEngine.UI;
using NinjaRun;
using UnityEngine.EventSystems;

namespace NinjaRun
{
    public class GamePlanel : BasePanel, IPointerDownHandler
    {
        [SerializeField]private Button ButtonBack;
        [SerializeField]private List<ParallaxBackgrounds> listOfParallaxBackGround;
        private ParallaxBackgrounds currentBackground;
        [SerializeField] private PlayerController player;
        [SerializeField] private GameObject fireballArea;
        [SerializeField] private GameObject fireballPrefab;
        private GameObject lastFireball = null;

        public override void OnEnable()
        {
            base.OnEnable();
            Initialize();
        }
        public void Start()
        {
            ButtonBack.onClick.AddListener(OnCLick_BackButton);
        }
        public override void OnCompleteTransition()
        {
            base.OnCompleteTransition();
        }
        private void Update()
        {
            if (lastFireball != null)
            {
                float distance = Screen.width - lastFireball.transform.position.x;
                if(distance > 350*PanelController.Instance.GetScaleFactor())
                {
                    CreateFireBall();
                }
            }
        }
        private void Initialize()
        {
            int rand = Random.Range(0, listOfParallaxBackGround.Count);
            currentBackground = listOfParallaxBackGround[rand];
            currentBackground.gameObject.SetActive(true);
            currentBackground.ReInitialize();
            player.Initialize();
            CreateFireBall();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            currentBackground.gameObject.SetActive(false);
            currentBackground.SetPause();
        }

        public void OnCLick_BackButton()
        {
            ClosePanelWithTransition(PanelId.Home);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            player.PlayerInstruction(PlayerState.Jumping);
        }

        public void CreateFireBall()
        {
            List<float> listOfPos = new List<float> { 250,500,350,600};
            int rand = Random.Range(0, listOfPos.Count);
            float posY = listOfPos[rand];
            lastFireball = Instantiate(fireballPrefab, fireballArea.transform);
            lastFireball.SetActive(true);
            Vector2 size = lastFireball.GetComponent<RectTransform>().sizeDelta;
            lastFireball.transform.position = new Vector2(Screen.width+size.x *PanelController.Instance.GetScaleFactor(),posY* PanelController.Instance.GetScaleFactor());
        }
    }
}
