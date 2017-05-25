using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Circle : NetworkBehaviour
{

	[SyncVar (hook = "OnColorChanged")]
	public int color;

	[SyncVar]
	int colorA;

	[SyncVar]
	int colorB;

	[SyncVar]
	bool swapped;

	[SyncVar]
	public string circleUniqueId;
	private Transform myTransform;

	public GameObject ciao;

	private SpriteRenderer sr;

	Color actualColor;

	// Use this for initialization
	void Start ()
	{
		myTransform = transform;
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		actualColor = GameScript.singleton.getColor (color);
		sr.color = actualColor;
	}
	
	// Update is called once per frame
	void Update ()
	{
		SetIdentity ();
	}

	void SetIdentity()
	{
		if (transform.name == "" || transform.name == "Circle 1(Clone)") {
			transform.name = circleUniqueId;
		}
	}

	[Server]
	public void setColorA (int a)
	{
		this.color = a;
		this.colorA = a;
	}

	[Server]
	public void setColorB (int b)
	{
		this.colorB = b;
	}

	[Server]
	public void Swap ()
	{
		color = color == colorA ? colorB : colorA;
	}

	void OnColorChanged (int color)
	{
		if (isServer) {
			RpcColorChanged (color);
			swapped = true;
		}
	}

	[ClientRpc]
	void RpcColorChanged (int color)
	{
		Debug.Log ("Changed to color: " + color);
		prepareAndDoSplash ();
		actualColor = GameScript.singleton.getColor (color);
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		sr.color = actualColor;
	}
		
	void prepareAndDoSplash ()
	{
		ciao.SetActive (true);
		ciao.transform.localScale = new Vector3 (0.2f, 0.2f, 1f);
		SpriteRenderer ciaoRenderer = ciao.GetComponent<SpriteRenderer> ();
		ciaoRenderer.color = new Color (0.9f, 0.9f, 0.9f, 0.66f);
		ciao.transform.rotation = new Quaternion (0, 0, 0, 1);
		InvokeRepeating ("doSplash", 0, 0.0001f);
	}
		
	void doSplash ()
	{
		float i = 0.1f;
		rotate (i);
		scale (i);
		alpha (i);
	}
//
//	[ClientRpc]
	void rotate (float i)
	{
		Quaternion rot = ciao.transform.rotation;
		if (rot.z < 1) {
			ciao.transform.rotation = new Quaternion (rot.x, rot.y, rot.z + i, rot.w);
		} else {
			ciao.transform.rotation = new Quaternion (rot.x, rot.y, 0, rot.w);
		}
	}

//	[ClientRpc]
	void scale (float i)
	{
		Vector3 scale = ciao.transform.localScale;
		if (ciao.transform.localScale.x <= 1f) {
			ciao.transform.localScale = new Vector3 (scale.x + i, scale.y + i, scale.z);
		}
	}

//	[ClientRpc]
	void alpha (float i)
	{
		SpriteRenderer ciaoRenderer = ciao.GetComponent<SpriteRenderer> ();
		Color temp = ciaoRenderer.color;
		if (temp.a > 0) {
			ciaoRenderer.color = new Color (temp.r, temp.g, temp.b, temp.a - (i / 5));
		} else {
			CancelInvoke ();
			Debug.Log ("Cancel invoke");
			ciao.SetActive (false);
		}
	}

	[ClientRpc]
	public void RpcDestroy ()
	{
		Destroy (gameObject);
	}
}
