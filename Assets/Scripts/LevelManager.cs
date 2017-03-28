// agora precisa mudar todo o controle dos bushs para um script e usar o bg para criar o paralax 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public PlayerController _player;
	public GameObject _hidrante;
	public float hidranteSpeed = 0.2f;
	public float maxBushes;
	private GameObject instanced;
	public GameObject _bush;
	private GameObject instancedBush;
	public Camera _camera;
	private List<GameObject> lstBushInstance;
	public Transform bushOverlapCheck;
	public LayerMask whatIsBush;

	// Use this for initialization
	void Start ()
	{
		_player = FindObjectOfType<PlayerController>();
		_camera = FindObjectOfType<Camera>();
		lstBushInstance = new List<GameObject>();        
	}

	// Update is called once per frame
	void Update ()
	{
		HidranteManager();
		BushManager();
	}

	void HidranteManager()
	{
		if (instanced == null)
			instanced = Instantiate(_hidrante, _player.transform.position + new Vector3(20f, 0, 0), transform.rotation);
		else
			instanced.transform.position = new Vector3(instanced.transform.position.x - 0.2f, instanced.transform.position.y, instanced.transform.position.z);

		if (!IsOnScreen(instanced))
		{
			Destroy(instanced);
			instanced = null;
		}
	}
	void BushManager()
	{
		if (lstBushInstance.Count < maxBushes)
		{                                    
			instancedBush = Instantiate(_bush, new Vector3(20f, -2f, 0), transform.rotation);


			var objectsWithTag = GameObject.FindGameObjectsWithTag("Bush");

			foreach (var obj in objectsWithTag)
			{

				Debug.Log("Distance" + Vector3.Distance(obj.transform.position, instancedBush.transform.position));
				Debug.Log("Bounds" + _bush.GetComponent<Renderer>().bounds.size.x + " / " + _bush.GetComponent<Renderer>().bounds.size.y);

				if (Vector3.Distance(obj.transform.position, instancedBush.transform.position) <= instancedBush.GetComponent<Renderer>().bounds.size.x + 5)
				{
					instancedBush.transform.position = new Vector3(instancedBush.transform.position.x + 8f, instancedBush.transform.position.y, instancedBush.transform.position.z);
				}
			}

			if(instanced != null)
				lstBushInstance.Add(instancedBush);
		}              

		foreach (var bush in lstBushInstance)
		{
			if (!IsOnScreen(bush))
			{
				lstBushInstance.Remove(bush);
				Destroy(bush);

			}
			else
			{
				bush.transform.position = new Vector3(bush.transform.position.x - 0.03f, bush.transform.position.y, instancedBush.transform.position.z);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Bush")
		{
			Debug.Log("bush enter something");
			lstBushInstance.Remove(instancedBush);
			Destroy(instancedBush);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Bush")
		{
			Debug.Log("bush enter something");
		}
	}

	bool IsOnScreen(GameObject obj)
	{
		if (obj == null)
			return false;


		bool onScreen = true;

		var horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

		Debug.Log(obj.transform.position.x + " / " + horzExtent);

		if (obj.transform.position.x < (horzExtent * -1))
		{            
			onScreen = false;
			//Destroy(obj);
			//obj = null;
		}

		return onScreen;
	}    
}