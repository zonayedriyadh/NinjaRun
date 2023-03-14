using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules;
using UnityEngine.UI;
using NinjaRun;
using UnityEngine.EventSystems;
using TMPro;

namespace NinjaRun
{
    public enum GameState
    {
        TransitionPeriod,
        Running,
        Pause,
        GameOver
    }

    [SerializeField]
    public class FireballCreation
    {
        public List<float> listYPos;
        public int countOfCreation;
    }

    public class GamePlanel : BasePanel, IPointerDownHandler, IPointerUpHandler
    {
        private GameState currentState;
        [SerializeField]private Button ButtonBack;
        [SerializeField]private List<ParallaxBackgrounds> listOfParallaxBackGround;
        private ParallaxBackgrounds currentBackground;
        [SerializeField] private PlayerController player;
        [SerializeField] private GameObject fireballAreaPrefab;
        private GameObject fireballArea;
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private List<float> listOfDistanceOfCreateFireBalls;
        private float currentDistanceFireBall;
        private int gamePoint = 0;
        [SerializeField] TextMeshProUGUI textScore;
        private PointerEventData lastPointerData = null;
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
            currentState = GameState.Running;
        }
        private void Update()
        {
            if (currentState == GameState.Running)
            {
                if (lastFireball != null && currentState == GameState.Running)
                {
                    float distance = Screen.width - lastFireball.transform.position.x;
                    if (distance > currentDistanceFireBall * PanelController.Instance.GetScaleFactor())
                    {
                        //Debug.Log(" game state -> "+currentState.ToString());
                        CreateFireBall();
                    }
                }
                else if (lastFireball == null)
                {
                    CreateFireBall();
                }
            }
        }
        private void Initialize()
        {
            int rand = Random.Range(0, listOfParallaxBackGround.Count);
            gamePoint = 0;
            currentBackground = listOfParallaxBackGround[rand];
            currentBackground.gameObject.SetActive(true);
            currentBackground.ReInitialize();
            player.Initialize(PlayerDeath);
            CreateFireballArea();
            currentState = GameState.TransitionPeriod;
            CreateFireBall();
            StartCoroutine("ChangeOfDistance");
            updateScore();
        }
        
        private void CreateFireballArea()
        {
            if(fireballArea != null)
            {
                Destroy(fireballArea);
            }

            fireballArea = Instantiate(fireballAreaPrefab,transform);
        }
        private void PlayerDeath()
        {
            currentState = GameState.GameOver;
            GameOverCall();
        }

        private void GameOverCall()
        {
            currentBackground.SetPause();
            currentState = GameState.GameOver;

            GameOverProperties properties = new GameOverProperties();
            properties.score = gamePoint;
            ClosePanelWithTransitionAndProperties(PanelId.GameOver, properties);
        }
        private void AddScore(int score)
        {
            if (currentState == GameState.Running)
            {
                gamePoint += score;
                updateScore();
            }
        }
        private void updateScore()
        {
            textScore.text = gamePoint.ToString();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            currentBackground.gameObject.SetActive(false);
            currentBackground.SetPause();
            StopCoroutine("ChangeOfDistance");
        }

        private IEnumerator ChangeOfDistance()
        {
            currentDistanceFireBall = listOfDistanceOfCreateFireBalls[Random.Range(0, listOfDistanceOfCreateFireBalls.Count)];
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("ChangeOfDistance");
        }
        public void OnCLick_BackButton()
        {
            ClosePanelWithTransition(PanelId.Home);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            lastPointerData = eventData;
            player.PlayerInstruction(PlayerState.Jumping);
        }
        public void CreateFireBall()
        {
            List<float> listOfPos = new List<float> { 230,275,350,500,650};
            int rand = Random.Range(0, listOfPos.Count);
            float posY = listOfPos[rand];
            lastFireball = Instantiate(fireballPrefab, fireballArea.transform);
            lastFireball.SetActive(true);
            lastFireball.GetComponent<FireBall>().addPoint = AddScore;
            Vector2 size = lastFireball.GetComponent<RectTransform>().sizeDelta;
            lastFireball.transform.position = new Vector2(Screen.width+size.x *PanelController.Instance.GetScaleFactor(),posY* PanelController.Instance.GetScaleFactor());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(lastPointerData.pointerId == eventData.pointerId)
            {
                player.IsFloatingStarted = false;
                player.PlayerInstruction(PlayerState.DoubleJumping);
            }
        }
    }
}
