using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    [Header("Engine Sound")]
    [Space(5)]
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    [Header("Nitro Sound")]
    [Space(5)]
    [SerializeField] private AudioSource nitroSound;

    [Header("Tire Screech Sound")]
    [Space(5)]
    [SerializeField] private AudioSource tireScreechSound;

    [Header("Delivery Sounds")]
    [Space(5)]
    [SerializeField] private AudioClip packageTakeSound;
    [SerializeField] private AudioClip packageDeliverySound;

    private DriverController _driverController;
    private AudioSource _carAudioSource;
    private Rigidbody2D _carRigidBody;
    private float _topSpeed;

    private void Awake()
    {
        _carRigidBody = GetComponent<Rigidbody2D>();
        _carAudioSource = GetComponent<AudioSource>();
        _driverController = GetComponent<DriverController>();
        _topSpeed = _driverController.TopSpeed;
    }

    private void FixedUpdate()
    {
        PlayEngineSound();
    }

    private void PlayEngineSound()
    {
        float currentVelocity = _carRigidBody.velocity.magnitude;
        float normalizedVelocity = currentVelocity / _topSpeed;

        float pitchValue = Mathf.Lerp(minPitch, maxPitch, normalizedVelocity);

        engineSound.pitch = pitchValue;
    }

    public void PlayNitroSound()
    {
        if (!nitroSound.isPlaying)
        {
            nitroSound.Play();
        }
    }

    public void StopNitroSound()
    {
        if (nitroSound.isPlaying)
        {
            nitroSound.Stop();
        }
    }

    public void PlayTireScreechSound()
    {
        if (!tireScreechSound.isPlaying)
        {
            tireScreechSound.Play();
        }
    }

    public void StopTireScreechSound()
    {
        if (tireScreechSound.isPlaying)
        {
            tireScreechSound.Stop();
        }
    }

    public void PlayPackageTakeSound()
    {
        _carAudioSource.PlayOneShot(packageTakeSound);
    }

    public void PlayPackageDeliverSound()
    {
        _carAudioSource.PlayOneShot(packageDeliverySound);
    }
}
