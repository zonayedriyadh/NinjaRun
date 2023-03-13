using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules;
using UnityEngine.UI;
using System;

public class FireBall : MonoBehaviour
{
    public bool isActive = true;
    private Vector2 size;
    private float scale;
    private SimpleSpriteAnimator simpleAnimation;
    [SerializeField] private float speed;
    public Action<int> addPoint;
    // Start is called before the first frame update
    void Start()
    {
        size = GetComponent<RectTransform>().sizeDelta;
        scale = PanelController.Instance.GetScaleFactor();
        simpleAnimation = GetComponent<SimpleSpriteAnimator>();
        simpleAnimation.PlayAnimation("Fire");
        GetComponent<Image>().SetNativeSize();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x - speed *scale* Time.deltaTime, transform.position.y);
        if(transform.position.x < -(size.x *scale))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ScoreDetector"))
        {
            addPoint?.Invoke(1);
            isActive = false;
        }
    }
    /*public void DestroyFireBall()
    {
        addPoint?.Invoke(isActive?1:0);
        Destroy(gameObject);
    }*/

    /*private void OnTriggetEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }*/
}
