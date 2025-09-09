using ImprovedTimers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenerating : Health
{
    [SerializeField] float regenPerSecond;
    FrequencyTimer tick = new(2);

    private void Start()
    {
        tick.OnTick += RegenTick;
        tick.Start();
    }

    void RegenTick()
    {
        if (Value < Max)
            Value += regenPerSecond / (float)tick.TicksPerSecond;
    }

    private void OnDestroy()
    {
        tick.Dispose();
    }
}
