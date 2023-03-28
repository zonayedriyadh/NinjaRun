using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules;
using UnityEngine.UI;
using System;

public enum FireballState
{
    Active,
    InActive,
    Pause
}
public class FireBall : MonoBehaviour
{
    public Action<int> OnDestroyFireball;
    private Vector2 size;
    private float scale;
    private SimpleSpriteAnimator simpleAnimation;
    [SerializeField] private float speed;
    public Action<int> addPoint;
    public FireballState currentState;
    public int ballNo;
    // Start is called before the first frame update
    void Start()
    {
        currentState = FireballState.Active;
        size = GetComponent<RectTransform>().sizeDelta;
        scale = PanelController.Instance.GetScaleFactor();
        simpleAnimation = GetComponent<SimpleSpriteAnimator>();
        simpleAnimation.PlayAnimation("Fire");
        GetComponent<Image>().SetNativeSize();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == FireballState.Active || currentState == FireballState.InActive)
        {
            transform.position = new Vector2(transform.position.x - speed * scale * Time.deltaTime, transform.position.y);
            if (transform.position.x < -(size.x * scale))
            {
                
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ScoreDetector"))
        {
            addPoint?.Invoke(1);
            currentState = FireballState.InActive;
        }
    }

    void OnDestroy()
    {
        OnDestroyFireball?.Invoke(ballNo);
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            currentState = FireballState.Pause;
            simpleAnimation.PausAnimation();
        }
        else
        {
            simpleAnimation.ResumeAnimation();
            currentState = FireballState.Active;
        }
    }
}
