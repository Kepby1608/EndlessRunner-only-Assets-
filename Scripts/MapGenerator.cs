using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    int itemSpace = 15;
    int itemCountInMap = 5;
    public float laneOffset = 2.5f;

    enum TrackPos {Left = -1, Center = 0,  Right = 1};

    public GameObject CoinPrefab;
    public GameObject BlockUpPrefab;
    public GameObject BlockDownPrefab;
    public GameObject BlockFullPrefab;
    public GameObject RampPrefab;

    public List<GameObject> maps = new List<GameObject>();

    static public MapGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        maps.Add(MakeMap1());
    }

    void Update()
    {
        
    }

    GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject block = null;
            TrackPos trackPos = TrackPos.Center;

            if (i == 2)
            {
                trackPos = TrackPos.Left;
                block = RampPrefab;
            }
            else if (i == 3 || i == 4)
            {
                trackPos = TrackPos.Right;
                block = BlockDownPrefab;
            }

            Vector3 blockPos = new Vector3((int) trackPos * laneOffset, 0, i * itemSpace);
            if (block != null)
            {
                GameObject go = Instantiate(block, blockPos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }
}
