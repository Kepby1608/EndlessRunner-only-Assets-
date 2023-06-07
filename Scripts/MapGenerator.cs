using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    int itemSpace = 20;
    int itemCountInMap = 5;
    int coinsCountInItem = 7;
    int mapSize;
    int countMap = 1;
    float coinsHeight = 0.5f;
    public float laneOffset = 2.5f;
    public int laneUpOffset = 2;
    public GameObject coinPrefab;

    int[] trackPos = new int[7];
    int[] height = new int[3];
    public GameObject[] prefabs;

    public List<GameObject> maps = new List<GameObject>();
    public List<GameObject> activeMaps = new List<GameObject>();

    //enum TrackPos {LLL = -3, LL = -2,  L = -1, Z = 0, R = 1, RR = 2, RRR = 3};
    //enum CoinsStyle {Line = 0, JumpLine = 1, RampLine = 2}; 
    //enum HeightPos {Down = 1, Center = 2, Up = 3};

    //struct MapItem
    //{
    //    public void SetValues(GameObject block, TrackPos trackPos, CoinsStyle coinsStyle)
    //    {
    //        this.block = block;
    //        this.trackPos = trackPos;
    //        this.coinsStyle = coinsStyle;
    //    }
    //    public GameObject block;
    //    public TrackPos trackPos;
    //    public CoinsStyle coinsStyle;
    //}

    private void Awake()
    {
        mapSize = itemCountInMap * itemSpace;
        int track = -3;
        for (int i = 0; i < trackPos.Length; i++)
        {
            trackPos[i] = track;
            track++;
        }
        for (int i = 0; i < height.Length; i++)
        {
            height[i] = i;
        }
        maps.Add(MakeMap3());
        maps.Add(MakeMap3());
        maps.Add(MakeMap3());
        maps.Add(MakeMap3());
        maps.Add(MakeMap3());
        maps.Add(MakeMap3());
        maps.Add(MakeMap3());
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }
        
    }

    void Start()
    {

    }

    void Update()
    {
        if (RoadGenerator.Instance.speed == 0) return;

        foreach (GameObject map in activeMaps)
        {
            map.transform.position -= new Vector3(0, 0, RoadGenerator.Instance.speed * Time.deltaTime);
        }
        if (activeMaps[0].transform.position.z < (-mapSize - 50))
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }

    void RemoveFirstActiveMap ()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }

    void AddActiveMap ()
    {
        int r = Random.Range(0, maps.Count);
        GameObject go = maps[r];
        go.SetActive(true); 
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetActive(true);
        }
        go.transform.position = activeMaps.Count > 0 ?
            activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * mapSize :
            new Vector3(0, 0, 10);
        maps.RemoveAt(r);
        activeMaps.Add(go);
    }

    public void ResetMaps()
    {
        while (activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }
        AddActiveMap();
        AddActiveMap();
    }

    GameObject MakeMap3()
    {
        GameObject result = new GameObject("Map" + countMap);
        countMap++;
        result.transform.SetParent(transform);

        int randPref = Random.Range(0, prefabs.Length);

        GameObject go;

        Vector3[] blockPos = new Vector3[7];

        for (int i = 0; i < itemCountInMap; i++)
        {
            int coinsStyle = 0;
                for (int j = 0; j < 7; j++)
                {
                    blockPos[j] = new Vector3(trackPos[j] * laneOffset, height[Random.Range(0, height.Length)] * laneUpOffset, i * itemSpace);
                    go = Instantiate(prefabs[randPref], blockPos[j], Quaternion.identity);

                // line
                if (prefabs[randPref].gameObject.name == "BlockUp")
                {
                    coinsStyle = 0;
                    CreateCoins(coinsStyle, blockPos[j], result);
                }
                // jump
                else if (prefabs[randPref].gameObject.name == "BlockDown")
                {
                    coinsStyle = 1;
                    CreateCoins(coinsStyle, blockPos[j], result);

                }
                // ramp
                else if (prefabs[randPref].gameObject.name == "Ramp")
                {
                    coinsStyle = 2;
                    CreateCoins(coinsStyle, blockPos[j], result);
                }

                randPref = Random.Range(0, prefabs.Length);
                go.transform.SetParent(result.transform);
                
                }

        }
        return result;
    }

    void CreateCoins(int style, Vector3 pos, GameObject parentObject) 
    {
        Vector3 coinPos = Vector3.zero;
        if (style == 0)
        {
            for (int i = -coinsCountInItem/2; i < coinsCountInItem/2; i++)
            {
                coinPos.y = coinsHeight;
                coinPos.z = i * ((float) itemSpace / coinsCountInItem);
                GameObject go = Instantiate(coinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == 1) 
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Max(-1/2f * Mathf.Pow(i, 2) + 3, coinsHeight);
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(coinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == 2) 
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Min(Mathf.Max(0.7f * (i + 4), coinsHeight), 2.5f);
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(coinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
    }
}
