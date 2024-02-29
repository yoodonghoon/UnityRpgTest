using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;

    public float height = 4;
    public float distance = 4;
    public float angle = 45;
    public float lookAtHeight = 2;
    public float smoothSpeed = 0.5f;

    void LateUpdate()
    {
        if (!target)
        {
            return;
        }

        Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height);
        Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
        Vector3 flatTargetPosition = target.position;
        flatTargetPosition.y += lookAtHeight;

        Vector3 finalPosition = flatTargetPosition + rotatedVector;
        transform.position = finalPosition;

        transform.LookAt(target.position);
    }
}
