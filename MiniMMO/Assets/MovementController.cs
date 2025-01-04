using Unity.Netcode;
using UnityEngine;

public class MovementController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    

    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(Vector3 position, RpcParams rpcParams = default)
    {
        Position.Value = position;
    }

    public float speed = 5f;
    void Update()
    {
        if (IsOwner && !IsServer)
        {
            float moveX = Input.GetAxis("Horizontal"); // A, D or Left, Right
            float moveZ = Input.GetAxis("Vertical"); // W, S or Up, Down

            Vector3 movement = new Vector3(moveX, 0f, moveZ) * speed * Time.deltaTime;
            transform.Translate(movement, Space.World);
            SubmitPositionRequestServerRpc(transform.position);
        }

        if (IsServer)
        {
            transform.position = Position.Value;
        }
    }
}