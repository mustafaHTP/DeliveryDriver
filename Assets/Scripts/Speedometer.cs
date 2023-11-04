using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private GameObject car;

    [Header("Needle")]
    [Space(5)]
    [SerializeField] private GameObject needle;
    [SerializeField] private float minNeedleRotation;
    [SerializeField] private float maxNeedleRotation;

    [Header("Nitro Bar")]
    [SerializeField] private Image nitroBar;

    private float _topSpeed;
    private DriverController _driverController;
    private NitroController _nitroController;
    private Rigidbody2D _carRigidBody;

    private void Awake()
    {
        _nitroController = car.GetComponent<NitroController>();
        _carRigidBody = car.GetComponent<Rigidbody2D>();
        _driverController = car.GetComponent<DriverController>();
        _topSpeed = _driverController.TopSpeed;
    }

    private void FixedUpdate()
    {
        UpdateNeedle();
        UpdateNitroBar();
    }

    private void UpdateNitroBar()
    {
        float currentNitroAmount = _nitroController.CurrentNitroAmmount;
        float nitroCapacity = _nitroController.NitroCapacity;
        float normalizedNitroAmount = currentNitroAmount / nitroCapacity;

        nitroBar.fillAmount = normalizedNitroAmount;
    }

    private void UpdateNeedle()
    {
        float currentVelocity = _carRigidBody.velocity.magnitude;
        float normalizedVelocity = currentVelocity / _topSpeed;
        float needleRotation = Mathf.Lerp(maxNeedleRotation, minNeedleRotation, normalizedVelocity);

        needle.transform.localEulerAngles =
            new Vector3(0f, 0f, needleRotation);
    }
}
