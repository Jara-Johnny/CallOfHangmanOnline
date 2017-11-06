using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _PlayersManager : MonoBehaviour {

    #region Properties

    [SerializeField]
    private string[] wordsForSinglePlayer;

    [SerializeField]
    private GameObject wordSelectionMenu;

    [SerializeField]
    private GameObject playerModeMenu;

    [SerializeField]
    private InputField myInputField;

    [SerializeField]
    private Text textOfWordSelection;

    private bool isFirstPlayersSelected;

    [SerializeField]
    private GameObject[] playersMatchs = new GameObject[2];

    private int numOfTurns;

    private int currentTurn;

    #endregion

    #region Unity Functions

    private void Start()
    {
        playersMatchs[0].GetComponent<_Player>().AdActionPresButton(OnEnterChar);
        playersMatchs[1].GetComponent<_Player>().AdActionPresButton(OnEnterChar);
    }

    #endregion

    #region Class Functions

    public void SetupSingleplayer()
    {
        playerModeMenu.SetActive(false);

        numOfTurns = 1;
        playersMatchs[0].GetComponent<_Player>().SetWordOfGame(wordsForSinglePlayer[Random.Range(0, wordsForSinglePlayer.Length)]);
        ActivateMatches();
    }

    public void SetupLocalMultiplayer()
    {
        isFirstPlayersSelected = false;
        playerModeMenu.SetActive(false);
        numOfTurns = 2;
        wordSelectionMenu.SetActive(true);
    }

    public void SetupOnlineMultiplayer()
    {
        //TODO
    }

    private void ActivateMatches()
    {
        for (int i = 0; i < numOfTurns; i++)
        {
            playersMatchs[i].SetActive(true);
        }
    }

    public void SelectWord()
    {
        if (!isFirstPlayersSelected)
        {
            isFirstPlayersSelected = true;
            playersMatchs[0].GetComponent<_Player>().SetWordOfGame(myInputField.textComponent.text);
            myInputField.textComponent.text = "";
            textOfWordSelection.text = "Player 1 close your eyes, Player 2 select a word.";
        }
        else
        {
            playersMatchs[1].GetComponent<_Player>().SetWordOfGame(myInputField.textComponent.text);
            myInputField.textComponent.text = "";
            textOfWordSelection.text = "Player 2 close your eyes, Player 1 select a word.";
            wordSelectionMenu.SetActive(false);

            playersMatchs[0].GetComponent<_Player>().SetLockState(true);
            playersMatchs[1].GetComponent<_Player>().SetLockState(false);

            ActivateMatches();
        }
    }

    public void OnEnterChar()
    {
        switch (numOfTurns)
        {
            case 1:
                break;

            case 2:

                currentTurn++;

                if (currentTurn >= numOfTurns)
                    currentTurn = 0;

                SetupTurn();

                break;

            default:
                break;

        }
    }

    private void SetupTurn()
    {
        switch (currentTurn)
        {
            case 0:
                playersMatchs[0].GetComponent<_Player>().SetLockState(true);
                playersMatchs[1].GetComponent<_Player>().SetLockState(false);
                break;

            case 1:
                playersMatchs[1].GetComponent<_Player>().SetLockState(true);
                playersMatchs[0].GetComponent<_Player>().SetLockState(false);
                break;

            default:
                break;
        }
    }

    #endregion
}
