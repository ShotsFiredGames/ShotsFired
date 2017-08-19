using UnityEngine;
using System.Collections;

public class Flag : Photon.MonoBehaviour
{
    public Coroutine resetTimer;
    public PlayerFlagInfo carrier;
    public float flagResetTime;
    public FlagBase flagBase;

    [HideInInspector]
    public bool isPickedUp;
    public GameObject spawnPosition { get; set; }
    public byte index { get; set; }

    PlayerFlagInfo newCarrier;

    void OnEnable()
    {
        ResetFlagPosition();
    }

    void OnDisable()
    {
        if (carrier != null)
            carrier.hasFlag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            newCarrier = other.GetComponent<PlayerFlagInfo>();

			if (!isPickedUp)
            {
				if (other.gameObject.GetComponent<PlayerManager>().CheckAbilityToPickupFlag())
                {
					if (!flagBase.owner.name.Equals (newCarrier.name))
                    {
                        Debug.LogError("Pick up flag");
						isPickedUp = true;
						FlagManager.instance.photonView.RPC ("RPC_FlagPickedUp", PhotonTargets.All, index, other.transform.root.name);
					}
                    else
                    {
						if (!flagBase.hasFlag)
                        {
							ResetFlagPosition ();
							//Debug.LogError("returned your flag");
						}
                        else
                        {
							//Debug.LogError ("This is your flag at base");
						}
					}
				} else {
					//Debug.Log ("They already have a flag");
				}
			} else {
				//print ("someone already has it");
			}
        }
    }

    public IEnumerator ResetTimer()
    {
        ResetCarrier();
        yield return new WaitForSeconds(flagResetTime);
		FlagManager.instance.photonView.RPC("RPC_ReturnFlag", PhotonTargets.All, index);
    }

    public IEnumerator CanBePickedUp()
    {
        ResetCarrier();
        yield return new WaitForSeconds(2);
        isPickedUp = false;
    }

    public string GetStringOfCarrier()
    {
        if (carrier == null) return "No One";
        return carrier.name;
    }

    public void ResetFlagPosition()
    {
        transform.parent = spawnPosition.transform;
        transform.position = spawnPosition.transform.position + new Vector3(0, 1, 0);

        if (flagBase != null)
            flagBase.hasFlag = true;

        if (carrier != null)
            carrier.hasFlag = false;
    }

    void ResetCarrier()
    {
        if (carrier != null)
        {
            carrier.hasFlag = false;
            carrier = null;
        }
    }
}