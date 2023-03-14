using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public enum PanelState
{
    InActive,
    Active,
    OnTransitionPhase,

}
public enum PanelTransitionType
{
    BlackOut,
    None,
    PopUp
}
namespace Modules
{
    public abstract class PanelProperties
    {

    }
    [RequireComponent(typeof(CanvasGroup))]
    public class BasePanel : MonoBehaviour
    {
        [Header("Base Panel")]
        public PanelId panelId;
        public PanelState state;
        public PanelTransitionType transitionType;
        [SerializeField]
        [Range(0.1f, 0.5f)]
        private float halfTransitionTime = 0.3f;
        private CanvasGroup BlackImage;

        public virtual void OnEnable()
        {
            state = PanelState.OnTransitionPhase;
            TransitionAnimationIn();
        }

        public virtual void TransitionAnimationIn()
        {
            if (transitionType == PanelTransitionType.BlackOut)
            {
                if (BlackImage == null)
                {
                    CreateBlackImage();
                }
                //BlackImage.gameObject.SetActive(true);
                BlackImage.alpha = 1;
                Tween fadeOut = BlackImage.DOFade(0, halfTransitionTime).OnComplete(() => {
                    OnCompleteTransition();
                });
            }
            else
            {
                OnCompleteTransition();
            }
        }

        public virtual void OnCompleteTransition()
        {
            state = PanelState.Active;
        }

        public virtual void OnDisable()
        {
            state = PanelState.InActive;
        }
        public virtual void SetPropeties(PanelProperties propeties) { }
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }

        public void OpenPanelWithoutClosing(PanelId nextPanelToOpen, PanelProperties propeties = null)
        {
            PanelController.Instance.OpenPanel(nextPanelToOpen, propeties);
        }
        public virtual void ClosePanelWithTransition(PanelId nextPanelToOpen = PanelId.None, Action CallbackFunc = null)
        {
            state = PanelState.OnTransitionPhase;
            if (transitionType == PanelTransitionType.BlackOut)
            {
                if (BlackImage == null)
                {
                    CreateBlackImage();
                }
                BlackImage.alpha = 0;
                BlackImage.DOFade(1, halfTransitionTime).OnComplete(() => {
                    CallbackFunc?.Invoke();
                    BlackImage.alpha = 0;
                    PanelController.Instance.ClosePanel(panelId);
                    if (nextPanelToOpen != PanelId.None)
                    {
                        PanelController.Instance.OpenPanel(nextPanelToOpen);
                    }
                });
            }
            else if (transitionType == PanelTransitionType.PopUp)
            {
                transform.localScale = Vector3.one;
                gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
                gameObject.transform.DOJump(transform.position, 100 * PanelController.Instance.GetScaleFactor(), 1, 0.3f).OnStart(() =>
                {
                    gameObject.transform.DOScale(0, 0.3f).OnComplete(() => { ClosePanelWithOutTransition(PanelId.None); });
                });
            }
            else
            {
                ClosePanelWithOutTransition(nextPanelToOpen, CallbackFunc);
            }
        }

        public void CreateBlackImage()
        {
            GameObject blackImage = new GameObject();
            blackImage.name = "TransitionImage";
            blackImage.AddComponent<RectTransform>();
            blackImage.AddComponent<Image>();
            blackImage.GetComponent<Image>().color = Color.black;
            blackImage.GetComponent<Image>().raycastTarget = false;
            blackImage.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            blackImage.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            blackImage.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

            blackImage.transform.SetParent(transform, false);
            blackImage.AddComponent<CanvasGroup>();
            BlackImage = blackImage.GetComponent<CanvasGroup>();
        }

        public void ClosePanelWithTransitionAndProperties(PanelId nextPanelToOpen = PanelId.None, PanelProperties properties = null)
        {
            state = PanelState.OnTransitionPhase;
            if (BlackImage != null)
            {
                BlackImage.DOFade(1, halfTransitionTime).OnComplete(() => {
                    PanelController.Instance.ClosePanel(panelId);
                    if (nextPanelToOpen != PanelId.None)
                    {
                        PanelController.Instance.OpenPanel(nextPanelToOpen, properties);
                    }
                });
            }
            else
            {
                ClosePanelWithOutTransition(nextPanelToOpen);
            }

        }

        public void ClosePanelWithOutTransition(PanelId nextPanelToOpen = PanelId.None, Action CallbackFunc = null)
        {
            state = PanelState.OnTransitionPhase;
            CallbackFunc?.Invoke();
            PanelController.Instance.ClosePanel(panelId);
            if (nextPanelToOpen != PanelId.None)
            {
                PanelController.Instance.OpenPanel(nextPanelToOpen);
            }
        }
    }
}

