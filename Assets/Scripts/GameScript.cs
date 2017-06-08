using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameScript : NetworkBehaviour
{

	[SyncVar]
	int online;

	[SyncVar]
	bool inGame;

	int x;
	int y;
	float size = 1.2f;
	int lastColor1 = -1;
	int lastColor2 = -1;
	int randomColors;
	Vector2 initialPos;

	List<string> connectedUsers;

	List<Circle> circles;

	public GameObject circlePrefab;

	static List<Color> colors = new List<Color> ();

	public Text connectedText;

	public static GameScript singleton;

	void Start ()
	{
		initColors ();
		singleton = this;
	}

	void startGame ()
	{
		//Init count of circles and position
		x = Random.Range (2, 5);
		y = Random.Range (2, 5);
		float middleCircle = (size / 2);
		float middleX = (x * size / 2) - middleCircle;
		float middleY = (y * size / 2) - middleCircle;
		initialPos = new Vector2 (-middleX, -middleY);
		// Init 2 colors
		int n1 = -1;
		int n2 = -1;
		while (true) {
			int i = Random.Range (0, colors.Count - 1);
			if (i != lastColor1 && i != lastColor2) {
				if (n1 == -1) {
					n1 = i;
					lastColor1 = n1;
				} else {
					n2 = i;
					lastColor2 = n2;
					break;
				}
			}
		}
		initCircles (n1, n2);
	}

	void initCircles (int color1, int color2)
	{
		int cantTotal = x * y;
		Debug.Log ("Cantidad Total: " + cantTotal);
		int random1 = (cantTotal / 2) - 1;
		int random2 = (cantTotal / 2) + 1;
		Debug.Log ("Random1: " + random1);
		Debug.Log ("Random2: " + random2);
		randomColors = Random.Range (random1, random2 + 1);

		List<Vector2> randomPos = getRandomPos (randomColors, x, y);

		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				Vector2 pos = new Vector2 ((i * size) + initialPos.x, (j * size) + initialPos.y);
				GameObject o = (GameObject)Instantiate (circlePrefab, pos, Quaternion.identity);
				Circle c = o.GetComponent<Circle> ();
				Vector2 position = new Vector2 (i, j);
				if (randomPos.Contains (position)) {
					setColors (color1, color2, c);
				} else {
					setColors (color2, color1, c);
				}
				circles.Add (c);
				c.circleUniqueId = "Circle " + circles.Count;
				NetworkServer.Spawn (o);
			}
		}
	}

	List<Vector2> getRandomPos (int random, int x, int y)
	{
		List<Vector2> result = new List<Vector2> ();
		while (random > 0) {
			result.Add (new Vector2 (Random.Range (0, x), Random.Range (0, y)));
			random--;
		}
		return result;
	}

	void setColors(int color1, int color2, Circle c)
	{
		c.setColorA (color1);
		c.setColorB (color2);
	}

	static Color rgba (int r, int g, int b)
	{
		return new Color ((float)r / 255, (float)g / 255, (float)b / 255, 0.66f); 
	}

	public override void OnStartServer ()
	{
		circles = new List<Circle> ();
		connectedUsers = new List<string> ();
	}

	void Awake ()
	{
		singleton = this;
		online = 0;
	}

	static void initColors ()
	{
		colors.Add (rgba (244, 67, 54)); //red
		colors.Add (rgba (156, 39, 176)); //purple
		//		colors.Add (rgba(63,81,181)); // indigo
		colors.Add (rgba (33, 150, 243)); // blue
		colors.Add (rgba (76, 175, 80)); //green
		colors.Add (rgba (255, 235, 59)); //yellow
		colors.Add (rgba (255, 152, 0)); //orange
	}

	void updateConnectedUsers (string name)
	{
		connectedText.text = "CONNECTED USERS: " + name;
	}
		
	
	// Update is called once per frame
	[ServerCallback]
	void Update ()
	{
		if (isServer) {
			if (NetworkManager.singleton.numPlayers > online) {
				PlayerScript[] players = FindObjectsOfType<PlayerScript> ();
				if (AllReadys (players)) {
					online = NetworkManager.singleton.numPlayers;
					if (!inGame) {
						CmdStartGame ();
					}
				}
			}
		}
	}

	bool AllReadys (PlayerScript[] players)
	{
		foreach (PlayerScript p in players) {
			if (!p.ready) {
				return false;
			}
		}
		return true;
	}

	string getStringList (List<string> connectedUsers)
	{
		string result = "";
		for (int i = 0; i < connectedUsers.Count; i++) {
			result = result + connectedUsers [i];
			if (i != connectedUsers.Count - 1) {
				result = result + ", ";
			}
		}
		Debug.Log (result);
		return result;
	}

	public void AddUser (string user)
	{
		connectedUsers.Add (user);
		RpcConnectedUsers (getStringList (connectedUsers));
	}

	[Command]
	void CmdStartGame ()
	{
		inGame = true;
		startGame ();
	}

	[ClientRpc]
	void RpcConnectedUsers (string list)
	{
		updateConnectedUsers (list);
	}

	public Color getColor (int i)
	{
		return colors [i];
	}
		
	[Server]
	public void CheckWin() 
	{
		if (sameColor ()) {
			nextLevel ();
		}
	}

	[Server]
	public void nextLevel ()
	{
//		flash ();
		foreach (Circle o in circles) {
			o.RpcDestroy ();
		}
		circles = new List<Circle> ();
		startGame ();
	}

	[Server]
	public void finish ()
	{
		// flash ();
		foreach (Circle o in circles) {
			o.RpcDestroy ();
		}
		inGame = false;
		// Show game over

	}

	private bool sameColor ()
	{
		Color color = colors[circles [0].color];
		for (int i = 1; i < circles.Count; i++) {
			if (color != colors[circles [i].color]) {
				return false;
			}
		}
		return true;
	}

	[Server]
	public bool isInGame()
	{
		return inGame;
	}
}
