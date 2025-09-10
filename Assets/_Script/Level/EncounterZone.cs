using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EncounterZone : MonoBehaviour
{
    [SerializeField] int waveIndex;
    [SerializeField] Transform enemyContainer;
    [SerializeField] Transform blockerContainer;

    [SerializeField] List<Spawner> spawners;
    List<IEncounterObjective> waveObjectives = new();
    //List<GameObject> blockers;
    TriggerEnter trigger;

    float delayBetweenWaves = 3f;

    private void Start()
    {
        trigger = transform.GetComponentInChildren<TriggerEnter>();
        blockerContainer.gameObject.SetActive(false);
        trigger.onTriggerEntered += StartEncounter;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var spawner in spawners) 
            if(spawner.spawnPoint != null) 
                Gizmos.DrawWireSphere(spawner.spawnPoint.position, 0.5f);
    }

    private void OnEnable()
    {
        EventManager.onEncounterObjective += CheckWave;
    }

    private void OnDisable()
    {
        EventManager.onEncounterObjective -= CheckWave;
    }

    void StartEncounter()
    {
        blockerContainer.gameObject.SetActive(true);
        trigger.gameObject.SetActive(false);
        SpawnWave(waveIndex, enemyContainer);
    }

    void SpawnWave(int index, Transform parent) //this is a bit of a mess, needs refactor
    {
        foreach (Spawner spawner in spawners)
        {
            if (!spawner.HasWave(index)) 
                continue;

            GameObject entity = Instantiate(spawner.waves.Find(x => x.index == index).enemyPrefab, spawner.spawnPoint.position, transform.rotation, parent);

            if (entity.TryGetComponent(out IEncounterObjective obj))
                waveObjectives.Add(obj);
        }
    }

    void CheckWave(IEncounterObjective obj)
    {
        if(waveObjectives.Exists(x => !x.IsComplete))
            return;
        
        StartCoroutine(WaveFinished());
    }

    IEnumerator WaveFinished()
    {
        yield return new WaitForSeconds(delayBetweenWaves);
        waveIndex++;

        if(spawners.Exists(x => x.HasWave(waveIndex)))
            SpawnWave(waveIndex, enemyContainer);
        else
            blockerContainer.gameObject.SetActive(false);
    }           
}
