
using UnityEngine;

public class coll : MonoBehaviour
{ 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { Destroy(transform.parent.gameObject);
            //Item-Effekte
        }
    }

}
