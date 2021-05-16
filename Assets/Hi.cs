using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hi : MonoBehaviour
{
    public GameObject hidiho;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hidiho.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hidiho.SetActive(false);
    }
}
