using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using Astar;
using UnityEngine.EventSystems;
using NavMeshPlus.Components;
public enum WalkMode
{
    AStar,
    NavMesh,
    Keyboard,
}
public class PlayerC : MonoBehaviour
{
    Vector2 Velocity;
    Rigidbody2D rig;
    Vector3 targetPos;
    bool moveActive = false;
    public float moveSpeed;
    public float defaultSpeed;
    Tilemap map;

    List<Vector2> pathList;
    int i;

    public WalkMode walkMode;

    public AIMovement Agent;
    
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        walkMode = WalkMode.AStar;
        map = MapManager.Instance.tilemaps["FloorMap"];
        defaultSpeed = moveSpeed;
    }
    private void Update()
    {
        switch (walkMode)
        {
            case WalkMode.AStar:
                Astar();
                break;
            case WalkMode.NavMesh:
                NavMesh();
                break;
            case WalkMode.Keyboard:
                break;
            default:
                break;
        }
    }
    void FixedUpdate()
    {
        switch (walkMode)
        {
            case WalkMode.AStar:
                MoveByAstar();
                break;
            case WalkMode.NavMesh:
                MoveByNavMesh();
                break;
            case WalkMode.Keyboard:
                MoveByKeyboard();
                break;
            default:
                break;
        }
        if (walkMode != WalkMode.NavMesh)
        {
            Agent.SetTarget(transform.position);
        }
    }

    void NavMesh()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos = map.WorldToCell(worldPoint) + new Vector3(0.5f, 0.5f, 0);
            MoveActive = true;
        }
    }

    private void MoveByNavMesh()
    {
        if (MoveActive)
        {
            Agent.SetTarget(targetPos);

            MoveToTarget(Agent.transform.position);
        }
    }

    public void SwitchWalkMode(WalkMode walkMode)
    {
        MoveActive = false;
        this.walkMode = walkMode;
    }
    public bool MoveActive
    {
        get
        {
            return moveActive;
        }
        set
        {
            moveActive = value;
            if (!value)
                rig.velocity = Vector3.zero;
        }
    }
    void MoveByKeyboard()
    {
        Velocity.x = Input.GetAxisRaw("Horizontal");
        Velocity.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 2 * defaultSpeed;
        }
        else
        {
            moveSpeed = defaultSpeed;
        }
        MoveToTarget(transform.position + new Vector3(Velocity.x, Velocity.y, 0));
        
    }
    void Astar()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int targetGridPos = map.WorldToCell(targetPos);
            Vector2 finalTarget = new Vector2(targetGridPos.x, targetGridPos.y);

            Vector3Int startGridPos = map.WorldToCell(transform.position);
            Vector2 start = new Vector2(startGridPos.x, startGridPos.y);

            pathList = AStar.AStarFindWay(start, finalTarget, MapManager.Instance.currentMap.CheckWalkable);

            MoveActive = true;
            i = 0;
        }
    }
    void MoveByAstar()
    {
        if (MoveActive)
        {
            if (pathList != null && i < pathList.Count)
            {
                float distance = MoveToTarget(pathList[i] + new Vector2(0.5f, 0.5f));
                if (distance < 0.1f)
                {
                    i++;
                }
            }
            else
            {
                MoveActive = false;
            }
        }
    }
    float MoveToTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        
        rig.velocity = direction * moveSpeed;
        transform.localScale = Mult(new Vector3(Mathf.Sign(-direction.x),1,1) ,( 0.8f * Vector3.one - Mult(direction, direction) * 0.2f));
        
        return direction.magnitude;
    }
    Vector3 Mult(Vector3 a,Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }
}
