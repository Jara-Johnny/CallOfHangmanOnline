using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class PlayerNetworking : NetworkBehaviour {


    public int index;
    

    public string word;
    

    public int sucessCount
    {
        get; private set;
    }

    public int errorsCount
    {
        get; private set;
    }

    public char[] wordCharsArray;

    public List<char> charsSelected = new List<char>();
    /*{
        get; private set;
    }*/
    //Word chars indexer
    public char this[int i]
    {
        get
        {
            return wordCharsArray[i];
        }
        private set
        {
            wordCharsArray[i] = value;
        }
    }

    public override void OnStartClient(){
        base.OnStartClient();
    }


    private void Start()
    {   
        
        if(isServer)
        {
            SetUpServerPlayers();
            
        }else
        {
           SetUpClientPlayers();
        }
        
    }  


    void SetUpServerPlayers()
    {
         if(isLocalPlayer)
            {
                GameManagerNetworking.Singleton.SetHostPlayerOnline(gameObject);
            }
            else
            {
                GameManagerNetworking.Singleton.SetPlayerTwoClient(gameObject);
            }
    }


    public void SetUpClientPlayers()
    {
         if(isLocalPlayer)
            {
                GameManagerNetworking.Singleton.SetPlayerTwoClient(gameObject);
            }
            else
            {
                GameManagerNetworking.Singleton.SetHostPlayerOnline(gameObject);
            }
    }

    [Command]
    public void CmdUpdateIndex(int index)
    {
        RpcUpdateIndex(index);
    }

    [ClientRpc]
    public void RpcUpdateIndex(int index)
    {
        this.index = index;
    }
   
    public void SetIndex(int index)
    {
       CmdUpdateIndex(index);
        if(isLocalPlayer)
        {
            GameManagerNetworking.Singleton.SetLocalPlayerSettings(index);
        }
        

        switch (index)
        {
            case 0:
                Observer.Singleton.onPlayerTwoEndsTurn += Turn;
                Observer.Singleton.onPlayerOneEndsTurn += EndTurn;
                break;
            case 1:
                Observer.Singleton.onPlayerOneEndsTurn += Turn;
                Observer.Singleton.onPlayerTwoEndsTurn += EndTurn;
                break;
            default:
                return;
        }

        Debug.Log(string.Format("Player {0} created!", index));

        

    }

    [Command]
    public void CmdSetWord(string word)
    {
        RpcSetWord(word);
    }

    [ClientRpc]
    public void RpcSetWord(string word)
    {
        Debug.Log("the word is "+word);

        this.word = word;
        wordCharsArray = word.ToCharArray();

        switch (index)
        {
            case 0:
                for (int i = UIFacade.Singleton.playerOneEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                    UIFacade.Singleton.playerOneEmptyTexts[i].gameObject.SetActive(false);
                break;

            case 1:
                for (int i = UIFacade.Singleton.playerTwoEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                    UIFacade.Singleton.playerTwoEmptyTexts[i].gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    public IEnumerator ChekForWordsInPlayers ()
    {
     while(true)
     {
         if(GameManagerNetworking.Singleton.players[0].word!=""&&GameManagerNetworking.Singleton.players[1].word!="")
        {
            UIFacade.Singleton.SetActiveLocalMultiplayerScreen(0, false);
            UIFacade.Singleton.SetActiveLocalMultiplayerScreen(1, true);
            break;
        }
         yield return new WaitForSeconds(1f);
     }
    }

    public void SetWord(string word)
    {


        CmdSetWord(word);

        if(isLocalPlayer)
            StartCoroutine(ChekForWordsInPlayers());

        Debug.Log(string.Format("Player {0} word: {1}", index, word));

    }

    


    public Dictionary<int, char> CheckForCharsInWord(char inputChar)
    {
        Dictionary<int, char> charsInWord = new Dictionary<int, char>();

        for (int i = 0; i < wordCharsArray.Length; i++)
        {
            if (inputChar == wordCharsArray[i])
            {
                charsInWord.Add(i, wordCharsArray[i]);
                //wordCharsArray[i] = '';
            }
                
        }

        return charsInWord;
    }
    

    public void IncreaseSuccessCount()
    {
        sucessCount++;
    }

    
    public void IncreaseErrorsCount()
    {
        errorsCount++;
    }

    
    private void Turn()
    {
        //gameObject.SetActive(true);


        if (GameManagerNetworking.Singleton.semiTurn > 1)
        {
            switch (index)
            {
                case 0:
                    UIFacade.Singleton.playersWords[1].SetActive(true);

                    
                    break;

                case 1:
                    UIFacade.Singleton.playersWords[0].SetActive(true);

     
                    break;

                default:
                    break;
            }
        }
    }

    
    private void EndTurn()
    {
        //gameObject.SetActive(false);

        if (GameManagerNetworking.Singleton.semiTurn > 1)
        {
            switch (index)
            {
                case 0:
                    UIFacade.Singleton.playersWords[1].SetActive(false);
                    break;

                case 1:
                    UIFacade.Singleton.playersWords[0].SetActive(false);
                    break;

                default:
                    break;
            }
        }
    }


}
