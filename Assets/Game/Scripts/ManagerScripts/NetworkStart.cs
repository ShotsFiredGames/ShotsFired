using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkStart : MonoBehaviour {

    int myReliableChannelID;
    int socketID;
    int socketPort = 7777;
    int connectionID;
    
	void Start () {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        myReliableChannelID = config.AddChannel(QosType.Reliable);
        int maxConnections = 6;
        HostTopology topology = new HostTopology(config, maxConnections);
        socketID = NetworkTransport.AddHost(topology, socketPort);
        Debug.Log("Socket Open. SocketId is: " + socketID);
    }
	
	public void Connect()
    {
        byte error;
        connectionID = NetworkTransport.Connect(socketID, "localhost", socketPort, 0, out error);
        Debug.Log("Connected to server. ConnectionId: " + connectionID);
    }
}
