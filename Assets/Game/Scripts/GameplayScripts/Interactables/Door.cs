using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            print("Open");
            anim.SetBool("IsOpen", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            print("Close");
            anim.SetBool("IsOpen", false);
        }
    }
}
