using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    public Transform characterBody;
    public Animator movementController;
    [Space]
    public float speedMultiplicator;
    [Space]
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Camera mainCamera;
    private CharacterController controller;
    private Character_Stats cs;
    private bool isGrounded;
    Vector3 velocity;

    private void Awake()
    {
        Physics.IgnoreLayerCollision(6, 8); //  PLAYER | PARTICLE MESH
        cs = GetComponent<Character_Stats>();
        controller = FindObjectOfType<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        float mousePosX = Input.mousePosition.x / Screen.height - (Screen.width / Screen.height) * .5f;
        float mousePosY = Input.mousePosition.y / Screen.height - .5f;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosX, mousePosY, 0));

        Vector3 mousePosPercent = new Vector3(mousePosX, 0, mousePosY);
        Vector3 lookPos = new Vector3(mousePosPercent.x, transform.position.y, mousePosPercent.z);
        lookPos += new Vector3(transform.position.x, 0, transform.position.z);
        Vector2 axisInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Set character move
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            characterBody.transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        Vector3 move = transform.right * axisInput.x + transform.forward * axisInput.y;
        controller.Move(move * cs.speed * speedMultiplicator * Time.deltaTime);

        // Set movement state
        int dir = 0;
        if (move.x != 0 || move.z != 0) dir = AnimationState(axisInput);
        movementController.SetInteger("moveDirection", dir);
    }

    int AnimationState(Vector2 inputs)
    {
        // Get body orientation
        float bodyRot = characterBody.eulerAngles.y;
        // Get character translation direction
        int bodyDir = AngleToDir(bodyRot);
        
        float inputRot = -VectorToAngle(inputs.y, -inputs.x) + 360;
        if (inputRot == 360) { inputRot = 0; }
        int inputDir = AngleToDir(inputRot);

        return RelativeDir(inputDir, bodyDir);
    }

    float RadToDeg(float rad) { return rad * (180 / Mathf.PI); }

    float VectorToAngle(float x, float y)
    {
        if (x == 0)
            return (y > 0) ? 90
                : (y == 0) ? 0
                : 270;
        else if (y == 0)
            return (x >= 0) ? 0
                : 180;

        float ret = RadToDeg(Mathf.Atan(y / x));

        if (x < 0 && y < 0)
            ret = 180 + ret;
        else if (x < 0)
            ret = 180 + ret;
        else if (y < 0)
            ret = 270 + (90 + ret);

        return ret;
    }

    int AngleToDir(float angle)
    {
        if (angle > 315 || angle <= 45) return 1;
        else if (angle > 45 && angle <= 135) return 2;
        else if (angle > 135 && angle <= 225) return 3;
        else if (angle > 225 && angle <= 315) return 4;
        else return 0;
    }

    int RelativeDir(int dirA, int dirB)
    {
        if (dirA == dirB) return 1;
        else if (dirA == 1 && dirB == 4 || dirA == 2 && dirB == 1 || dirA == 3 && dirB == 2 || dirA == 4 && dirB == 3) return 2;
        else if (dirA == 1 && dirB == 3 || dirA == 2 && dirB == 4 || dirA == 3 && dirB == 1 || dirA == 4 && dirB == 2) return 3;
        else return 4;
    }

    public void PlayerToSpawn()
    {
        Transform spawnPoint = transform;
        if (GameObject.FindGameObjectWithTag("SpawnPoint").transform)
            spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        else
            Debug.LogWarning("No spawn point !");
        if (spawnPoint)
            transform.position = spawnPoint.position;
        Debug.Log("Player ready !");
    }
}
