using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Flag : MonoBehaviour
{
    public Coroutine resetTimer;
    public PlayerManager carrier;
    public float flagResetTime;
    public FlagBase flagBase;

    [HideInInspector]
    public bool isPickedUp;
    public GameObject spawnPosition { get; set; }
    public byte index { get; set; }

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
            if (!isPickedUp)
            {
                if (!other.GetComponent<PlayerManager>().hasFlag)
                {
                    if (!flagBase.owner.Equals(other.GetComponent<PlayerManager>()))
                    {
                        isPickedUp = true;
                        FlagManager.instance.CmdFlagPickedUp(index, other.transform.root.name);
                    }
                    else
                    {
                        if (!flagBase.hasFlag)
                        {
                            ResetFlagPosition();
                            Debug.LogError("returned your flag");
                        }
                        else
                        {
                            Debug.LogError("This is your flag at base");
                        }
                    }
                }
                else
                {
                    Debug.LogError("They already have a flag");
                }
            }
            else
            {
                print("someone already has it");
            }

        }
    }

    public IEnumerator ResetTimer()
    {
        ResetCarrier();
        yield return new WaitForSeconds(flagResetTime);
        FlagManager.instance.CmdReturnFlag(index);
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
