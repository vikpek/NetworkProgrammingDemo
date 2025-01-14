using Unity.Netcode;
using UnityEngine;

public class MovementNetworkController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new(writePerm: NetworkVariableWritePermission.Server);

    private void Awake()
    {
        Position.OnValueChanged += OnPositionChanged;
    }

    private void OnDestroy()
    {
        Position.OnValueChanged -= OnPositionChanged;
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(Vector3 position)
    {
        Position.Value = position;
    }

    private void Update()
    {
        if (IsOwner)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveX, 0f, moveZ) * 5f * Time.deltaTime;
            transform.Translate(movement, Space.World);

            // Send the new position to the server
            SubmitPositionRequestServerRpc(transform.position);
        }

        if (!IsOwner && !IsServer)
        {
            // Ensure the position is updated for non-owners
            transform.position = Position.Value;
        }
    }

    private void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        if (!IsOwner)
        {
            transform.position = newPosition;
        }
    }
}