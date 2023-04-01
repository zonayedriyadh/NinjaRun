using Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveListPanelPopup : PopUp
{
    // Start is called before the first frame update
    [SerializeField] private Button buttomHome;
    void Start()
    {
        buttomHome.onClick.AddListener(OnClickHomeButton);
    }

    private void OnClickHomeButton()
    {
        ClosePanelWithTransition();
    }

}
