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

    private bool chasing = false;
    private float roomChecks = 3;
    private float roomCheck = 0;

    private float wonderTime = 2;
    private float wonderTimer = 0;
    private Vector3 wonderCentre;
    private float wonderRange = 0;

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
        if (chasing)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerGameObject.transform.position);

            if (distanceToPlayer < attackRange)
            {
                // Attack player
                agent.speed = 0;
                agentTargetPosition = transform.position;
            }
            else if (CanSeePlayer())
            {
                agent.speed = runSpeed;
                agentTargetPosition = playerGameObject.transform.position;
                Vector3 playerVelcocity = playerGameObject.GetComponent<CharacterController>().velocity;
                wonderCentre = playerGameObject.transform.position + playerVelcocity.normalized * distanceToPlayer;
                wonderCentre.y = playerGameObject.transform.position.y;
                wonderRange = distanceToPlayer;

                roomCheck = 0;
            }
            else
            {
                if (agent.remainingDistance < 0.5f)
                {
                    if (roomCheck < roomChecks)
                    {
                        Vector2 randomOffset = Random.insideUnitCircle;
                        agentTargetPosition = wonderCentre + new Vector3(randomOffset.x, 0, randomOffset.y) * wonderRange;

                        roomCheck++;
                    }
                    else
                    {
                        chasing = false;
                    }
                }
            }
        }
        else
        {
            agent.speed = walkSpeed;
            if (CanSeePlayer())
            {
                chasing = true;
            }
            else
            {
                if (agent.remainingDistance > 0.5f)
                {
                    wonderTimer = wonderTime;
                }
                else
                {
                    agent.speed = 0;
                    if (wonderTimer < 0)
                    {
                        // Fix this to be smarter
                        agentTargetPosition = Random.insideUnitSphere * 9;
                    }
                    else
                        wonderTimer -= Time.deltaTime;
                }
            }
        }

        foreach (GameObject door in doors)
        {
            float distanceToDoor = Vector3.Distance(transform.position, door.transform.position);
            if (distanceToDoor < attackRange * 1.5f)
            {
                if (!door.GetComponent<Door>().open)
                    door.GetComponent<Door>().Interact();
            }
            else if (distanceToDoor < attackRange * 2)
            {
                if (door.GetComponent<Door>().open && !chasing)
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
