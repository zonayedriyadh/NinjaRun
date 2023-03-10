using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;
using Modules;

public enum ParallaxState
{
    Pause,
    Running
}

public enum ParallaxItemType
{
    layer0,
    Layer1,
    Layer2,
    Layer3,
    Layer4
}

[Serializable]
public class ItemConfig
{
    public int itemCount;
    public GameObject itemObjectPrefab;
}

[Serializable]
public class ParallaxItem
{
    public ParallaxItemType itemType;
    public int order;
    public bool isLoop;
    public List<ItemConfig> listOfItemsPrefab;
    [Range(0, 1)]
    public float SpeedFraction;
    [Range(0,1920)]
    public float minSpacingBetween;
    [Range(0, 1920)]
    public float maxSpacingBetween;
    private GameObject parentObj;
    public GameObject ParentObj { get { return parentObj; } set { parentObj = value; } }
    private List<GameObject> listOfObject = new List<GameObject>();

    public void AddListOfObject(GameObject itemObject)
    {
        listOfObject.Add(itemObject);
    }
    public Vector2 GetLastObjectPos()
    {
        if (listOfObject.Count == 0)
            return Vector2.zero;
        else
        {
            GameObject lastObj = listOfObject[listOfObject.Count - 1];
            Vector2 lastPos = lastObj.transform.position;
            Vector2 lastPosToRespown = new Vector2(lastPos.x+ lastObj.GetComponent<RectTransform>().sizeDelta.x * PanelController.Instance.GetScaleFactor(), lastPos.y);
            return lastPosToRespown;
        }
    }

    public GameObject GetRandomObject()
    {
        int rand = Random.Range(0,listOfItemsPrefab.Count);
        return listOfItemsPrefab[rand].itemObjectPrefab;
    }

    public bool moveObjects(float dt,float speed)
    {
        bool isOneItemOutOfTheScreen = false;
        foreach(GameObject item in listOfObject)
        {
            item.transform.position = new Vector2(item.transform.position.x-dt * speed * SpeedFraction* PanelController.Instance.GetScaleFactor(), item.transform.position.y);
            if (item.transform.position.x <= -item.GetComponent<RectTransform>().sizeDelta.x * PanelController.Instance.GetScaleFactor())
            {
                isOneItemOutOfTheScreen = true;
            }
        }
        if(isOneItemOutOfTheScreen)
        {
            GameObject toBeDestroy = listOfObject[0];
            listOfObject.RemoveAt(0);
            UnityEngine.Object.Destroy(toBeDestroy);
        }
        return isOneItemOutOfTheScreen;
    }
}
public class ParallaxBackgrounds : MonoBehaviour
{
    public List<ParallaxItem> parallaxItem;
    public GameObject ParallaxArea;
    public float speed;
    private ParallaxState currentState;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        parallaxItem.OrderByDescending(x => x.order);
        SetParallaxItem();
        currentState = ParallaxState.Running;
    }
    public void ReInitialize()
    {
        currentState = ParallaxState.Running;
    }

    public void SetPause()
    {
        currentState = ParallaxState.Pause;
    }
    // Update is called once per frame
    void Update()
    {
        if(currentState == ParallaxState.Running)
            MoveParallaxObjects(Time.deltaTime);
    }

    private void SetParallaxItem()
    {
        foreach(ParallaxItem item in parallaxItem)
        {
            GameObject area = Instantiate(ParallaxArea, transform);
            area.name = item.itemType.ToString();
            item.ParentObj = area;
            area.SetActive(true);
            Vector2 startPoint = Vector2.zero;
            foreach(ItemConfig element in item.listOfItemsPrefab)
            {
                for(int i = 0; i< element.itemCount;i++)
                {
                    AddObject(item, element.itemObjectPrefab);
                }
            }
        }
    }
    private void MoveParallaxObjects(float dt)
    {
        foreach (ParallaxItem item in parallaxItem)
        {
            bool isNewItemShouldBeAdded = item.moveObjects(dt,speed);
            if (isNewItemShouldBeAdded)
                AddObject(item);
        }
    }

    public void AddObject(ParallaxItem item, GameObject element = null)
    {
        if(element == null)
        {
            element = item.GetRandomObject();
        }
        GameObject platformObj = Instantiate(element, item.ParentObj.transform);
        /*GameObject platformObj = new GameObject();
        platformObj.transform.SetParent(item.ParentObj.transform,false);
        platformObj.AddComponent<Image>().sprite = element;
        platformObj.GetComponent<Image>().SetNativeSize();
        platformObj.GetComponent<RectTransform>().pivot = new Vector2(0, 0);*/
        //platformObj.GetComponent<RectTransform>().anchorPre = 
        //platformObj.transform.position = item.GetLastObjectPos();
        /*platformObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f,0.0f);
        platformObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.0f, 0.0f);*/
        float offset = Random.Range(item.minSpacingBetween, item.maxSpacingBetween);
        platformObj.transform.position = new Vector2(item.GetLastObjectPos().x+ offset, item.GetLastObjectPos().y);
        item.AddListOfObject(platformObj);
    }
}
