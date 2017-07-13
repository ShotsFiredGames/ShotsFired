using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostMigration : NetworkMigrationManager
{
    public void OnHostDisconnect()
    {
        NetworkMigrationManager hostMigration = new NetworkMigrationManager();
        print(hostMigration.pendingPlayers.Count);
    }
}
