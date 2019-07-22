﻿
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Florian Müller-Martin und Tobias Schwarz
/// Zuletzt geändert am 16.07.2019
/// Mainklasse für den Player
/// </summary>
public class Player_Main : MonoBehaviour
{

    [Header("Stats")]
    static float HP; //Aktuelle Lebenspunkte des Players // WarumIstDasNichtStatic(EineAnmerkungVonR.Jokiel)
    public float maxHP; //Maximale Lebenspunkte des Charakters
    public float strength; //Angriffswert des Players

    [Header("Cooldown")]
    bool attackCooldownBool; //Entscheidend, ob ein Angriff ausgeführt werden kann, oder nich
    public float attackCooldownLength;    


    [Header("Unity Stuff")]
    public Image healthBarPauseMenu;//Referenz zur Healthbar im PauseMenu (Geändert von Rene Jokiel)
    private Image healthBar; //Referenz zur HealthBar
    private Animator animator; // Animator des Players
    public GameOver gameOver;   // Die GameOver UI 


    private bool changingColor; //ist der Player bereits rötlich gefärbt?


    void Awake()
    {
        changingColor = false;
    }


    private void Start()
    {
        HP = maxHP;
        healthBar = GameObject.Find("Bar").GetComponent<Image>(); //Referenz wird hergestellt
        animator = GameObject.Find("Player").GetComponent(typeof(Animator)) as Animator;
        attackCooldownBool = true;
    }

    /// <summary>
    /// Schadensmethode des Players - Reduziert die HP des Players um den übergebenen Wert und stößt den Player in die übergebene Richtung weg
    /// </summary>
    /// <param name="damage">Der Schadenswert, der zugeführt werden soll</param>
    /// <param name="knockbackDirection">Richtung des Knockbacks</param>
    public void takeHit(GameObject enemy)
    {
        if (HP > 0 && enemy.CompareTag("Enemy"))
        {
            HP -= enemy.GetComponent<Enemy>().strength;
            StartCoroutine(ColorChangeForSeconds(0.35f));
        }
        if(HP > 0 && enemy.CompareTag("Projectile"))
        {
            HP -= enemy.GetComponent<Projectile>().strength;
            StartCoroutine(ColorChangeForSeconds(0.35f));
        }

        //kein Knockback bei GameOver
        if(HP > 0 && enemy.CompareTag("Enemy"))
        {
        StartCoroutine(GetComponent<Player_Movement>().KnockbackCo((transform.position - enemy.transform.position), enemy.GetComponent<Enemy>().attackKnockback));
        }
    }

    /// <summary>
    /// Heilmethode des Players - Erhöht die HP des Players um den übergebenen Wert
    /// </summary>
    /// <param name="heal">Der Heilwert, der zugeführt werden soll</param>
    public void heal(float heal)
    {
        HP += heal;
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        Debug.Log("Health set to: " + HP);
    }

    /// <summary>
    /// Stärkemethode des Players - Erhöht den Schadenswert des Players um den übergebenen Wert
    /// </summary>
    /// <param name="strengthening">Der Schadenswert, der zugeführt werden soll</param>
    public void strengthen(float strengthening)
    {
        strength += strengthening;
    }

    /// <summary>
    /// Sterbemethode des Players
    /// </summary>
    public void die()
    {
        animator.SetFloat("attack", 0f);
        gameOver.GoGameOver();  //Die GameOver UI wird getriggert. Spiel Vorbei (Von Rene Jokiel)
        Destroy(this);

    }


    /// <summary>
    /// Die passende Attack-Hitbox wird aktiviert
    /// </summary>
    public void attackMain()
    {
        if (attackCooldownBool)
        {
            StartCoroutine("attackCooldownCoroutine");
            float moveX = GameObject.Find("Player").GetComponent<Player_Movement>().moveX;
            float moveY = GameObject.Find("Player").GetComponent<Player_Movement>().moveY;

            float absMoveX = Mathf.Abs(moveX);
            float absMoveY = Mathf.Abs(moveY);

            string viewDirection = getViewDirection();

            if (viewDirection == "right")
            {
                // Spieler schaut nach rechts
                StartCoroutine("attackRight");
                Debug.Log("Debug von Flo: Angriff nach Rechts");
            }

            else if (viewDirection == "top")
            {
                // Spieler schaut nach oben
                StartCoroutine("attackTop");
                Debug.Log("Debug von Flo: Angriff nach Oben");
            }

            else if (viewDirection == "left")
            {
                // Spieler schaut nach links
                StartCoroutine("attackLeft");
                Debug.Log("Debug von Flo: Angriff nach Links");

            }

            else if (viewDirection == "bot")
            {
                // Spieler schaut nach unten
                StartCoroutine("attackBottom");
                Debug.Log("Debug von Flo: Angriff nach Unten");
            }

        }
        

    }


