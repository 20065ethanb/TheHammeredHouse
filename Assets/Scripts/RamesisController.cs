using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RamesisController : MonoBehaviour
{
    private float walkSpeed = 2;
    private float runSpeed = 4;

    private float sightRange = 15;
    private float attackRange = 1.1f;

    private float angerTime = 30;
    private float angerTimer = 0;

    private float wonderTime = 2;
    private float wonderTimer = 0;
    private Vector3 wonderCentre;
    private float wonderRange = 3;

    public GameObject head;

    private GameObject playerGameObject;
    private NavMeshAgent agent;

    private float agentUpdateTime = 0.2f;
    private float agentUpdateTimer = 0;
    private Vector3 agentTargetPosition;

    public GameObject wonderCentreObject;

    public GameObject[] doors;

    // Start is called before the first frame update
    void Start()
    {
        wonderCentre = transform.position;
        agentTargetPosition = transform.position;

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
            agentTargetPosition = transform.position;
        }
        else if (CanSeePlayer())
        {
            // Chasing player
            agent.speed = runSpeed;
            agentTargetPosition = playerGameObject.transform.position;

            Vector3 toPlayer = playerGameObject.transform.position - transform.position;
            wonderCentre = playerGameObject.transform.position + toPlayer.normalized * wonderRange / 2;
            wonderCentre.y = playerGameObject.transform.position.y;

            angerTimer = angerTime;
        }
        else
        {
            if (agent.remainingDistance > 0.5f)
            {
                agent.speed = (angerTimer > 0) ? runSpeed : walkSpeed;
                wonderTimer = wonderTime;
            }
            else
            {
                agent.speed = 0;
                if (wonderTimer < 0)
                {
                    Vector3 randomOffset = new Vector3((Random.value * 2) - 1, 0, (Random.value * 2) - 1);
                    agentTargetPosition = (randomOffset * wonderRange) + wonderCentre;
                }
                else
                    wonderTimer -= Time.deltaTime;
            }
            if (angerTimer > 0)
                angerTimer -= Time.deltaTime;
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

        if (agentUpdateTimer < 0)
        {
            agent.SetDestination(agentTargetPosition);
            agentUpdateTimer = agentUpdateTime;
        }
        else
            agentUpdateTimer -= Time.deltaTime;


        if (wonderCentreObject != null) wonderCentreObject.transform.position = wonderCentre;
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
