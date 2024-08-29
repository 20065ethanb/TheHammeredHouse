using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaddlesController : MonoBehaviour
{
    public float walkSpeed = 1;
    public float runSpeed = 2;

    private float sightRange = 15;

    private float wonderTime = 2;
    private float wonderTimer = 0;

    private Vector3 spotPosition;
    private float spotRange;
    private bool hasSnitched;

    public GameObject head;

    private GameObject playerGameObject;
    private GameObject ramesisGameObject;
    private NavMeshAgent agent;
    private Animator animator;

    private float agentUpdateTime = 0.2f;
    private float agentUpdateTimer = 0;
    private Vector3 agentTargetPosition;

    private AudioSource audioSource;
    public AudioClip detectSound;
    public AudioClip alertSound;

    // Start is called before the first frame update
    void Start()
    {
        spotPosition = transform.position;
        spotRange = 0;
        hasSnitched = true;

        playerGameObject = GameObject.Find("PlayerCharacter");
        ramesisGameObject = GameObject.Find("Ramesis");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerGameObject.transform.position); // Gets distance to player
        // If waddles can see player it'll take note of the players location
        if (CanSeePlayer())
        {
            spotPosition = playerGameObject.transform.position;
            spotRange = distanceToPlayer;
        }

        if (!hasSnitched)
        {
            // if not hasSnitched
            float distanceToRamesis = Vector3.Distance(transform.position, ramesisGameObject.transform.position); // Gets distance to ramesis

            if (distanceToRamesis < 2)
            {
                // snitch
                agent.speed = 0;
                animator.SetFloat("Speed", 0);
                agentTargetPosition = transform.position;
                ramesisGameObject.GetComponent<RamesisController>().Alert(spotPosition, spotRange);
                audioSource.PlayOneShot(alertSound);
                hasSnitched = true;
            }
            else
            {
                // Run to ramesis
                agent.speed = runSpeed;
                animator.SetFloat("Speed", 1);
                agentTargetPosition = ramesisGameObject.transform.position;
            }
        }
        else
        {
            // When waddles had snitched it wonders around
            agent.speed = walkSpeed;
            animator.SetFloat("Speed", 0.5f);
            if (CanSeePlayer())
            {
                hasSnitched = false;
                audioSource.PlayOneShot(detectSound);
            }
            else
            {
                // Sets wonder timer until wondering to next location
                if (agent.remainingDistance > 0.5f)
                {
                    wonderTimer = wonderTime;
                }
                // Else it'll stand still
                else
                {
                    agent.speed = 0;
                    animator.SetFloat("Speed", 0.0f);
                    if (wonderTimer < 0)
                        agentTargetPosition = RandomPoint(Vector3.zero, 10, false);
                    else
                        wonderTimer -= Time.deltaTime;
                }
            }
        }

        // Agent updating timer
        if (agentUpdateTimer <= 0)
        {
            agent.SetDestination(agentTargetPosition);
            agentUpdateTimer = agentUpdateTime;
        }
        else
        {
            // Otherwise decrease timer
            agentUpdateTimer -= Time.deltaTime;
        }
    }

    public bool CanSeePlayer()
    {
        // Casts ray towards player
        Ray ray = new(head.transform.position, playerGameObject.transform.position - transform.position);

        // If raycast hits player, waddles can see the player
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
    
    // Gets random point to wonder to
    private Vector3 RandomPoint(Vector3 center, float range, bool sameY)
    {
        Vector3 randomOffset = Random.insideUnitSphere * range;
        if (sameY) randomOffset.y = 0;
        Vector3 randomPoint = center + randomOffset;
        return ClosestMeshPoint(randomPoint, range);
    }


    private Vector3 ClosestMeshPoint(Vector3 point, float range)
    {
        NavMeshHit hit;
        // the function that finds the closest point on the nav mesh to the point 
        NavMesh.SamplePosition(point, out hit, range * 2, NavMesh.AllAreas);
        return hit.position;
    }
}
