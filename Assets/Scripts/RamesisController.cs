using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RamesisController : MonoBehaviour
{
    private float walkSpeed = 2;
    private float runSpeed = 4.5f;

    private float sightRange = 10;
    private float attackRange = 1.1f;

    public GameObject head;

    private GameObject playerGameObject;
    private NavMeshAgent agent;

    public GameObject[] doors;

    // Start is called before the first frame update
    void Start()
    {
        playerGameObject = GameObject.Find("PlayerCharacter");
        agent = GetComponent<NavMeshAgent>();

        doors = GameObject.FindGameObjectsWithTag("Door");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, playerGameObject.transform.position) < attackRange)
        {
            // Attack player
            agent.speed = 0;
            agent.SetDestination(transform.position);
        }
        else if (CanSeePlayer())
        {
            // Chasing player
            agent.speed = runSpeed;
            agent.SetDestination(playerGameObject.transform.position);
        }
        else
        {
            if (agent.remainingDistance  > 0.5f)
            {
                agent.speed = walkSpeed;
            }
            else
            {
                agent.speed = 0;
            }
        }

        foreach (GameObject door in doors)
        {
            if (Vector3.Distance(transform.position, door.transform.position) < attackRange * 2)
            {
                // the door is in range
                if (!door.GetComponent<Door>().open)
                    door.GetComponent<Door>().Interact();
            }
        }
    }

    bool CanSeePlayer()
    {
        // Casts ray towards player
        Ray ray = new(head.transform.position, playerGameObject.transform.position - transform.position);

        // If raycast hits player, ramesis can see the player
        // Otherwise it is another object
        if (Physics.Raycast(ray, out RaycastHit hit, sightRange, ~(1 << 2)))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                return true;
            }
            else
            {
                Debug.DrawLine(ray.origin, hit.point, Color.blue);
            }
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * sightRange, Color.green);
        }
        return false;
    }
}
