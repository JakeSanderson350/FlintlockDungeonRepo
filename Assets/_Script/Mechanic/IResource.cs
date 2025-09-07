using System;

public interface IResource 
{
    public float Value { get; set; }
    public float Max { get; }
    public float Min { get; }

    public event Action onChanged;
    public event Action onIncreased;
    public event Action onDecreased;
    public event Action onEmpty;
    public event Action onFull;
}
