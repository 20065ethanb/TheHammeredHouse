using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RamesisController : MonoBehaviour
{
    private float walkSpeed = 2.5f;
    private float runSpeed = 5;

    private float sightRange = 10;
    private float attackRange = 1.1f;

    public GameObject head;

    private GameObject playerGameObject;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        playerGameObject = GameObject.Find("PlayerCharacter");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, playerGameObject.transform.position) < attackRange)
        {
            agent.speed = 0;
            agent.SetDestination(transform.position);
        }
        else if (CanSeePlayer())
        {
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
    }

    bool CanSeePlayer()
    {
        // Casts ray towards player
        Ray ray = new(head.transform.position, playerGameObject.transform.position - transform.position);

        // If raycast hits player, tihor can see player
        // Otherwise it is hitting a tree or a building
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
