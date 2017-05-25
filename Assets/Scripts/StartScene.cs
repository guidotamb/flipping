using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScene : MonoBehaviour
{

	public Text username;

	private OverriddenNetworkDiscovery nd;
	void Start ()
	{
		nd = GetComponent<OverriddenNetworkDiscovery> ();
		nd.Initialize ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void play ()
	{
		//SceneManager.LoadScene ("ColorTap");
	}

	public void host ()
	{
		PlayerPrefs.SetString ("UserName", username.text);
		NetworkManager.singleton.StartHost ();
		nd.StartAsServer ();
	}

	public void joinGame ()
	{	
		PlayerPrefs.SetString ("UserName", username.text);
		nd.StartAsClient ();
	}
		
}
