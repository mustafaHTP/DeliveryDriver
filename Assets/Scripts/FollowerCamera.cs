using UnityEngine;

public class FollowerCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float targetOffset;
    [SerializeField] private float minTargetCameraSize;
    [SerializeField] private float maxTargetCameraSize;

    private Camera _camera;
    private DriverController _driverController;
    private Rigidbody2D _carRigidBody;
    private float _topSpeed;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _driverController = target.GetComponent<DriverController>();
        _carRigidBody = target.GetComponent<Rigidbody2D>();
        _topSpeed = _driverController.TopSpeed;
    }

    private void LateUpdate()
    {
        float currentVelocity = _carRigidBody.velocity.magnitude;
        float normalizedVelocity = currentVelocity / _topSpeed;

        float currentCameraSize = Mathf.Lerp(
            minTargetCameraSize,
            maxTargetCameraSize,
            normalizedVelocity);

        _camera.orthographicSize = currentCameraSize;

        transform.position = new Vector3(
            target.transform.position.x,
            target.transform.position.y,
            targetOffset);
    }
}
