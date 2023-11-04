using UnityEngine;

public class DriverController : MonoBehaviour
{
    [Header("Speed Settings")]
    [Space(5)]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float topSpeed;
    [SerializeField] private float topSpeedLimitForce;

    [Header("Car Smoke VFX")]
    [SerializeField] private ParticleSystem vfxCarSmoke;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }
    public float TopSpeed { get => topSpeed; }

    [Header("Steer Settings")]
    [Space(5)]
    [SerializeField] private float steerSpeed;
    [Tooltip("Force that is applied when car slips in grip mode. " +
        "It should be bigger than steerHelperDriftForce")]
    [SerializeField] private float steerHelperGripForce;
    [Tooltip("Force that is applied when car slips in drift mode. " +
        "It should be smaller than steerHelperGripForce")]
    [SerializeField] private float steerHelperDriftForce;

    private Rigidbody2D _carRigidBody;

    //Controllers
    private NitroController _nitroController;
    private CarSoundController _carSoundController;

    //States
    private bool _isDrifting;

    private void Awake()
    {
        _carSoundController = GetComponent<CarSoundController>();
        _carRigidBody = GetComponent<Rigidbody2D>();
        _nitroController = GetComponent<NitroController>();
    }

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        #region DriftControl

        if (Input.GetKey(KeyCode.Space))
        {
            _isDrifting = true;
        }
        else
        {
            _isDrifting = false;
        }

        #endregion

        #region NitroControl

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _nitroController.EnableNitro();
        }
        else
        {
            _nitroController.DisableNitro();
        }

        #endregion

        Move(moveInput);
        Steer(steerInput);
        SteerHelper();

        Debug.Log("Velocity: " + _carRigidBody.velocity.magnitude);
    }

    private void Steer(float steerInput)
    {
        float normalizedSpeed = _carRigidBody.velocity.magnitude / topSpeed;
        float rotationAmount = -1f * steerInput * steerSpeed * Time.fixedDeltaTime * normalizedSpeed;

        //Check car goes backwards
        Vector3 localVelocity = transform.InverseTransformDirection(_carRigidBody.velocity);
        float localYVelocity = localVelocity.y;

        if (localYVelocity < 0f)
        {
            rotationAmount *= -1f;
        }

        Quaternion currentRotation = _carRigidBody.transform.rotation;
        Quaternion targetRotation = currentRotation
            * Quaternion.Euler(
                0f,
                0f,
                rotationAmount);

        _carRigidBody.MoveRotation(targetRotation);
    }

    /*
     * If the car slips, 
     * it applies force that opposite of velocity vector
     * **/
    private void SteerHelper()
    {
        //Check car slips
        float slipAmount = Vector3.Dot(
            _carRigidBody.velocity.normalized,
            _carRigidBody.transform.up.normalized);

        if (slipAmount < 1f)
        {
            if (_isDrifting)
            {
                _carSoundController.PlayTireScreechSound();
                PlayVFXCarSmoke();

                _carRigidBody.AddForce(
                    -_carRigidBody.velocity * steerHelperDriftForce * Time.fixedDeltaTime);
            }
            else
            {
                _carSoundController.StopTireScreechSound();
                StopVFXCarSmoke();

                _carRigidBody.AddForce(
                    -_carRigidBody.velocity * steerHelperGripForce * Time.fixedDeltaTime);
            }
        }
    }

    private bool IsExceedTopSpeed()
    {
        float currentSpeed = _carRigidBody.velocity.magnitude;
        if (currentSpeed >= topSpeed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Move(float moveInput)
    {
        bool isExceedingTopSpeed = IsExceedTopSpeed();
        if (!isExceedingTopSpeed)
        {
            _carRigidBody.AddForce(
                moveInput * moveSpeed * Time.fixedDeltaTime * _carRigidBody.transform.up);
        }
        else
        {
            _carRigidBody.AddForce(
                        topSpeedLimitForce * Time.fixedDeltaTime * -_carRigidBody.velocity);
        }
    }

    private void PlayVFXCarSmoke()
    {
        if (!vfxCarSmoke.isPlaying)
        {
            vfxCarSmoke.Play();
        }
    }

    private void StopVFXCarSmoke()
    {
        if (vfxCarSmoke.isPlaying)
        {
            vfxCarSmoke.Stop();
        }
    }
}


