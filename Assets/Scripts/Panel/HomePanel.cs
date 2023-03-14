using Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : BasePanel
{
    [SerializeField]
    private Button ButtonEnterInGame;
    // Start is called before the first frame update
    void Start()
    {
        ButtonEnterInGame.onClick.AddListener(OnCLick_EnterInGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCLick_EnterInGame()
    {
        ClosePanelWithTransition(PanelId.GamePlay);
    }
}
