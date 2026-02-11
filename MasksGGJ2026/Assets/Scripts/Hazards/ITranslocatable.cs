using UnityEngine;

public interface ITranslocatable
{
    public Vector3 TranslocatePosition();
    public void SwitchPosition(Vector3 newPosition);

}
