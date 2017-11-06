using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour {

    //Actions
    public Action onSingleplayer;
    public Action onLocalMultiplayer;
    public Action onOnlineMultiplayer;
    public Action onReadme;

    public Action onPlayerOneEndsTurn;
    public Action onPlayerTwoEndsTurn;

    public Action onWordInputFieldEnter;
    public Action onLetterInputFieldEnter;

    //Singleton!
    public static Observer Singleton
    {
        get; private set;
    }

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;
    }

    public void Singleplayer()
    {
        Debug.Log("Singleplayer");

        if (onSingleplayer != null)
            onSingleplayer();
    }

    public void LocalMultiplayer()
    {
        Debug.Log("LocalMultiplayer");

        if (onLocalMultiplayer != null)
            onLocalMultiplayer();
    }

    public void OnlineMultiplayer()
    {
        Debug.Log("OnlineMultiplayer");

        if (onOnlineMultiplayer != null)
            onOnlineMultiplayer();
    }

    public void Readme()
    {
        Debug.Log("Readme");

        if (onReadme != null)
            onReadme();
    }

    public void PlayerOneEndsTurn()
    {
        Debug.Log("PlayerOneEndsTurn");

        if (onPlayerOneEndsTurn != null)
            onPlayerOneEndsTurn();
    }

    public void PlayerTwoEndsTurn()
    {
        Debug.Log("PlayerTwoEndsTurn");

        if (onPlayerTwoEndsTurn != null)
            onPlayerTwoEndsTurn();
    }

    public void WordInputFieldEnter()
    {
        if (onWordInputFieldEnter != null)
            onWordInputFieldEnter();
    }

    public void LetterInputFieldEnter()
    {
        if (onLetterInputFieldEnter != null)
            onLetterInputFieldEnter();
    }
}
