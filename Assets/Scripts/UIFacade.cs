using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIFacade : MonoBehaviour {

    [Header("Menus")]

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject modeSingleplayer;
    [SerializeField]
    private GameObject modeLocalMultiplayer;
    [SerializeField]
    private GameObject modeOnlineMultiplayer;

    [Space(10F)]

    [SerializeField]
    private GameObject[] singleplayerScreens;
    [SerializeField]
    private GameObject[] localMultiplayerScreens;
    [SerializeField]
    private GameObject[] onlineMultiplayerScreens;

    [Header("Input Fields")] [Space(10f)]

    [SerializeField]
    private InputField wordInputField;
    [SerializeField]
    private InputField letterInputField;

    [Header("Player Text")] [Space(10f)]

    public GameObject[] playersWords;
    public Text[] playerOneEmptyTexts;
    public Text[] playerTwoEmptyTexts;

    [Header("Others")] [Space(10f)]

    public Text localMultiplayerInfo;
    public Text playerOneErrors;
    public Text playerTwoErrors;

    [Header ("WinScreen")] [Space(10f)]

    public Text[] playersErros;
    public Text[] playersWordsWinScreen;

    public GameObject winScreen;

    public GameObject onlineWaitingScreen;

    [HideInInspector]
    public string currentInputFieldText;

    //Singleton!
    public static UIFacade Singleton
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
        wordInputField.characterLimit = 10;
        wordInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateChar(addedChar);
        };

        letterInputField.characterLimit = 1;
        letterInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateChar(addedChar);
        };

        //Subscribing to the input field events
        Observer.Singleton.onWordInputFieldEnter += ClearInputFields;
        Observer.Singleton.onLetterInputFieldEnter += ClearInputFields;
    }

    public void SetActiveMainMenu(bool state)
    {
        mainMenu.SetActive(state);
    }

    public void SetActiveSingleplayer(bool state)
    {
        modeSingleplayer.SetActive(state);
    }

    public void SetActiveLocalMultiplayer(bool state)
    {
        modeLocalMultiplayer.SetActive(state);
    }

    public void SetActiveOnlineMultiplayer(bool state)
    {
        modeOnlineMultiplayer.SetActive(state);
    }

    public void SetActiveSingleplayerScreen(int screen, bool state)
    {
        if (screen < 0 || screen > singleplayerScreens.Length - 1)
            return;

        singleplayerScreens[screen].SetActive(state);
    }

    public void SetActiveLocalMultiplayerScreen(int screen, bool state)
    {
        if (screen < 0 || screen > localMultiplayerScreens.Length - 1)
            return;

        localMultiplayerScreens[screen].SetActive(state);
    }

    public void SetActiveOnlineMultiplayerScreen(int screen, bool state)
    {
        if (screen < 0 || screen > onlineMultiplayerScreens.Length - 1)
            return;

        onlineMultiplayerScreens[screen].SetActive(state);
    }

    public void SetActiveOnlineWaitingScreen(bool state)
    {
        onlineWaitingScreen.SetActive(state);
    }

    public void OnWordInputFieldEndEdit(string value)
    {
        currentInputFieldText = value;
    }

    public void OnWordInputFieldValueChanged(string value)
    {
        currentInputFieldText = value.ToUpper();
    }

    public void OnLetterInputFieldEndEdit(string value)
    {
        currentInputFieldText = value;
    }

    public void OnLetterInputFieldValueChanged(string value)
    {
        currentInputFieldText = value.ToUpper();
    }

    public void ClearInputFields()
    {
       /* wordInputField.text = "";
        letterInputField.text = "";*/
    }

    public void UpdateWinScreen()
    {   
        modeLocalMultiplayer.SetActive(false);
        winScreen.SetActive(true);

        playersErros[0].text = string.Format("Player 1 errors: {0}/10",GameManagerNetworking.Singleton.players[0].errorsCount);
        playersErros[1].text = string.Format("Player 2 errors: {0}/10",GameManagerNetworking.Singleton.players[1].errorsCount);

        playersWordsWinScreen[0].text = string.Format("{0}",GameManagerNetworking.Singleton.players[0].word);
        playersWordsWinScreen[1].text = string.Format("{0}",GameManagerNetworking.Singleton.players[1].word);

    }

    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }

    private char ValidateChar(char charToValidate)
    {
        if (!char.IsLetter(charToValidate) && !char.IsWhiteSpace(charToValidate))
            charToValidate = ' ';

        return char.ToUpper(charToValidate);
    }
 
}
