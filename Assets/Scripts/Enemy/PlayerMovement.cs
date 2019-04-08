using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 30f;

    void Update()
    {
        //Vector3 x = new Vector3(1f, 0f, 0f);
        Vector3 y = new Vector3(0f, 1f, 0f);

        if (Input.GetKey("w"))
        {
            // Vector3.forward entspricht einem Vector3(0f,0f,1f)

            transform.Translate(y * speed * Time.deltaTime, Space.World);

        }
        if (Input.GetKey("s"))
        {
            transform.Translate( - y * speed * Time.deltaTime, Space.World);

        }
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

        }
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        }

    }
}
    
