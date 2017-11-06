using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerSetUp : MonoBehaviour {

	public CustomNetworkManager myNetworkManager;

	public GameObject OnlineMultiplayerCanvas;
	
	void Start()
	{
		Observer.Singleton.onOnlineMultiplayer += ModeSelected;
	}

	public void ModeSelected()
	{
	
	}

	public void StartHostingMadafaca()
	{
		myNetworkManager.StopClient();
		myNetworkManager.StartHost();

		OnlineMultiplayerCanvas.SetActive(false);
	}


	public void StartBeingClient()
	{
		
		myNetworkManager.StopHost();
		myNetworkManager.StopClient();
		myNetworkManager.StartClient();

		OnlineMultiplayerCanvas.SetActive(false);

		//UIFacade.Singleton.SetActiveLocalMultiplayer(false);
		//UIFacade.Singleton.SetActiveOnlineMultiplayerScreen(1,true);
		
	}


}
