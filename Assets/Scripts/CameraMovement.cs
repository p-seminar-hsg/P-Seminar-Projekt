using UnityEngine;

/// <summary>
/// Ersteller: Benedikt Wille <br/>
/// Zuletzt geändert am: 26.05.2019 <br/>
/// Dieses Script steuert die Kamerabewegung.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    // Das Ziel, dem die Kamera folgt (der Transform vom Spieler)
    public Transform target;
    public float height;
    public float smoothing;

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
