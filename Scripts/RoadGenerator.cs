using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : Singleton<RoadGenerator>
{
    public GameObject roadPrefab;
    public List<GameObject> roads = new List<GameObject>();
    private float startSpeed = 10;
    public float speed = 0;
    public int maxRoadCount = 7;
    private int endPosition = -35;

    void Start()
    {
        PoolManager.Instance.Preload(roadPrefab, 15);
        ResetLevel();
    }

    void Update()
    {
        if (speed == 0) return;

        foreach (GameObject road in roads) 
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
        
        if (roads[0].transform.position.z < endPosition)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
            CreateNextRoad();
        }
    }

    private void CreateNextRoad()
    {
        Vector3 pos = new Vector3(0, 0, -10);
        if (roads.Count > 0)
        {
            pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 15);
        }
        GameObject go = PoolManager.Instance.Spawn(roadPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        roads.Add(go);
    }

    public void StartLevel()
    {
        speed = startSpeed;
        Time.timeScale = 1;
        MovePlayer.Instance.enabled = true;
    }

    public void ResetLevel()
    {
        speed = 0;
        while (roads.Count > 0) 
        {
            PoolManager.Instance.Despawn(roads[0]);
            roads.RemoveAt(0);
        }
        for (int i = 0; i < maxRoadCount; i++)
        {
            CreateNextRoad();
        }

        MovePlayer.Instance.enabled = false;
        MapGenerator.Instance.ResetMaps();
    }
}
