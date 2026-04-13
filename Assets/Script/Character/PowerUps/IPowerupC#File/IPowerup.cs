using UnityEngine;

public interface IPowerup
{
    void Activate(GameObject gameObject);
    void Deactivate(GameObject gameObject);
    void Tick(float tickRate);
    bool IsFinished { get; }
}

