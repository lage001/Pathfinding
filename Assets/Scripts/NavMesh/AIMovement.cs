using UnityEngine;
using UnityEngine.AI;


public class AIMovement : MonoBehaviour
{
    Vector3 target;
    NavMeshAgent agent;
    public bool moveActive;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        target = transform.position;
    }
    private void OnEnable()
    {
        moveActive = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (moveActive)
        {
            agent.destination = new Vector3(target.x, target.y, 0);
        }
    }
    public void SetTarget(Vector3 target)
    {
        this.target = target;
        moveActive = true;
    }

    
}