    //ändert die Farbe des Player Sprites für eine bestimmte Zeit, sodass er rötlich eingefärbt wird
    //von Luca Kellermann am 18.07.2019
    private IEnumerator ColorChangeForSeconds(float time){
        if(!changingColor){
            changingColor  = true;
            SpriteRenderer playerSprite = gameObject.GetComponent<SpriteRenderer>();
            playerSprite.color = new Color(1f, 0.3f, 0.3f);
            yield return new WaitForSeconds(time);
            playerSprite.color = new Color(1f, 1f, 1f);
            changingColor = false;
        }
    }
    
    private IEnumerator attackCooldownCoroutine()
    {
        attackCooldownBool = false;

        yield return Utility.Wait(attackCooldownLength);

        attackCooldownBool = true;
    }

    private IEnumerator attackBottom()
    {
        animator.SetFloat("attack", 1);
        GameObject hitboxGO = transform.Find("hitboxBottom").gameObject;
        hitboxGO.SetActive(true);
        yield return Utility.Wait(0.32f);
        animator.SetFloat("attack", 0);
        hitboxGO.SetActive(false);

    }

    private IEnumerator attackRight()
    {
        animator.SetFloat("attack", 2);
        GameObject hitboxGO = transform.Find("hitboxRight").gameObject;
        hitboxGO.SetActive(true);
        yield return Utility.Wait(0.32f);
        animator.SetFloat("attack", 0);
        hitboxGO.SetActive(false);

    }

    private IEnumerator attackTop()
    {
        GameObject hitboxGO = transform.Find("hitboxTop").gameObject;
        hitboxGO.SetActive(true);
        animator.SetFloat("attack", 3);        
        yield return Utility.Wait(0.32f);
        animator.SetFloat("attack", 0);
        hitboxGO.SetActive(false);

    }

    private IEnumerator attackLeft()
    {
        GameObject hitboxGO = transform.Find("hitboxLeft").gameObject;
        hitboxGO.SetActive(true);
        animator.SetFloat("attack", 4);        
        yield return Utility.Wait(0.32f);
        animator.SetFloat("attack", 0);
        hitboxGO.SetActive(false);

    }

    // FixedUpdate wird einmal pro Frame aufgerufen
    private void FixedUpdate()
    {
        //Wenn der Player keine Leben mehr hat wird die Methode die() aufgerufen
        if (HP <= 0)
        {
            die();
        }

        //Die Health Bar wird aktualisiert
        healthBar.fillAmount = HP / maxHP;
        healthBarPauseMenu.fillAmount = HP / maxHP;// Von Rene Jokiel

        //Selbstgeißelung auf Leertaste zu Demonstrationszwecken
        if (Input.GetKey(KeyCode.Space))
        {
            HP -= 1;
        }
    }


    //Nur für Testzwecke
    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.CompareTag("Enemy"))
        {
            //Debug.Log("Debug von Flo: Collision with Enemy");
        }
    }

    /// <summary>
    /// Diese Methode gibt die aktuelle Blickrichtung des Players als String (right, left, bot, top) zurück
    /// </summary>
    public static string getViewDirection() {

        string viewDirection = null;
        float moveX = GameObject.Find("Player").GetComponent<Player_Movement>().moveX;
        float moveY = GameObject.Find("Player").GetComponent<Player_Movement>().moveY;

        float absMoveX = Mathf.Abs(moveX);
        float absMoveY = Mathf.Abs(moveY);

        // Die Richtung in die der Spieler schaut, wird bestimmt

        if (moveX > 0 && absMoveX > absMoveY)
        {
            // Spieler schaut nach rechts
            viewDirection = "right";
        }

        else if (moveY > 0 && absMoveY > absMoveX)
        {
            // Spieler schaut nach oben
            viewDirection = "top";
        }

        else if (moveX < 0 && absMoveX > absMoveY)
        {
            // Spieler schaut nach links
            viewDirection = "left";
        }

        else if (moveY < 0 && absMoveY > absMoveX)
        {
            // Spieler schaut nach unten
            viewDirection = "bot";
        }

        else if (moveX == 0 && moveY == 0)
        {
            // Wenn der Sp. in keine Richtung schaut, dann schaut er nach unten; wichtig wenn der Spieler vorher noch nicht gelaufen ist}
            viewDirection = "bot";
        }

        return viewDirection;
    }
    



}
