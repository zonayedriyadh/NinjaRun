using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CloudOnce;
#if UNITY_ANDROID
using GooglePlayGames;
#elif UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif
using UnityEngine.SocialPlatforms;
using System;


public class CloudOnceServices
{
    private static CloudOnceServices instance;
    private int scoreToPost = 0;

#if UNITY_IOS
    public static string HighScore = "com.riseuplabs.wfdgame.promode";
#elif UNITY_ANDROID
    public static string HighScore = "CgkIyr3017wJEAIQAQ"; 
#endif

    public string currentLeaderBoard = string.Empty;

    public delegate void OnSignIn(bool isSigned);
    public OnSignIn onSignIn;

    public static CloudOnceServices Instance
    {
        get
        {
            if (instance == null)
            {
                instance = CloudOnceServices.Create();
            }
            return instance;
        }
    }

    public static CloudOnceServices Create()
    {
        CloudOnceServices ret = new CloudOnceServices();

        if (ret != null && ret.Init())
        {
            return ret;
        }
        return null;
    }

    private bool Init()
    {
        return true;
    }

    public void Initialize(bool isMessageShowed = true)
    {
        if (isMessageShowed)
            SubscribeEvent(OnInitialize);
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
        //Social.localUser.Authenticate(OnSignedInChanged);
        Social.localUser.Authenticate((bool isSign) => {
            onSignIn?.Invoke(isSign);
        });
#elif UNITY_IOS
        //Social.localUser.Authenticate(OnSignedInChanged);
        Social.localUser.Authenticate((bool isSign)=> {
            onSignIn?.Invoke(isSign);
        });
#endif
    }

    public void SubmitToLeaderBoard(int score, string _leaderBoard)
    {

#if UNITY_IOS
        string leaderboardID = _leaderBoard;
        //Debug.Log("Reporting score " + score + " on leaderboard " + leaderboardID);
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, leaderboardID, ScoreSubmited);
            //((GameCenterPlatform)Social.Active).ReportScore(score, leaderboardID, ScoreSubmited);
        }
            
        else
        {
            scoreToPost = score;
            currentLeaderBoard = _leaderBoard;
            SubscribeEvent(ReSubmitScore);
            Initialize(false);
        }
#elif UNITY_ANDROID
        if (Social.localUser.authenticated)
            ((PlayGamesPlatform)Social.Active).ReportScore((long)score, _leaderBoard, ScoreSubmited);
        else
        {
            scoreToPost = score;
            currentLeaderBoard = _leaderBoard;
            SubscribeEvent(ReSubmitScore);
            Initialize(false);
        }
#endif
    }
    public void ScoreSubmited(bool isSuccess)
    {
        //Debug.Log("Submited score-> " + isSuccess);
    }

    public void ShowLeaderBoard(string leaderBoard )
    {
        currentLeaderBoard = leaderBoard;
#if UNITY_IOS
        if (Social.localUser.authenticated)
            GameCenterPlatform.ShowLeaderboardUI(leaderBoard, TimeScope.AllTime);
        else
        {
            SubscribeEvent(ReLoadLeaderBoard);
            Initialize(true);
        }
#elif UNITY_ANDROID
        if (Social.localUser.authenticated)
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderBoard);
        else
        {
            SubscribeEvent(ReLoadLeaderBoard);
            Initialize(true);
        }

#endif
    }
    public void ShowLeaderBoard()
    {
        currentLeaderBoard = string.Empty;
#if UNITY_IOS
        if (Social.localUser.authenticated)
            Social.ShowLeaderboardUI();
        else
        {
            SubscribeEvent(ReLoadLeaderBoard);
            Initialize(true);
        }
#elif UNITY_ANDROID
        if (Social.localUser.authenticated)
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
        else
        {
            SubscribeEvent(ReLoadLeaderBoard);
            Initialize(true);
        }

#endif
    }


    private void OnInitialize(bool isSucess)
    {

    }
    private void SubscribeEvent(OnSignIn CallBack)
    {
        UnSubscribeEvent();
        onSignIn += CallBack;
    }

    private void UnSubscribeEvent()
    {
        //onSignIn -= OnSignedInChanged;
        Delegate[] observerList = onSignIn?.GetInvocationList();
        if(observerList != null && observerList.Length !=0)
        {
            foreach (var d in observerList)
                onSignIn -= (d as OnSignIn);
        }
        
    }

    private void ReLoadLeaderBoard(bool isSignedIn)
    {
        UnSubscribeEvent();
        if (isSignedIn)
        {
            if (currentLeaderBoard == string.Empty)
            {
                ShowLeaderBoard();
            }
            else
            {
                ShowLeaderBoard(currentLeaderBoard);
            }

            currentLeaderBoard = string.Empty;
        }
    }

    private void ReSubmitScore(bool isSignedIn)
    {
        UnSubscribeEvent();
        if (isSignedIn)
        {
            if (CloudOnceServices.Instance.scoreToPost != 0 && currentLeaderBoard != string.Empty)
            {
                SubmitToLeaderBoard(CloudOnceServices.Instance.scoreToPost, currentLeaderBoard);
            }

            CloudOnceServices.Instance.scoreToPost = 0;
            currentLeaderBoard = string.Empty;
        }
    }
}
