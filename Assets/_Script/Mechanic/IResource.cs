using System;

public interface IResource 
{
    public float Value { get; set; }
    public float Percent { get; set; }
    public float Max { get; }
    public float Min { get; }

    public event Action<float, float, float> onChanged;
    public event Action<float, float, float> onIncreased;
    public event Action<float, float, float> onDecreased;
    public event Action onEmpty;
    public event Action onFull;
}
