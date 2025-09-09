using System;
using UnityEngine;

public class Resource : MonoBehaviour, IResource
{
    public float Value { get => value; set => SetValue(value); }
    public float Percent { get => Mathf.InverseLerp(min, max, value); set => SetValue(Mathf.Lerp(min, max, value)); }
    public float Max { get => max; }
    public float Min { get => min; }

                                                            /// <summary> (Value, Change, Percent) </summary>
    public event Action<float, float, float> onChanged;     /// <summary> (Value, Change, Percent) </summary>
    public event Action<float, float, float> onIncreased;   /// <summary> (Value, Change, Percent) </summary>
    public event Action<float, float, float> onDecreased;
    public event Action onEmpty;
    public event Action onFull;

    private float value;
    private float max;
    private float min;

    [SerializeField] ResourceStats stats;

    private void Awake()
    {
        value = stats.startValue;
        max = stats.max;
        min = stats.min;
    }


    protected virtual void SetValue(float num)
    {
        if(num == value) 
            return;

        float dif = num - value;

        if(num > value)
        {
            value = num;

            onChanged?.Invoke(num, dif, Percent);
            onIncreased?.Invoke(num, dif, Percent);

            if (value >= max)
            {
                value = max;
                onFull?.Invoke();
            }

            return;
        }

        if(num < value)
        {
            value = num;

            onChanged?.Invoke(num, dif, Percent);
            onDecreased?.Invoke(num, dif, Percent);

            if(value <= min)
            {
                value = min;
                onEmpty?.Invoke();
            }

            return;
        }

    }
}
