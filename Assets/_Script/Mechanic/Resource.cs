using System;
using UnityEngine;

public class Resource : MonoBehaviour, IResource
{
    public float Value { get => value; set => SetValue(value); }
    public float Max { get => max; }
    public float Min { get => min; }

    public event Action onChanged;
    public event Action onIncreased;
    public event Action onDecreased;
    public event Action onEmpty;
    public event Action onFull;

    private float value;
    private float max;
    private float min;

    protected virtual void SetValue(float num)
    {
        if(num == value) 
            return;
        
        if(num > value)
        {
            value = num;

            onChanged?.Invoke();
            onIncreased?.Invoke();

            if (value > max)
            {
                value = max;
                onFull?.Invoke();
            }

            return;
        }

        if(num < value)
        {
            value = num;

            onChanged?.Invoke();
            onDecreased?.Invoke();

            if(value > min)
            {
                value = min;
                onEmpty?.Invoke();
            }
        }

    }
}
