using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManagerNetworking : MonoBehaviour
{
    [SerializeField]
    private PlayerNetworking playerPrefab;

    [SerializeField]
    private PlayerAI playerAIPrefab;

    public int gameMode // 0. No game selected, 1. Singleplayer, 2. Local Multiplayer, 3. Online Multiplayer
    {
        get; private set;
    }

    public int turn
    {
        get; private set;
    }

    public int semiTurn
    {
        get; private set;
    }

    public int playerInTurn // 0. Player 1, 1. Player 2
    {
        get; private set;
    }

    public int localPlayerIndentity;
 

    public PlayerNetworking[] players
    {
        get; private set;
    }
    //Players indexer
    public PlayerNetworking this[int i]
    {
        get
        {
            return players[i];
        }
        private set
        {
            players[i] = value;
        }
    }

    //Singleton!
    public static GameManagerNetworking Singleton
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

    private void Start()
    {
        //Subscribing to the game modes
        Observer.Singleton.onSingleplayer += SetupSingleplayer;
        Observer.Singleton.onLocalMultiplayer += SetupLocalMultiplayer;
        Observer.Singleton.onOnlineMultiplayer += SetupOnlineMultiplayer;
        //Subscribing to the word input field events
        Observer.Singleton.onWordInputFieldEnter += SetPlayerWord;
        Observer.Singleton.onWordInputFieldEnter += NextTurn;
        //Subscribing to the letter input field events
        Observer.Singleton.onLetterInputFieldEnter += CheckForCharOnRivalPlayerWord;
        Observer.Singleton.onLetterInputFieldEnter += NextTurn;
    }

    public void SetupSingleplayer()
    {
        gameMode = 1;

        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.SetActiveSingleplayer(true);

        CreatePlayerWithAI();
    }

    public void SetupLocalMultiplayer()
    {
        gameMode = 2;

        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.SetActiveLocalMultiplayer(true);

        CreatePlayers();
    }

    public void SetupOnlineMultiplayer()
    {
        gameMode = 3;

        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.SetActiveOnlineMultiplayer(true);
    }

    public void SetHostPlayerOnline(GameObject otherPlayer)
    { 

       

        if (players == null)
            players = new PlayerNetworking[2];

        players[0] = otherPlayer.GetComponent<PlayerNetworking>();
        players[0].SetIndex(0);

        if(players[0]==null)
            Debug.Log("is null");
        else
            Debug.Log("isnt null");
    }

    public void SetPlayerTwoClient(GameObject otherPlayer)
    {
   
        players[1] = otherPlayer.GetComponent<PlayerNetworking>();
        players[1].SetIndex(1); 
    

        gameMode = 2;

        UIFacade.Singleton.SetActiveOnlineMultiplayer(false);
        UIFacade.Singleton.SetActiveLocalMultiplayer(true);

    }

    private void CreatePlayers()
    {
        if (players == null)
            players = new PlayerNetworking[2];

        players[0] = Instantiate(playerPrefab) as PlayerNetworking;
        players[0].SetIndex(0);
        players[1] = Instantiate(playerPrefab) as PlayerNetworking;
        players[1].SetIndex(1);

        players[1].gameObject.SetActive(false);
    }

    private void CreatePlayerWithAI()
    {
        if (players == null)
            players = new PlayerNetworking[2];

        players[0] = Instantiate(playerPrefab) as PlayerNetworking;
        players[0].SetIndex(0);

        PlayerAI playerAI = Instantiate(playerAIPrefab) as PlayerAI;

        players[1] = playerAI;
        players[1].SetIndex(1);

        players[1].gameObject.SetActive(false);
    }

    private void SetPlayerWord()
    {
       /*if(!isServer)
            return;*/
        Debug.Log("Local player index "+ localPlayerIndentity);
        players[localPlayerIndentity].SetWord(UIFacade.Singleton.currentInputFieldText);

        /*
        UIFacade.Singleton.SetActiveLocalMultiplayerScreen(0, false);
        UIFacade.Singleton.SetActiveLocalMultiplayerScreen(1, true);
         */
            
    
    }

    private void CheckForCharOnRivalPlayerWord()
    {
      /* if(!isServer)
            return;*/

        int otherPlayerIndex = (playerInTurn == 0) ? 1 : 0;

        Dictionary<int, char> correctChars = 
            players[otherPlayerIndex].CheckForCharsInWord(UIFacade.Singleton.currentInputFieldText[0]);

        if (correctChars.Count == 0)
        {
            players[playerInTurn].IncreaseErrorsCount();

            switch (playerInTurn)
            {
                case 0:
                    UIFacade.Singleton.playerOneErrors.text = string.Format("Player 1 Errors: {0}/10", players[0].errorsCount);
                    break;

                case 1:
                    UIFacade.Singleton.playerTwoErrors.text = string.Format("Player 2 Errors: {0}/10", players[1].errorsCount);
                    break;

                default:
                    break;
            }

            return;
        }

        if (players[playerInTurn].errorsCount >= 10)
        {
            Debug.Log("You Lose");
        }

        for (int i = 0; i < players[otherPlayerIndex].wordCharsArray.Length; i++)
        {
            if (correctChars.ContainsKey(i))
            {
                players[playerInTurn].IncreaseSuccessCount();

                switch (otherPlayerIndex)
                {
                    case 0:
                        UIFacade.Singleton.playerOneEmptyTexts[i].text = players[0].wordCharsArray[i].ToString();
                        break;

                    case 1:
                        UIFacade.Singleton.playerTwoEmptyTexts[i].text = players[1].wordCharsArray[i].ToString();
                        break;

                    default:
                        break;
                }
            }
        }

        if (players[playerInTurn].sucessCount == 
            players[otherPlayerIndex].wordCharsArray.Length)
        {
            Debug.Log("You Win");
        }
    }

    private void NextTurn()
    {
       /*if(!isServer)
            return;*/

        //turn ++;
        semiTurn++;

        if (playerInTurn == 1)
            Observer.Singleton.PlayerOneEndsTurn();
        else
            Observer.Singleton.PlayerTwoEndsTurn();

        playerInTurn = localPlayerIndentity;
    }

    public void SetLocalPlayerSettings(int index)
    {
        turn = index;
        localPlayerIndentity = index;
    }
}
