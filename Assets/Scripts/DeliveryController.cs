using UnityEngine;

public class DeliveryController : MonoBehaviour
{
    [Tooltip("Color that when player has package")]
    [SerializeField] private Color hasPackageCarColor;

    private Color32 _defaultCarColor;
    private SpriteRenderer _carSpriteRenderer;

    //Controller
    CarSoundController _carSoundController;

    //States
    private bool _hasPackage;

    private void Awake()
    {
        _carSoundController = GetComponent<CarSoundController>();
        _carSpriteRenderer = GetComponent<SpriteRenderer>();
        _defaultCarColor = _carSpriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Package") && !_hasPackage)
        {
            _carSoundController.PlayPackageTakeSound();

            _carSpriteRenderer.color = hasPackageCarColor;
            _hasPackage = true;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Customer") && _hasPackage)
        {
            _carSoundController.PlayPackageDeliverSound();

            _carSpriteRenderer.color = _defaultCarColor;
            _hasPackage = false;
        }

    }
}
