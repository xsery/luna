using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{

    public GameObject _objToMove;
    public float maxObjCount;
    private GameObject instancedObj;
    private List<GameObject> lstObjInstance;
    public Camera _camera;
    public string objTag;
    public float spawnHeight;
    public float spawnWidth;
    public float objDistance;
    public float objSpeed;

    // Use this for initialization
    void Start ()
    {
        lstObjInstance = new List<GameObject>();
        _camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update ()
    {
        ObjectManager();
	}

    void ObjectManager()
    {
        if (lstObjInstance.Count < maxObjCount)
        {
            if (lstObjInstance.Count <= 0)
                instancedObj = Instantiate(_objToMove, new Vector3(spawnWidth, spawnHeight, 0), transform.rotation);
            else
            {
                instancedObj = Instantiate(_objToMove, new Vector3(lstObjInstance[lstObjInstance.Count -1].GetComponent<Renderer>().bounds.size.x + objDistance, spawnHeight, 0), transform.rotation);
            }

            
            Debug.Log("Instanced X" + instancedObj.transform.position.x);
            Debug.Log("Instanced Y" +  instancedObj.transform.position.y);
            

            var objectsWithTag = GameObject.FindGameObjectsWithTag(objTag);

            foreach (var obj in objectsWithTag)
            {
                Debug.Log("Distance" + Vector3.Distance(obj.transform.position, instancedObj.transform.position));
                Debug.Log("Bounds" + _objToMove.GetComponent<Renderer>().bounds.size.x + " / " + _objToMove.GetComponent<Renderer>().bounds.size.y);

                if (Vector3.Distance(obj.transform.position, instancedObj.transform.position) <= instancedObj.GetComponent<Renderer>().bounds.size.x + objDistance)
                {
                    instancedObj.transform.position = new Vector3(instancedObj.transform.position.x + (objDistance + 2f) , instancedObj.transform.position.y, instancedObj.transform.position.z);
                }
            }

            if (instancedObj != null)
                lstObjInstance.Add(instancedObj);
        }

        foreach (var bush in lstObjInstance)
        {
            if (!IsOnScreen(bush))
            {
                lstObjInstance.Remove(bush);
                Destroy(bush);
                return;
            }
            else
            {
                bush.transform.position = new Vector3(bush.transform.position.x - objSpeed, bush.transform.position.y, instancedObj.transform.position.z);
            }
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
