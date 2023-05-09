using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    int itemSpace = 20;
    int itemCountInMap = 5;
    int coinsCountInItem = 7;
    int mapSize;
    float coinsHeight = 0.5f;
    public float laneOffset = 2.5f;

    enum TrackPos {Left = -1, Center = 0,  Right = 1};
    enum CoinsStyle {Line, JumpLine, RampLine}; 

    public GameObject CoinPrefab;
    public GameObject BlockUpPrefab;
    public GameObject BlockDownPrefab;
    public GameObject BlockFullPrefab;
    public GameObject RampPrefab;
    public Canvas panel;

    public List<GameObject> maps = new List<GameObject>();
    public List<GameObject> activeMaps = new List<GameObject>();

    //static public MapGenerator instance;

    struct MapItem
    {
        public void SetValues(GameObject block, TrackPos trackPos, CoinsStyle coinsStyle)
        {
            this.block = block;
            this.trackPos = trackPos;
            this.coinsStyle = coinsStyle;
        }
        public GameObject block;
        public TrackPos trackPos;
        public CoinsStyle coinsStyle;
    }

    private void Awake()
    {
        //instance = this;
        mapSize = itemCountInMap * itemSpace;
        maps.Add(MakeMap1());
        maps.Add(MakeMap2());
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
        if (activeMaps[0].transform.position.z < -mapSize)
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

    GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject block = null;
            TrackPos trackPos = TrackPos.Center;
            CoinsStyle coinsStyle = CoinsStyle.Line;

            if (i == 2)
            {
                trackPos = TrackPos.Left;
                block = RampPrefab;
                coinsStyle = CoinsStyle.RampLine;
            }
            else if (i == 3 || i == 4)
            {
                trackPos = TrackPos.Right;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;
            }

            Vector3 blockPos = new Vector3((int) trackPos * laneOffset, 0, i * itemSpace);
            CreateCoins(coinsStyle, blockPos, result);

            if (block != null)
            {
                GameObject go = Instantiate(block, blockPos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }
    GameObject MakeMap2()
    {
        GameObject result = new GameObject("Map2");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject block = null;
            TrackPos trackPos = TrackPos.Center;
            CoinsStyle coinsStyle = CoinsStyle.Line;
            if (i == 0)
            {
                trackPos = TrackPos.Left;
                block = BlockUpPrefab;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);
                GameObject go = Instantiate(block, blockPos1, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Center;
                block = RampPrefab;
                coinsStyle = CoinsStyle.RampLine;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);
                go = Instantiate(block, blockPos2, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Right;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;

            }
            else if (i == 1)
            {
                trackPos = TrackPos.Center;
                block = BlockFullPrefab;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);

                trackPos = TrackPos.Right;
                coinsStyle = CoinsStyle.Line;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);

                trackPos = TrackPos.Left;
                coinsStyle = CoinsStyle.JumpLine;
            }
            else if (i == 2)
            {
                trackPos = TrackPos.Left;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);
                GameObject go = Instantiate(block, blockPos1, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Center;
                coinsStyle = CoinsStyle.Line;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);

                trackPos = TrackPos.Right;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;
            }
            else if (i == 3)
            {
                trackPos = TrackPos.Left;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);
                GameObject go = Instantiate(block, blockPos1, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Center;
                coinsStyle = CoinsStyle.Line;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);

                trackPos = TrackPos.Right;
                coinsStyle = CoinsStyle.Line;
                block = null;
            }
            else
            {
                trackPos = TrackPos.Left;
                block = RampPrefab;
                coinsStyle = CoinsStyle.RampLine;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);
                GameObject go = Instantiate(block, blockPos1, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Center;
                coinsStyle = CoinsStyle.Line;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);

                trackPos = TrackPos.Right;
                block = RampPrefab;
                coinsStyle = CoinsStyle.RampLine;
            }

            Vector3 blockPos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateCoins(coinsStyle, blockPos, result);

            if (block != null)
            {
                GameObject go = Instantiate(block, blockPos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }
    GameObject MakeMap3()
    {
        GameObject result = new GameObject("Map2");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject block = null;
            TrackPos trackPos = TrackPos.Center;
            CoinsStyle coinsStyle = CoinsStyle.Line;
            if (i == 0)
            {
                trackPos = TrackPos.Left;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);
                GameObject go = Instantiate(block, blockPos1, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Center;
                coinsStyle = CoinsStyle.Line;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);
                go = Instantiate(block, blockPos2, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Right;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;

            }
            else if (i == 1)
            {
                trackPos = TrackPos.Center;
                block = BlockFullPrefab;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);

                trackPos = TrackPos.Right;
                coinsStyle = CoinsStyle.Line;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);

                trackPos = TrackPos.Left;
                coinsStyle = CoinsStyle.JumpLine;
            }
            else if (i == 2)
            {
                trackPos = TrackPos.Left;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);
                GameObject go = Instantiate(block, blockPos1, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Center;
                coinsStyle = CoinsStyle.Line;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);

                trackPos = TrackPos.Right;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;
            }
            else if (i == 3)
            {
                trackPos = TrackPos.Left;
                block = BlockDownPrefab;
                coinsStyle = CoinsStyle.JumpLine;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);
                GameObject go = Instantiate(block, blockPos1, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Center;
                coinsStyle = CoinsStyle.Line;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);

                trackPos = TrackPos.Right;
                coinsStyle = CoinsStyle.Line;
                block = null;
            }
            else
            {
                trackPos = TrackPos.Left;
                block = RampPrefab;
                coinsStyle = CoinsStyle.RampLine;
                Vector3 blockPos1 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos1, result);
                GameObject go = Instantiate(block, blockPos1, Quaternion.identity);
                go.transform.SetParent(result.transform);

                trackPos = TrackPos.Center;
                coinsStyle = CoinsStyle.Line;
                Vector3 blockPos2 = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
                CreateCoins(coinsStyle, blockPos2, result);

                trackPos = TrackPos.Right;
                block = RampPrefab;
                coinsStyle = CoinsStyle.RampLine;
            }

            Vector3 blockPos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateCoins(coinsStyle, blockPos, result);

            if (block != null)
            {
                GameObject go = Instantiate(block, blockPos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    void CreateCoins(CoinsStyle style, Vector3 pos, GameObject parentObject) 
    {
        Vector3 coinPos = Vector3.zero;
        if (style == CoinsStyle.Line)
        {
            for (int i = -coinsCountInItem/2; i < coinsCountInItem/2; i++)
            {
                coinPos.y = coinsHeight;
                coinPos.z = i * ((float) itemSpace / coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinsStyle.JumpLine) 
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Max(-1/2f * Mathf.Pow(i, 2) + 3, coinsHeight);
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinsStyle.RampLine) 
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Min(Mathf.Max(0.7f * (i + 4), coinsHeight), 2.5f);
                coinPos.z = i * ((float)itemSpace / coinsCountInItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
    }
}
