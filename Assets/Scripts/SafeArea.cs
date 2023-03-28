using System;
using UnityEngine;


public class SafeArea : MonoBehaviour
{
    private RectTransform _rectTransform;

    private readonly bool isBannerAdAvailable = false;
    private readonly bool isBannerTop = true;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        //GameEvents.OnRemovedAds += OnRemovedAds;
        FitArea();
    }

    private void OnDisable()
    {
        //GameEvents.OnRemovedAds -= OnRemovedAds;
    }

    private void FitArea()
    {
        if (!_rectTransform)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        float bannerHeight = 0;
        float screenRatio = (float)Screen.width / (float)Screen.height;

        float bannerHightRatio = bannerHeight / Screen.height;

        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        if (isBannerAdAvailable)
        {
            if (isBannerTop)
            {
                anchorMax.y -= bannerHightRatio;
            }
            else
            {
                anchorMin.y += bannerHightRatio;
            }
        }

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }

    #region Event Callbacks

    private void OnRemovedAds()
    {
        FitArea();
    }

    #endregion
}

