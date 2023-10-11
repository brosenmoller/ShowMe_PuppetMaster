using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRaft : MonoBehaviour
{

    [SerializeField]
    private bool isPlayerOnRaft;
    [SerializeField]
    private float speedCap = 15f;
    [SerializeField]
    private float drag = 0.1f;
    [SerializeField]
    private float collisionDrag = 0.5f;
    [SerializeField]
    private Vector2 currentVelocity;
    [SerializeField]
    private Vector3 raftSize;
    [SerializeField]
    private float raylength = 4.24f;
    [SerializeField]
    private bool usingStream;
    public List<WaterStream> activeWaterStreams = new List<WaterStream>();
    private float maxSpeed;

    private float secondRayLength;

    private Vector2 forces;

    private bool isRaftBlocked;

    public bool IsPlayerOnRaft => isPlayerOnRaft;

    public bool IsRaftBlocked => isRaftBlocked;

    public LayerMask RaftCollision;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        maxSpeed = raylength;
        secondRayLength = raftSize.x / 2f;
    }

    private void Start()
    {
        AddForce(1f, 1f);
    }

    public void AddForce(float x, float y)
    {
        forces += new Vector2(x, y);
        forces = Vector2.ClampMagnitude(forces, speedCap);
    }

    public void ShootToAddForce(float x, float y)
    {
        AddForce(x, y);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        isPlayerOnRaft = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        isPlayerOnRaft = true;
    }

    private Vector2 GetStreamForce()
    {
        Vector2 zero = Vector2.zero;
        foreach (WaterStream activeWaterStream in activeWaterStreams)
        {
            zero += new Vector2(activeWaterStream.calculatedDirection.x, activeWaterStream.calculatedDirection.z);
        }
        return zero;
    }

    private void FixedUpdate()
    {
        Vector2 vector = Vector2.zero;
        if (usingStream)
        {
            vector = GetStreamForce();
        }
        Vector2 vector2 = (currentVelocity + forces + vector) / (1f + drag);
        Debug.DrawRay(base.transform.position, new Vector3(vector2.x, 0f, vector2.y).normalized * secondRayLength, Color.red, Time.fixedDeltaTime);
        if (Physics.BoxCast(base.transform.position - new Vector3(0f, raftSize.y / 2f, 0f), raftSize / 2f, new Vector3(vector2.x, 0f, vector2.y).normalized, out var hitInfo, base.transform.rotation, raylength, RaftCollision, QueryTriggerInteraction.Ignore))
        {
            isRaftBlocked = true;
            Vector3 vector3 = Vector3.Reflect(new Vector3(vector2.x, 0f, vector2.y), hitInfo.normal);
            vector2 = new Vector2(vector3.x, vector3.z) / (1f + collisionDrag);
        }
        else if (forces != Vector2.zero)
        {
            isRaftBlocked = false;
        }
        forces = Vector2.zero;
        base.transform.position += new Vector3(vector2.x, 0f, vector2.y) * Time.fixedDeltaTime;
        currentVelocity = vector2;
    }
}
