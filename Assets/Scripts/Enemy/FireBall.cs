using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules;
using UnityEngine.UI;

public class FireBall : MonoBehaviour
{
    private Vector2 size;
    private float scale;
    private SimpleSpriteAnimator simpleAnimation;
    [SerializeField] private float speed;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
