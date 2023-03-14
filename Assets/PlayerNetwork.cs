using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1);
    // Start is called before the first frame update
    private void Update()
    {

        Debug.Log(OwnerClientId + "; randomNumber " + randomNumber.Value);
        if (!IsOwner) return;
        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = Random.Range(0, 100);
        }

        if (Input.GetKey(KeyCode.U)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.J)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.H)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.K)) moveDir.x = +1f;

        float movepeed = 3f;
        transform.position += moveDir * movepeed * Time.deltaTime;
    }
}
