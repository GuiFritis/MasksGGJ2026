using UnityEngine;

public interface ITranslocatableMovingObject
{
    public Vector3 StartPos { get; }
    public Vector3 EndPos { get; }

    public void ResetPosition();

}
