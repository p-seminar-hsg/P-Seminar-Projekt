using System.Collections;
using UnityEngine;

/// <summary>
/// Ersteller: Benedikt Wille <br/>
/// Zuletzt geändert am: 16.07.2019 <br/>
/// Dieses Script zerstört ein GameObject nach einer vorgegebenen Zeit (ab dem Spawn). <br/>
/// Kann z.B. für ParticleSystems benutzt werden.
/// </summary>
public class DespawnTimer : MonoBehaviour
{
    public float secondsToDespawn;

    private void Start()
    {
        StartCoroutine(DespawnCo());
    }

    private IEnumerator DespawnCo()
    {
        yield return new WaitForSeconds(secondsToDespawn);
        Destroy(this.gameObject);
    }
}
