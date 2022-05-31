using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Room_Expansion : MonoBehaviour
{
    [Header("Map Generation Manager")]
    public Map_Generation_Manager mgm;
    public LayerMask roomSpace;

    [Header("Prefab Elements")]
    public Transform doors;
    public Transform walls;
    public Transform floors;
    public Transform furnitures;
    public Transform enemies;

    [Header("Parent Infos")]
    public entrancePosition previousDirection;

    [Header("Room Infos")]
    public string roomId;
    public Vector2 roomCoord; // Origin is at (0; 0)
    public int iteration;

    [Header("Prefabs")]
    public List<GameObject> wallPrefabs;
    public List<GameObject> floorPrefabs;

    [Header("Debug")]
    public Transform debug;


    private void Start()
    {
        mgm = GameObject.FindObjectsOfType<Map_Generation_Manager>()[0];

        // Lets grow this map !
        DestroyPreviousDirection();

        for (int i = 0; i < doors.childCount; i++)
        {
            if (Random.Range(0, 100) < mgm.chanceOfExpension && doors.GetChild(i).CompareTag("Door"))
                SpawnNewRoom(doors.GetChild(i));
            else
                switch (doors.GetChild(i).gameObject.name)
                {
                    case "D_NORTH": TransformDoorToWall(doors.GetChild(i).gameObject, entrancePosition.North); break;
                    case "D_EAST": TransformDoorToWall(doors.GetChild(i).gameObject, entrancePosition.East); break;
                    case "D_SOUTH": TransformDoorToWall(doors.GetChild(i).gameObject, entrancePosition.South); break;
                    case "D_WEST": TransformDoorToWall(doors.GetChild(i).gameObject, entrancePosition.West); break;
                }
        }

        BuildRoom();
        DestroyDebug();

        Destroy(GetComponent<Map_Room_Expansion>());
    }

    private void DestroyPreviousDirection()
    {
        if (mgm.iterations == iteration)
            switch (previousDirection)
            {
                case entrancePosition.North: TransformDoorToWall(doors.Find("D_SOUTH").gameObject, entrancePosition.South); break;
                case entrancePosition.East: TransformDoorToWall(doors.Find("D_WEST").gameObject, entrancePosition.West); break;
                case entrancePosition.South: TransformDoorToWall(doors.Find("D_NORTH").gameObject, entrancePosition.North); break;
                case entrancePosition.West: TransformDoorToWall(doors.Find("D_EAST").gameObject, entrancePosition.East); break;
            }
        else
            switch (previousDirection)
            {
                case entrancePosition.North: Destroy(doors.Find("D_SOUTH").gameObject); break;
                case entrancePosition.East: Destroy(doors.Find("D_WEST").gameObject); break;
                case entrancePosition.South: Destroy(doors.Find("D_NORTH").gameObject); break;
                case entrancePosition.West: Destroy(doors.Find("D_EAST").gameObject); break;
            }
    }

    private void SpawnNewRoom(Transform door)
    {
        entrancePosition direction = entrancePosition.North;
        switch (door.name)
        {
            case "D_NORTH": direction = entrancePosition.North; break;
            case "D_EAST": direction = entrancePosition.East; break;
            case "D_SOUTH": direction = entrancePosition.South; break;
            case "D_WEST": direction = entrancePosition.West; break;
        }

        if (iteration > 0)
        {
            GameObject newGameObject = new GameObject();
            Transform newRoomTransform = newGameObject.transform;
            switch (direction)
            {
                case entrancePosition.North: newRoomTransform.position = new Vector3(door.position.x, door.position.y, door.position.z + 5); break;
                case entrancePosition.East: newRoomTransform.position = new Vector3(door.position.x + 5, door.position.y, door.position.z); break;
                case entrancePosition.South: newRoomTransform.position = new Vector3(door.position.x, door.position.y, door.position.z - 5); break;
                case entrancePosition.West: newRoomTransform.position = new Vector3(door.position.x - 5, door.position.y, door.position.z); break;
            }

            if (!Physics.CheckSphere(newRoomTransform.position, 1))
            {
                GameObject newRoom = Instantiate(mgm.FindPrefabWithDirection(direction), newRoomTransform.position, newRoomTransform.rotation, mgm.transform);
                Map_Room_Expansion mre = newRoom.GetComponent<Map_Room_Expansion>();

                mre.iteration = iteration - 1;
                mre.previousDirection = direction;
                mre.roomId = "R" + (mgm.iterations - iteration + 1);
                switch (direction)
                {
                    case entrancePosition.North: mre.roomCoord = new Vector2(roomCoord.x, roomCoord.y + 1); break;
                    case entrancePosition.East: mre.roomCoord = new Vector2(roomCoord.x + 1, roomCoord.y); break;
                    case entrancePosition.South: mre.roomCoord = new Vector2(roomCoord.x, roomCoord.y - 1); break;
                    case entrancePosition.West: mre.roomCoord = new Vector2(roomCoord.x - 1, roomCoord.y); break;
                }
            }
            else
                TransformDoorToWall(door.gameObject, direction);
            Destroy(newGameObject);
        }
        else if (!mgm.lastRoomSpawned && mgm.LastRoomCompatibleDirection(direction))
        {
            GameObject newGameObject = new GameObject();
            Transform newRoomTransform = newGameObject.transform;
            switch (direction)
            {
                case entrancePosition.North: newRoomTransform.position = new Vector3(door.position.x, door.position.y, door.position.z + 5); break;
                case entrancePosition.East: newRoomTransform.position = new Vector3(door.position.x + 5, door.position.y, door.position.z); break;
                case entrancePosition.South: newRoomTransform.position = new Vector3(door.position.x, door.position.y, door.position.z - 5); break;
                case entrancePosition.West: newRoomTransform.position = new Vector3(door.position.x - 5, door.position.y, door.position.z); break;
            }

            if (!Physics.CheckSphere(newRoomTransform.position, 1))
            {
                mgm.lastRoomSpawned = true;
                GameObject newRoom = Instantiate(mgm.lastRoomPrefab, newRoomTransform.position, newRoomTransform.rotation, mgm.transform);
                Map_Room_Expansion mre = newRoom.GetComponent<Map_Room_Expansion>();

                mre.iteration = iteration - 1;
                mre.previousDirection = direction;
                mre.roomId = "R" + (mgm.iterations - iteration + 1);
                switch (direction)
                {
                    case entrancePosition.North: mre.roomCoord = new Vector2(roomCoord.x, roomCoord.y + 1); break;
                    case entrancePosition.East: mre.roomCoord = new Vector2(roomCoord.x + 1, roomCoord.y); break;
                    case entrancePosition.South: mre.roomCoord = new Vector2(roomCoord.x, roomCoord.y - 1); break;
                    case entrancePosition.West: mre.roomCoord = new Vector2(roomCoord.x - 1, roomCoord.y); break;
                }
            }
            else
                TransformDoorToWall(door.gameObject, direction);

            Destroy(newGameObject);
        }
        else
            TransformDoorToWall(door.gameObject, direction);
    }

    private void TransformDoorToWall(GameObject doorToTransform, entrancePosition ep)
    {
        if (doorToTransform.CompareTag("Door"))
        {
            doorToTransform.tag = "Wall";

            Destroy(doorToTransform.GetComponent<Map_Door_Debug>());
            doorToTransform.AddComponent<Map_Wall_Debug>();

            Destroy(doorToTransform.GetComponent<Door_Build>());
            doorToTransform.AddComponent<Wall_Build>();

            Map_Wall_Debug mwd = doorToTransform.GetComponent<Map_Wall_Debug>();
            mwd.wallOffset = new Vector3(0, 1.5f, 0);
            switch (ep)
            {
                case entrancePosition.North: mwd.wallSize = new Vector3(3, 3, 0); break;
                case entrancePosition.East: mwd.wallSize = new Vector3(0, 3, 3); break;
                case entrancePosition.South: mwd.wallSize = new Vector3(3, 3, 0); break;
                case entrancePosition.West: mwd.wallSize = new Vector3(0, 3, 3); break;
            }
            doorToTransform.transform.eulerAngles = new Vector3(doorToTransform.transform.eulerAngles.x, doorToTransform.transform.eulerAngles.y + 180, doorToTransform.transform.eulerAngles.z);
        }
    }

    private void BuildRoom()
    {
        List<Transform> elementsToBuildAsWall = new List<Transform>();
        List<Transform> elementsToBuildAsFloor = new List<Transform>();

        foreach (Transform wall in walls)
            elementsToBuildAsWall.Add(wall);

        foreach (Transform door in doors)
            if (door.GetComponent<Map_Wall_Debug>())
                elementsToBuildAsWall.Add(door);

        foreach (Transform floor in floors)
            elementsToBuildAsFloor.Add(floor);

        mgm.BuildWalls(elementsToBuildAsWall);
        mgm.BuildFloors(elementsToBuildAsFloor);
    }

    private void DestroyDebug()
    {
        foreach (Transform element in debug)
            Destroy(element.gameObject);
    }
}
