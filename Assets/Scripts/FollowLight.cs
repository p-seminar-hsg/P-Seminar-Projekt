using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel <br/>
/// Zuletzt geändert am: 08.08.2019 <br/>
/// Dieses Script lässt ein Licht dem Player folgen.
/// </summary>
public class FollowLight : MonoBehaviour
{
    public Transform target;
    public float height;
    public float smoothing;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        if (transform.position != target.position)
        {
            Vector3 targetPosition2D = new Vector3(target.position.x,
                                                   target.position.y,
                                                   height);

            transform.position = Vector3.Lerp(transform.position, targetPosition2D, smoothing);
        }
    }
}
