using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class _Player : MonoBehaviour {

    #region Properties

    private string wordToShow;

    private string currentWord;

    [SerializeField]
    private InputField myInputField;

    [SerializeField]
    private Text textWordToShow;

    [SerializeField]
    private int numOfTrys;

    [SerializeField]
    private Button myButton;

    private bool isActiveInteractable;

    private event Action OnButtonDown;

    #endregion

    #region Unity Functions

    private void Start()
    {
        SetSpaceNewWord(currentWord);

        isActiveInteractable = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("ENTER") && isActiveAndEnabled)
        {
            CheckCharInWord();
        }
    }

    #endregion

    #region Class Functions

    public void SetSpaceNewWord(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            wordToShow += "_";

        }

        textWordToShow.text = wordToShow;
    }

    public void CheckCharInWord()
    {
        if (OnButtonDown != null)
            OnButtonDown();

        bool tempBool = false;

        for (int i = 0; i < currentWord.Length; i++)
        {
            if (currentWord.Substring(i, 1) == myInputField.textComponent.text)
            {
                tempBool = true;
                ChangeChar(i, myInputField.textComponent.text);
            }

        }

        if (!tempBool)
        {
            numOfTrys--;

            if (numOfTrys <= 0)
            {
                Debug.Log("Player Lose");
            }
        }

    }

    public void CheckWin()
    {
        for (int i = 0; i < wordToShow.Length; i++)
        {
            if (wordToShow.Substring(i, 1) == "_")
                return;
        }

        Debug.Log("Player Win");
    }

    public void ChangeChar(int targetIndex, string charToChange)
    {
        string tempString = "";

        for (int i = 0; i < currentWord.Length; i++)
        {
            if (i != targetIndex)
            {
                tempString += wordToShow.Substring(i, 1);
            }
            else
            {
                tempString += charToChange;
            }
        }

        wordToShow = tempString;
        textWordToShow.text = tempString;
        CheckWin();
    }

    public void SetLockState(bool lockState)
    {
        myInputField.interactable = lockState;
        isActiveInteractable = lockState;
        myButton.interactable = lockState;
    }

    public void SetWordOfGame(string word)
    {
        currentWord = word;
    }

    public void AdActionPresButton(Action newAction)
    {
        if (OnButtonDown == null)
            OnButtonDown += newAction;
    }

    #endregion
}
