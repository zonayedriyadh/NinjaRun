using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Modules
{
    public enum PanelId
    {
        None,
        Home,
        GamePlay,
        Setting,
        MoveList,
        Store,
        Pause,
        GameOver,
        BonusGame,
        CommingSoon,
        Warning,
        Test,
        Paytable,
        GameLobby,
        SlotPanel,
        NoCoinsPopup,
        LevelUp,
        NoInternet
    }

    public class PanelController : MonoBehaviour
    {
        private static PanelController instance;
        [SerializeField]
        private RectTransform canvas;

        public static PanelController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PanelController>();
                }
                return instance;
            }
        }

        public List<BasePanel> basePanels;

        private void Awake()
        {
            basePanels = new List<BasePanel>();

            foreach (Transform panel in transform)
            {
                BasePanel basePanel = panel.GetComponent<BasePanel>();
                if (basePanel != null)
                {
                    basePanels.Add(basePanel);
                }
            }
        }


        public float GetScaleFactor()
        {
            return GetComponent<Canvas>().scaleFactor;
        }

        public Canvas GetCanvas()
        {
            return GetComponent<Canvas>();
        }
        public Vector2 GetSizeDelta()
        {
            return canvas.sizeDelta;
        }

        public void OpenPanel(PanelId panelId, PanelProperties properties = null)
        {
            foreach (BasePanel basePanel in basePanels)
            {
                if (basePanel.panelId == panelId)
                {
                    basePanel.Open();
                    if (properties != null)
                        basePanel.SetPropeties(properties);
                    break;
                }
            }
        }

        public void ClosePanel(PanelId panelId)
        {
            foreach (BasePanel basePanel in basePanels)
            {
                if (basePanel.panelId == panelId)
                {
                    basePanel.Close();
                    break;
                }
            }
        }

        public bool IsPanelOpen(PanelId panelId)
        {
            foreach (BasePanel basePanel in basePanels)
            {
                if (basePanel.panelId == panelId)
                {
                    return basePanel.gameObject.activeSelf;
                }
            }
            return false;
        }
        public BasePanel GetPanelIfOpen(PanelId panelId)
        {
            foreach (BasePanel basePanel in basePanels)
            {
                if (basePanel.panelId == panelId && basePanel.gameObject.activeSelf)
                {
                    return basePanel;
                }
            }
            return null;
        }
    }
}
