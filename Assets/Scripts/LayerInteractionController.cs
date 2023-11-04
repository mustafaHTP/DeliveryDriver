using UnityEngine;

public class LayerInteractionController : MonoBehaviour
{
    Rigidbody2D _carRigidBody;

    int groundLayerIndex;
    int upperLayerIndex;
    int playerLayerIndex;

    int groundLayerMask;
    int upperLayerMask;

    private void Awake()
    {
        _carRigidBody = GetComponent<Rigidbody2D>();

        upperLayerIndex = LayerMask.NameToLayer("Upper");
        groundLayerIndex = LayerMask.NameToLayer("Ground");
        playerLayerIndex = LayerMask.NameToLayer("Player");

        upperLayerMask = 1 << upperLayerIndex;
        groundLayerMask = 1 << groundLayerIndex;
    }

    /*
     * When player is on Upper Layer, 
     * player must not interact with Ground Layer, vice versa
     * **/
    private void FixedUpdate()
    {
        RaycastHit2D raycastHit2D;
        raycastHit2D = Physics2D.Raycast(
            _carRigidBody.transform.position,
            _carRigidBody.transform.forward,
            10f,
            upperLayerMask);

        if (raycastHit2D.collider != null)
        {
            Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(playerLayerIndex, groundLayerIndex, false);
        }

        raycastHit2D = Physics2D.Raycast(
            _carRigidBody.transform.position,
            _carRigidBody.transform.forward,
            10f,
            groundLayerMask);

        if (raycastHit2D.collider != null)
        {
            Physics2D.IgnoreLayerCollision(playerLayerIndex, upperLayerIndex, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(playerLayerIndex, upperLayerIndex, false);
        }
    }
}
