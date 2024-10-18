using UnityEngine;
using UnityEngine.Networking;

public class PlayerDance : NetworkBehaviour
{
    public GameObject defaultModel;
    public GameObject danceModel;

    void Update()
    {
        // Check for a key press to start dancing (e.g., 'D' key)
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Tab))
        {
            CmdStartDancing();
        }
    }

    [Command]
    void CmdStartDancing()
    {
        RpcStartDancing();
        // Schedule to stop dancing after 4 seconds
        Invoke(nameof(CmdStopDancing), 4f);
    }

    [ClientRpc]
    void RpcStartDancing()
    {
        defaultModel.SetActive(false);
        danceModel.SetActive(true);
    }

    [Command]
    void CmdStopDancing()
    {
        RpcStopDancing();
    }

    [ClientRpc]
    void RpcStopDancing()
    {
        danceModel.SetActive(false);
        defaultModel.SetActive(true);
    }
}
