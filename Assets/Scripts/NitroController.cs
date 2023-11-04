using UnityEngine;

public class NitroController : MonoBehaviour
{
    [Header("Nitro Settings")]
    [Space(5)]
    [SerializeField] private float nitroCapacity;
    [SerializeField] private float nitroConsumeAmount;
    [SerializeField] private float nitroFillAmount;
    [SerializeField] private float nitroRechargeDelay;
    [Range(1f, 3f)]
    [Tooltip("It is multiplied by move speed")]
    [SerializeField] private float nitroSpeedMultiplier;

    [Header("Nitro VFX")]
    [SerializeField] private ParticleSystem vfxNitro;
    [SerializeField] private ParticleSystem vfxExhaust;

    private float _currentNitroAmount;
    private float _defaultMoveSpeed;
    private float _nitroMoveSpeed;

    public float CurrentNitroAmmount { get => _currentNitroAmount; }
    public float NitroCapacity { get => nitroCapacity; }

    //Controllers
    private DriverController _driverController;
    private CarSoundController _carSoundController;


    private void Awake()
    {
        _currentNitroAmount = nitroCapacity;

        _carSoundController = GetComponent<CarSoundController>();
        _driverController = GetComponent<DriverController>();
        _defaultMoveSpeed = _driverController.MoveSpeed;
        _nitroMoveSpeed = _defaultMoveSpeed * nitroSpeedMultiplier;
    }

    private void FixedUpdate()
    {
        Debug.Log("Nitro amount: " + _currentNitroAmount);
    }

    public void EnableNitro()
    {
        if (_currentNitroAmount - nitroConsumeAmount > 0f)
        {
            StopVFXExhaust();
            PlayVFXNitro();

            _carSoundController.PlayNitroSound();

            _driverController.MoveSpeed = _nitroMoveSpeed;

            ConsumeNitro();
        }
        else
        {
            StopVFXNitro();
            PlayVFXExhaust();

            _carSoundController.StopNitroSound();

            _driverController.MoveSpeed = _defaultMoveSpeed;

            Invoke(nameof(FillNitro), nitroRechargeDelay);
        }
    }

    public void DisableNitro()
    {
        StopVFXNitro();
        PlayVFXExhaust();

        _carSoundController.StopNitroSound();

        _driverController.MoveSpeed = _defaultMoveSpeed;

        FillNitro();
    }

    private void ConsumeNitro()
    {
        _currentNitroAmount -= nitroConsumeAmount * Time.deltaTime;
        _currentNitroAmount = _currentNitroAmount < 0f ? 0f : _currentNitroAmount;
    }

    private void FillNitro()
    {
        _currentNitroAmount += nitroFillAmount * Time.deltaTime;
        _currentNitroAmount = _currentNitroAmount > nitroCapacity ? nitroCapacity : _currentNitroAmount;
    }

    private void PlayVFXNitro()
    {
        if (!vfxNitro.isPlaying)
        {
            vfxNitro.Play();
        }
    }

    private void StopVFXNitro()
    {
        if (vfxNitro.isPlaying)
        {
            vfxNitro.Stop();
        }
    }

    private void PlayVFXExhaust()
    {
        if (!vfxExhaust.isPlaying)
        {
            vfxExhaust.Play();
        }
    }

    private void StopVFXExhaust()
    {
        if (vfxExhaust.isPlaying)
        {
            vfxExhaust.Stop();
        }
    }
}
