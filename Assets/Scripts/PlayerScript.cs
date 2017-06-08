using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{

	[SyncVar]
	public string userName;

	[SyncVar]
	public bool ready;

	// Use this for initialization
	void Awake ()
	{
	}

	public override void OnStartLocalPlayer ()
	{
		CmdUserName (PlayerPrefs.GetString ("UserName").ToUpper ());
		CmdSetReady (true);
	}

	public string getUserName ()
	{
		return userName;
	}

	[Command]
	void CmdUserName (string str)
	{
		userName = str;
		GameObject.FindObjectOfType<GameScript> ().AddUser (userName);
	}

	[Command]
	void CmdSetReady (bool ready)
	{
		this.ready = ready;
	}

	[Command]
	void CmdCircleClicked (string uniqueId)
	{
		GameObject circleObject = GameObject.Find (uniqueId);
		circleObject.GetComponent<Circle> ().Swap ();
		GameObject.FindObjectOfType<GameScript> ().CheckWin ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer) {
			return;
		}
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				Vector3 touchPos = Camera.main.ScreenToWorldPoint (touch.position);
				RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
				if (hit.collider != null) {
					if (hit.collider.gameObject.tag == "Circle") {
						CmdCircleClicked (hit.collider.name);
					}

				}
			}
		}
	}
}
