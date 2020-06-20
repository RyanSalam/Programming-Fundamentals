using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    private Graph<GameObject> WayPoints;
    [SerializeField] GameObject target;
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private LayerMask whatisPlayer;

    public List<GameObject> paths = new List<GameObject>();

    private NavMeshAgent agent;
    private EnemyState state = EnemyState.Idle;

    private void Awake()
    {
        WayPoints = new Graph<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        //paths = new List<GameObject>();

    }

    private void Start()
    {
        if (paths == null)
        {
            Debug.Log("what");
            return;
        }
            

        foreach (var path in paths)
        {
            var node = WayPoints.AddNode(path);
        }


        foreach (GameObject path in paths)
        {
            var dest =  (paths[Random.Range(0, paths.Count)]);
            var dest1 = (paths[Random.Range(0, paths.Count)]);           

            WayPoints.AddEdge(path, dest);
            WayPoints.AddEdge(path, dest1);
        }

    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Idle:

                if (target == null)
                    target = LookForPlayer() == null ? paths[Random.Range(0, paths.Count)] : LookForPlayer();

                state = EnemyState.Moving;
                agent.SetDestination(Vector3.zero);
                break;

            case EnemyState.Moving:

                if (LookForPlayer() != null)
                {
                    target = LookForPlayer();
                    agent.SetDestination(target.transform.position);
                    break;
                }

                else if (target.CompareTag("Player") && (Mathf.Abs(Vector3.Distance(transform.position, target.transform.position)) > 3f))
                {
                    target = paths[Random.Range(0, paths.Count)];
                    agent.SetDestination(target.transform.position);

                    break;
                }

                if (Mathf.Abs(Vector3.Distance(transform.position, target.transform.position)) < 1f)
                {
                    Debug.Log("arrived");
                    target = NextPath();
                }

                break;
        }

        agent.SetDestination(target.transform.position);

    }

    private GameObject NextPath()
    {
        if (target.CompareTag("Player"))
            return target;

        if (WayPoints.FindNode(target) == null) { }
            target = paths[0];
    
        var node = WayPoints.FindNode(target);
        int rand = Random.Range(0, node.GetOutgoing().Count);

        Debug.Log(node.GetOutgoing()[1].GetData().name);
        var dest = node.GetOutgoing()[rand];
    
        return dest.GetData();
    }
    
    private GameObject LookForPlayer()

    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRadius, whatisPlayer);

        return players.Length > 0 ? players[0].gameObject : null;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("waypoint"))
    //    {
    //        agent.SetDestination(NextPath().transform.position);
    //    }
    //}

}

public enum EnemyState { Idle, Moving, Attacking}
