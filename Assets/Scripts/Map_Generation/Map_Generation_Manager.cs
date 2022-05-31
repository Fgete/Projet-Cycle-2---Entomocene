using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum entrancePosition { North, East, South, West };
public enum levelEnum { MainMenu, FirstLevel, LaboLevel };

public class Map_Generation_Manager : MonoBehaviour
{
    public bool reBuild = false;

    [Header("Map Infos")]
    // public (type of map)
    [Range(1, 10)]
    public int iterations;
    [Range(0, 100)]
    public int chanceOfExpension;
    [Range(0, 100)]
    public int corridorRatio;
    [Range(10, 50)]
    public int minimumRoomNumber;
    public levelEnum nextScene;
    public Color mainLightColor;

    [Header("Room Prefabs")]
    public GameObject firstRoomPrefab;
    public GameObject lastRoomPrefab;
    public List<GameObject> roomPrefabs;
    public List<GameObject> corridorPrefabs;
    [HideInInspector]
    public bool lastRoomSpawned = false;

    [Header("Inner Room Prefabs")]
    public List<PrefabPool> wallPools;
    public List<PrefabPool> openingPools;
    public List<PrefabPool> floorPools;
    // public List<GameObject> furniturePrefabs;
    public List<GameObject> enemyPrefabs;

    private UI_FadeOut introScreen;
    private bool isMapBigEnough = false;

    private void Start()
    {
        introScreen = FindObjectOfType<UI_FadeOut>();
        StartMap();
    }

    private void Update()
    {
        if (!isMapBigEnough)
            if (FindObjectsOfType<Map_Room_Expansion>().Length == 0)
                IsMapBigEnough();

        if (reBuild)
            ReBuild();
    }

    private void StartMap()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        lastRoomSpawned = false;

        GameObject firstRoom = Instantiate(firstRoomPrefab, transform.position, transform.rotation, transform);
        Map_Room_Expansion mre = firstRoom.GetComponent<Map_Room_Expansion>();

        mre.iteration = iterations;
        mre.previousDirection = entrancePosition.North;
        mre.roomId = "R0";
        mre.roomCoord = new Vector2(0, 0);
    }

    public void IsMapBigEnough()
    {
        if (transform.childCount < minimumRoomNumber || !lastRoomSpawned)
            StartMap();
        else
        {
            isMapBigEnough = true;

            BuildDoors();
            DestroyBeaconsCollider();

            GetComponent<NavMeshBaker>().GetNavMeshSurfaces();
            GetComponent<NavMeshBaker>().Bake();

            // --- SET ALL ENEMIES ALIVE ---
            foreach (Enemy e in FindObjectsOfType<Enemy>())
                e.alive = true;

            // --- SET CHARACTER PLAYABLE ---
            FindObjectOfType<Character_Movement>().PlayerToSpawn();

            // --- RUN INTRO ---
            introScreen.run = true;

            // --- DESTROY SELF ---
            Destroy(this);
        }
    }

    public void ReBuild()
    {
        reBuild = false;
        isMapBigEnough = false;
        StartMap();
    }

    public GameObject FindPrefabWithDirection(entrancePosition direction)
    {
        GameObject prefab;

        if (Random.Range(0, 100) < corridorRatio)
            prefab = corridorPrefabs[Random.Range(0, corridorPrefabs.Count)];
        else
            prefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)];

        bool isConform = false;

        switch (direction)
        {
            case entrancePosition.North:
                if (prefab.transform.Find("DOORS").Find("D_SOUTH"))
                    isConform = true;
                break;
            case entrancePosition.East:
                if (prefab.transform.Find("DOORS").Find("D_WEST"))
                    isConform = true;
                break;
            case entrancePosition.South:
                if (prefab.transform.Find("DOORS").Find("D_NORTH"))
                    isConform = true;
                break;
            case entrancePosition.West:
                if (prefab.transform.Find("DOORS").Find("D_EAST"))
                    isConform = true;
                break;
        }

        if (isConform)
            return prefab;
        else
            return FindPrefabWithDirection(direction);
    }

    public bool LastRoomCompatibleDirection(entrancePosition direction)
    {
        switch (direction)
        {
            case entrancePosition.North:
                return lastRoomPrefab.transform.Find("DOORS").Find("D_SOUTH");
            case entrancePosition.East:
                return lastRoomPrefab.transform.Find("DOORS").Find("D_WEST");
            case entrancePosition.South:
                return lastRoomPrefab.transform.Find("DOORS").Find("D_NORTH");
            case entrancePosition.West:
                return lastRoomPrefab.transform.Find("DOORS").Find("D_EAST");
            default: return false;
        }
    }

    private void BuildDoors()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        PrefabPool openingPool = openingPools[Random.Range(0, openingPools.Count)];

        foreach (GameObject door in doors)
        {
            GameObject spawned = Instantiate(openingPool.prefabs[Random.Range(0, openingPool.prefabs.Count)], door.transform.position, door.transform.rotation, door.transform);
            spawned.tag = "Door";
            if (door.GetComponent<Map_Door_Debug>())
                Destroy(door.GetComponent<Map_Door_Debug>());
        }
    }

    public void BuildWalls(List<Transform> walls)
    {
        PrefabPool wallPool = wallPools[Random.Range(0, wallPools.Count)];

        foreach (Transform wall in walls)
        {
            GameObject spawned = Instantiate(wallPool.prefabs[Random.Range(0, wallPool.prefabs.Count)], wall.position, wall.rotation, wall);
            spawned.tag = "Wall";
            if (wall.GetComponent<Map_Wall_Debug>())
                Destroy(wall.GetComponent<Map_Wall_Debug>());
        }
    }

    public void BuildFloors(List<Transform> floors)
    {
        PrefabPool floorPool = floorPools[Random.Range(0, floorPools.Count)];


        foreach (Transform floor in floors)
        {
            GameObject spawned = Instantiate(floorPool.prefabs[Random.Range(0, floorPool.prefabs.Count)], floor.position, floor.rotation, floor);
            spawned.tag = "Floor";
            if (floor.GetComponent<Map_Wall_Debug>())
                Destroy(floor.GetComponent<Map_Wall_Debug>());
        }
    }

    private void DestroyBeaconsCollider()
    {
        GameObject[] beacons = GameObject.FindGameObjectsWithTag("Room");

        foreach (GameObject beacon in beacons)
            if (beacon.GetComponent<BoxCollider>())
                Destroy(beacon.GetComponent<BoxCollider>());
    }
}
