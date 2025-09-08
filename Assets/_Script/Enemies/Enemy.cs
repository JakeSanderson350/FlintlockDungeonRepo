using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour //, IVisitable
{
    //List<IVisitable> visitableComponents = new List<IVisitable>();

    [SerializeField] EnemyData enemyData;

    GameObject player;

    //public void Accept(IVisitor visitor)
    //{
        
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<NavMeshAgent>().speed = enemyData.moveSpeed;
        GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
    }

    
}
