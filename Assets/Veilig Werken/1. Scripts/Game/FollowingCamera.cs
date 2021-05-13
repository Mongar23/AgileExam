using MBevers;
using UnityEngine;

public class FollowingCamera : ExtendedMonoBehaviour
{
    private const float CAMERA_START_Z = -10.0f;

    [SerializeField] [Required] private Transform targetTransform;

    private void LateUpdate()
    {
        Vector3 targetPosition = targetTransform.position;
        CachedTransform.position = new Vector3(targetPosition.x, targetPosition.y, CAMERA_START_Z);
    }
}