using ImprovedTimers;
using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour , IEncounterObjective
{
    //List<IVisitable> visitableComponents = new List<IVisitable>();

    [SerializeField] EnemyData stats;
    [SerializeField] Transform model;

    Transform target;
    Health health;
    NavMeshAgent navAgent;

    CountdownTimer reattack = new(3f);
    bool trackingTarget;
    bool isDead;

    public bool IsComplete => isDead;

    //public void Accept(IVisitor visitor)
    //{

    //}

    private void Awake()
    {
        target = FindAnyObjectByType<Player>().transform; 

        //nav
        navAgent = GetComponent<NavMeshAgent>();
        trackingTarget = true;
        navAgent.speed = stats.moveSpeed;
        navAgent.SetDestination(target.transform.position);

        //health
        health = GetComponent<Health>();
        health.onEmpty += Die;

        //other
        GetComponents<SphereCollider>()[0].radius = stats.attackRange;
        reattack.OnTimerStop += PersueTarget;
    }

    private void FixedUpdate()
    {
        if (trackingTarget && target != null)
            navAgent.SetDestination(target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {
            Attack();
        }
    }
    
    private void Attack()
    {
        //Debug.Log("Bat is attacking!");
        if(target.TryGetComponent(out Health hp))
            hp.Value -= stats.damage;

        SetRandomDestination();
    }

    void SetRandomDestination()
    {
        trackingTarget = false;

        Vector3 random = Random.insideUnitSphere;
        random.y = 0;
        random = random.normalized;
        random *= stats.retreatDistance;
        navAgent.SetDestination(transform.position + random);
        reattack.Start();
    }

    void PersueTarget()
    {
        trackingTarget = true;
    }

    private void Die()
    {
        if(isDead) return;

        isDead = true;
        EnableComponents(false);
        EventManager.EncounterObjective(this);

        Tween.ScaleY(model, 2, 2f, Ease.OutQuart);
        Tween.ScaleX(model, 0, 2f, Ease.OutQuart).OnComplete(() => Hide());
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    void EnableComponents(bool state)
    {
        navAgent.isStopped = !state;
        this.enabled = state;
        GetComponents<Collider>().ToList().ForEach(c => c.enabled = state);
    }
}
