using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Modules
{
    [Serializable]
    public class SpriteAnimation
    {
        public string name;
        [Range(0.01f, 3f)]
        public float speed = 1.0f;
        public bool loop = false;
        public List<string> frames;

        public SpriteAnimation() { }
        public SpriteAnimation(string _name, float _speed, bool _loop, List<string> _frames)
        {
            name = _name;
            speed = _speed;
            loop = _loop;
            frames = _frames;
        }
    }

    [RequireComponent(typeof(Image))]
    public class SimpleSpriteAnimator : MonoBehaviour
    {
        public static SimpleSpriteAnimator instance;
        public SpriteAtlas spriteAtlas;
        public List<SpriteAnimation> animations = new List<SpriteAnimation>();

        private Image image = null;
        private int frameCount = 0;
        private SpriteAnimation currAnimation = null;

        private void Awake()
        {
            instance = this;
        }

        public void AddAnimation(SpriteAnimation _animations)
        {
            animations.Add(_animations);
        }

        [ContextMenu("PlayAnimation")]
        public void PlayAnimation(string animName)
        {
            StopAnimation();
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            foreach (SpriteAnimation animation in animations)
            {
                if (animation.name == animName)
                {
                    currAnimation = animation;
                }
            }
            if (currAnimation == null)
            {
                Debug.LogWarning(gameObject.name + " : " + animName + " Not Found!");
                return;
            }
            StartCoroutine("ChangeFrames");
        }

        public void PausAnimation()
        {
            StopCoroutine("ChangeFrames");
        }
        public void ResumeAnimation()
        {
            StartCoroutine("ChangeFrames");
        }
        public void StopAnimation()
        {
            StopAllCoroutines();
            frameCount = 0;
            currAnimation = null;
        }

        IEnumerator ChangeFrames()
        {
            yield return new WaitForSeconds(0.042f / currAnimation.speed);
            frameCount++;
            if (frameCount >= currAnimation.frames.Count)
            {
                if (currAnimation.loop)
                {
                    frameCount = 0;
                    image.sprite = spriteAtlas.GetSprite(currAnimation.frames[0]);
                    StartCoroutine("ChangeFrames");
                }
                else
                {
                    OnCompleteAnim();
                }
            }
            else
            {
                image.sprite = spriteAtlas.GetSprite(currAnimation.frames[frameCount]);
                StartCoroutine("ChangeFrames");
            }
        }

        public void OnCompleteAnim()
        {
            //PlayAnimation("Run"); 
        }
    }
}
