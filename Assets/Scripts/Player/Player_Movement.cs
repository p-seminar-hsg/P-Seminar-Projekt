using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Ersteller: Florian Müller-Martin und Tobias Schwarz
 * Zuletzt geändert am 23.05.2019
 * Funktion: Dieses Skript ist für die Bewegung des Players mithilfe des Joysticks zuständig
 */

public class Player_Movement : MonoBehaviour
{
    public Joystick joystick; // Referenz zum GameObject MovementJoystick
    public float speed; // Geschwindigkeit des Players
    private Rigidbody2D rb; // Rigidbody des Players
    private Animator animator; // Animator des Players
    

    // Start wird einmal bei Erstellung des GameObjects aufgerufen
    // Referenzen zu den Components werden hergestellt
    void Start()
    {
        rb = GameObject.Find("Player").GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        animator = GameObject.Find("Player").GetComponent(typeof(Animator)) as Animator;
    }

    // FixedUpdate wird einmal pro Frame aufgerufen und fragt jedes mal die Position des Joysticks ab. Diese Position wird dann in eine Bewegung für den Player umgerechnet.
    void FixedUpdate()
    {
        //Die Geschwindigkeit des Rigidbodys wird je nach position des Joysticks eingestellt
        rb.velocity = new Vector2(Time.deltaTime * joystick.Horizontal * speed, Time.deltaTime * joystick.Vertical * speed);

        //Die Parameter des Animators werden aktualisiert
        animator.SetFloat("speed_horizontal", joystick.Horizontal);
        animator.SetFloat("speed_vertical", joystick.Vertical);
    }


    //OnCollsionEnter wird ausgelöst, wenn der Player eine collision detektiert. Wenn der collider als Boundary getagged ist, dann wird die Bewegung des Players eingeschränkt.
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Bei einer Collision mit einem als "boundaryX" getaggten GameObject wird die x-Bewegung eingeschränkt
        if (collision.collider.tag == "boundaryX") {

            rb.velocity = new Vector2(0, Time.deltaTime * joystick.Vertical * speed);
            Debug.Log("Collision with boundaryX");
        }

        //Bei einer Collision mit einem als "boundaryX" getaggten GameObject wird die x-Bewegung eingeschränkt
        if (collision.collider.tag == "boundaryY")
        {

            rb.velocity = new Vector2(Time.deltaTime * joystick.Horizontal * speed, 0);
            Debug.Log("Collision with boundaryY");
        }
    }

    //Die Geschwindigkeit des Players erhöht sich um den übergebenen Wert

    /// <summary>
    /// Methode, um die Geschwindigkeit des Players zu verändern
    /// </summary>
    /// <param name="newSpeed">Wert, auf den die Geschwindigkeit gesetzt werden soll</param>
   public void setSpeed (float newSpeed)
    {
        speed += newSpeed;
    }

}
