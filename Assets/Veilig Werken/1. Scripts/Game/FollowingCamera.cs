using MBevers;
using UnityEngine;
using VeiligWerken;

/// <summary>
///     This class makes the attached camera follow a target.
///     And it makes sure the camera stays in bounds.
///     <para>Created by Mathias on 13-05-2021</para>
/// </summary>
public class FollowingCamera : ExtendedMonoBehaviour
{
    private const float CAMERA_START_Z = -10.0f;
    [SerializeField] [Required] private SpriteRenderer map;

    private bool shouldFollowTarget = false;
    private new Camera camera = null;
    private Transform targetTransform = null;
    private Vector3 minBounds = new Vector3();
    private Vector3 maxBounds = new Vector3();

    private void Awake()
    {
        camera = ForceComponent<Camera>();
        
        float halfHeight = camera.orthographicSize;
        float halfWidth = halfHeight * camera.aspect;
        
        minBounds = map.bounds.min + new Vector3(halfWidth, halfHeight, 0.0f);
        maxBounds = map.bounds.max - new Vector3(halfWidth, halfHeight, 0.0f);

        GameManager.Instance.PlayerSpawnedEvent += player =>
        {
            targetTransform = player.transform;
            shouldFollowTarget = true;
        };
    }

    private void LateUpdate()
    {
        if(!shouldFollowTarget) { return; }

        Vector3 targetPosition = targetTransform.position;
        CachedTransform.position = new Vector3(targetPosition.x, targetPosition.y, CAMERA_START_Z);

        float x = Mathf.Clamp(CachedTransform.position.x, minBounds.x, maxBounds.x);
        float y = Mathf.Clamp(CachedTransform.position.y, minBounds.y, maxBounds.y);

        CachedTransform.position = new Vector3(x, y, CAMERA_START_Z);
    }
}