using Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace NinjaRun
{
    public enum PlayerState
    {
        Running,
        Jumping,
        DoubleJumping,
        Floating,
        dead
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
        public float currentVelocity = 0;
        private float deltaT = 0;
        //private InformationTouch lastTouchInfo;

        public float gravity = -9.8f;
        public float jumpForce ;
        public float floatingDragForce;
        private bool isFloatingStarted = false;
        public bool IsFloatingStarted { get { return isFloatingStarted; } set { isFloatingStarted = value; } }

        public Action deathMethod;
        // Start is called before the first frame update
        void Start()
        {
            //Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentState == PlayerState.Jumping || CurrentState == PlayerState.DoubleJumping)
            {
                deltaT += Time.deltaTime;
                applyVelocity();
            }
            else if (CurrentState == PlayerState.Floating)
            {
                applyVelocity();
            }

            if(CurrentState == PlayerState.DoubleJumping && currentVelocity < 0 && isFloatingStarted)
            {
                CurrentState = PlayerState.Floating;
                isFloatingStarted = false;
                sinmpleAnimation.PlayAnimation("Float");
                StartJumporFloat(true);
                //Debug.Log(transform.position);
            }
            if(Input.GetKey(KeyCode.A))
            {
                Debug.Log("position -> "+transform.position);
            }
        }

        private void StartJumporFloat(bool isFloating = false)
        {
            if(!isFloating)
            {
                currentVelocity = jumpForce *PanelController.Instance.GetScaleFactor();
            }
            else
            {
                currentVelocity = -floatingDragForce * PanelController.Instance.GetScaleFactor() ;
            }
            deltaT = 0;
        }
        public void applyVelocity()
        {
            if (CurrentState != PlayerState.dead)
            {
                float nextVelocity = currentVelocity + gravity * deltaT;
                currentVelocity = nextVelocity;
                transform.position = new Vector3(transform.position.x, transform.position.y + currentVelocity *Time.deltaTime);
                if(transform.position.y < startPos.y)
                {
                    transform.position = startPos;
                }
            }
        }

        public void Initialize(Action _deathMethod = null)
        {
            deathMethod = _deathMethod;
            CurrentState = PlayerState.Running;
            rigidBody = transform.GetComponent<Rigidbody2D>();
            sinmpleAnimation = transform.GetComponent<SimpleSpriteAnimator>();
            sinmpleAnimation.PlayAnimation("Run");
            deltaT = 0;
            if (startPos.x != 0 && startPos.y != 0)
            {
                transform.position = startPos;
                Debug.Log("Transform postion -> "+transform.position);
            }
            else
                startPos = transform.position;

            
            //lastTouchInfo.timeOfTouch = 0;
        }
        public void PlayerInstruction(PlayerState doState)
        {
            switch (doState)
            {
                case PlayerState.Jumping:
                    if (CurrentState == PlayerState.Running)
                    {
                        isFloatingStarted = false;
                        sinmpleAnimation.PlayAnimation("Jump");
                        CurrentState = PlayerState.Jumping;
                        StartJumporFloat();
                        //rigidBody.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
                    }
                    else if (CurrentState == PlayerState.Jumping)
                    {
                        isFloatingStarted = true;
                        sinmpleAnimation.PlayAnimation("Jump");
                        CurrentState = PlayerState.DoubleJumping;
                        StartJumporFloat();
                    }
                    break;
                case PlayerState.DoubleJumping:
                    if (CurrentState == PlayerState.Floating)
                    {
                        isFloatingStarted = false;
                        sinmpleAnimation.PlayAnimation("Jump");
                        CurrentState = PlayerState.DoubleJumping;
                        //StartJumporFloat();
                    }
                    break;
            }

            
        }
        public void OnCollisionExit2D(Collision2D collision)
        {
            if (CurrentState == PlayerState.dead)
                return;
            if (collision.collider.CompareTag("Ground"))
            {
                CurrentState = PlayerState.Jumping;
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (CurrentState == PlayerState.dead)
                return;
            if (collision.collider.CompareTag("Ground"))
            {
                //Debug.Log("ground hitted");
                if (CurrentState != PlayerState.Running )
                {
                    deltaT = 0;
                    currentVelocity = 0;
                    transform.position = startPos;
                    IsFloatingStarted = true;
                    CurrentState = PlayerState.Running;
                    sinmpleAnimation.PlayAnimation("Run");
                }
            }
            else if (collision.collider.tag == "Fireball")
            {
                /*FireBall fireBall = collision.gameObject.GetComponent<FireBall>();
                if (fireBall != null && fireBall.isActive)
                {
                    fireBall.isActive = false;
                    fireBall.DestroyFireBall();
                }*/
                PlayerDeathCall();
                
                Destroy(collision.gameObject);
            }
        }
        private void PlayerDeathCall()
        {
            deathMethod?.Invoke();
            CurrentState = PlayerState.dead;
            sinmpleAnimation.PlayAnimation("Dead");
            applyDeathForce();
        }

        private void applyDeathForce()
        {
            /*Vector2 force = new Vector2(-1000, -1000); ;
            rigidBody.AddForce(force,ForceMode2D.Impulse);*/
            Vector2 lastPos = new Vector2(startPos.x-300*PanelController.Instance.GetScaleFactor(),startPos.y);
            transform.DOJump(lastPos,300,1,0.5f,false);
        }

    }
}
