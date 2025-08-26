using UnityEngine;

/// <summary>
/// Smoothly follows a target (e.g., player ship) in both position and rotation.
/// Uses linear interpolation for movement and spherical interpolation for rotation.
/// </summary>
public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;

    [SerializeField] private float _interpolationLinear;
    [SerializeField] private float _interpolationAngular;

    [SerializeField] private float _cameraOffset;
    [SerializeField] private float _forwardOffset;

    private void FixedUpdate()
    {
        if (_target == null) return;

        Vector3 camPos = transform.position;
        Vector3 targetPos = _target.position + _target.transform.up * _forwardOffset;

        Vector3 newCamPos = Vector3.Lerp(camPos, targetPos, _interpolationLinear * Time.deltaTime);

        transform.position = new Vector3(newCamPos.x, newCamPos.y, _cameraOffset);

        if (_interpolationAngular > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _target.rotation, _interpolationAngular * Time.deltaTime);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }
}

