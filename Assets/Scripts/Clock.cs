using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Clock : NetworkBehaviour
{

	[SyncVar]
	float time = 15;

	Text timeText;

	GameScript game;


	int initialTime = 15;


	// Use this for initialization
	void Start ()
	{
	}

	void OnStartServer ()
	{
		time = initialTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isServer) {
			if (game == null) {
				game = FindObjectOfType<GameScript> ();
			}
			if (game.isInGame ()) {
				time -= Time.deltaTime;
				if (time < 0) {
					game.finish ();
					time = 0;
				}
				if (timeText == null) {
					timeText = GetComponent<Text> ();
				}
				timeText.text = "TIME: " + time.ToString ("F");
			}
		}
	}

}
