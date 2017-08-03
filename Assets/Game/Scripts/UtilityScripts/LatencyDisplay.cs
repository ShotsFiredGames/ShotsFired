using UnityEngine;
using UnityEngine.UI;

public class LatencyDisplay : Photon.MonoBehaviour {

	#region Latency Variables
	private int latency;
	[SerializeField]
	Text latencyText;
	#endregion
	
	void Update () {
		ShowLatency();
	}

	void ShowLatency()
	{
		if (photonView.isMine)
		{
			latency = PhotonNetwork.GetPing();
			latencyText.text = latency.ToString();
		}
	}
}
