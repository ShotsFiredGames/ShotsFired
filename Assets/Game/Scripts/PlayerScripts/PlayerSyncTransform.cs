using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[NetworkSettings(channel = 0, sendInterval = 0.033f)]
public class PlayerSyncTransform : NetworkBehaviour {

    [SyncVar]
    Vector3 syncPos;
    [SyncVar]
    float syncRot;
    [SerializeField]
    float lerpRate = 15;

    Vector3 lastPos;
    float lastRot;
    float posThreshold = 0.5f;
    float rotThreshold = 5;

    #region Latency Variables
    private NetworkClient nClient;
    private int latency;
    [SerializeField]
    Text latencyText;
    #endregion

    void Start()
    {
        nClient = GameObject.Find("LobbyManager").GetComponent<NetworkManager>().client;
    }

    void Update()
    {
        LerpPlayer();
        ShowLatency();
    }

    void FixedUpdate()
    {
        TransmitPosToServer();
        TransmitRotToServer();
    }

    void LerpPlayer()
    {
        if (!isLocalPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, syncPos, Time.deltaTime * lerpRate);
            LerpPlayerRot(syncRot);
            //transform.rotation = Quaternion.Lerp(transform.rotation, syncRot, Time.deltaTime * lerpRate);
        }
    }

    void LerpPlayerRot(float newAngle)
    {
        Vector3 newRot = new Vector3(0, newAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newRot), Time.deltaTime * lerpRate);
    }

    [Command]
    void CmdSendPosition(Vector3 playerPos)
    {
        syncPos = playerPos;
    }

    [Command]
    void CmdSendRotation(float playerRot)
    {
        syncRot = playerRot;
    }

    [ClientCallback]
    void TransmitPosToServer()
    {
        if (isLocalPlayer && Vector3.Distance(transform.position, lastPos) > posThreshold)
        {
            CmdSendPosition(transform.position);
            lastPos = transform.position;
        }
    }

    [ClientCallback]
    void TransmitRotToServer()
    {
        if (isLocalPlayer && CheckIfBeyondThreshold(transform.localEulerAngles.y, lastRot))
        {
            lastRot = transform.localEulerAngles.y;
            CmdSendRotation(lastRot);            
        }
    }

    [ClientCallback]
    void ShowLatency()
    {
        if (isLocalPlayer)
        {
            latency = nClient.GetRTT();
            latencyText.text = latency.ToString();
        }
    }

    bool CheckIfBeyondThreshold(float currentRot, float lastRot)
    {
        if (Mathf.Abs(currentRot - lastRot) > rotThreshold)
            return true;
        else
            return false;
    }
}