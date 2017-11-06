using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	public Vector3 startPosition;

	public GameObject[] players = new GameObject[2];

	public GameObject[] thingsToCreat;

	public GameObject onlineGameManagerPrefab;

	public MultiplayerSetUp myMultiplayerSetUp;

	public override void OnServerConnect(NetworkConnection conn)
	{
		Debug.Log("Player was conected, "+numPlayers+" connected");
		

	}

	

}
