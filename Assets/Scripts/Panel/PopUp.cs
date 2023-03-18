using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Modules
{
    public class PopUp : BasePanel
    {
        [Header("Popup Panel")]
        public Image Bg;
        public Image BgPopUp;
        private Vector3 bgPositon;

        public void Awake()
        {
            transitionType = PanelTransitionType.PopUp;
            bgPositon = BgPopUp.transform.position;
            Bg.gameObject.AddComponent<CanvasGroup>();
        }
        /*// Start is called before the first frame update
        public override void Start()
        {
            
            
        }*/
        public override void TransitionAnimationIn()
        {
            Bg.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
            BgPopUp.transform.localScale = Vector3.zero;
            BgPopUp.transform.DOJump(bgPositon, 100 * PanelController.Instance.GetScaleFactor(), 1, 0.3f).OnStart(() =>
            {
                BgPopUp.transform.DOScale(1, 0.3f).OnComplete(OnCompleteTransition);
            });
        }

        public override void ClosePanelWithTransition(PanelId nextPanelToOpen = PanelId.None, Action CallbackFunc = null)
        {
            state = PanelState.OnTransitionPhase;
            BgPopUp.transform.localScale = Vector3.one;
            Bg.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
            BgPopUp.gameObject.transform.DOJump(bgPositon, 100 * PanelController.Instance.GetScaleFactor(), 1, 0.3f).OnStart(() =>
            {
                BgPopUp.transform.DOScale(0, 0.3f).OnComplete(() => { ClosePanelWithOutTransition(PanelId.None, CallbackFunc); });
            });
        }
    }
}
