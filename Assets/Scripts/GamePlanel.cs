using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules;
using UnityEngine.UI;

public class GamePlanel : BasePanel
{
    [SerializeField]
    private Button ButtonBack;
    [SerializeField]
    private List<ParallaxBackgrounds> listOfParallaxBackGround;
    private ParallaxBackgrounds currentBackground;

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

    private void Initialize()
    {
        int rand = Random.Range(0, listOfParallaxBackGround.Count);
        currentBackground = listOfParallaxBackGround[rand];
        currentBackground.gameObject.SetActive(true);
        currentBackground.ReInitialize();
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
}
