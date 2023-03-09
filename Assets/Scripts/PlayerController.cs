using Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NinjaRun
{
    public enum PlayerState
    {
        Running,
        Jumping,
        DoubleJumping,
        Floating
    }

    public struct InformationTouch
    {
        public Touch touch;
        public float timeOfTouch;
    }

    public class PlayerController : MonoBehaviour
    {
        public PlayerState currentState;
        public PlayerState CurrentState { get { return currentState; } set { currentState = value; } }

        private Rigidbody2D rigidBody;
        private SimpleSpriteAnimator sinmpleAnimation;
        private Vector2 startPos;
        private float currentVelocity = 0;
        private float deltaT = 0;
        private InformationTouch lastTouchInfo;

        public float gravity = -9.8f;
        public float jumpForce ;
        
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
            {
                if(lastTouchInfo.touch.deltaTime != Input.GetTouch(Input.touchCount - 1).deltaTime)
                {
                    lastTouchInfo.touch = Input.GetTouch(Input.touchCount - 1);
                    if (CurrentState == PlayerState.Running)
                    {
                        sinmpleAnimation.PlayAnimation("Jump");
                        CurrentState = PlayerState.Jumping;
                        currentVelocity = jumpForce;
                        //rigidBody.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
                    }
                }

 
            }

            if(CurrentState == PlayerState.Jumping)
            {
                deltaT += Time.deltaTime;
                applyVelocity();
            }

        }

        public void applyVelocity()
        {
            //if (CurrentState == PlayerState.Jumping)
            {
                float nextVelocity = currentVelocity + gravity * deltaT;
                currentVelocity = nextVelocity;
                transform.position = new Vector3(transform.position.x, transform.position.y + currentVelocity *Time.deltaTime);
            }
        }

        public void Initialize()
        {
            CurrentState = PlayerState.Running;
            rigidBody = transform.GetComponent<Rigidbody2D>();
            sinmpleAnimation = transform.GetComponent<SimpleSpriteAnimator>();
            sinmpleAnimation.PlayAnimation("Run");
            deltaT = 0;
            startPos = transform.position;
            lastTouchInfo.timeOfTouch = 0;
        }

        public void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                //Debug.Log("Ground exit");
                CurrentState = PlayerState.Jumping;
            }
        }
    
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                if(CurrentState == PlayerState.Jumping)
                {
                    deltaT = 0;
                    currentVelocity = 0;
                    transform.position = startPos;
                    CurrentState = PlayerState.Running;
                    sinmpleAnimation.PlayAnimation("Run");
                }
                
            }
        }
    }
}
