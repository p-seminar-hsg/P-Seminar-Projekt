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
    public float strengthPattern; //Angriffswert des Players. Wird nicht verändert. Dient zur Wiederherstellung des ursprünglichen Angriffwertes (Rene Jokiel)
    public float strength; //Angriffswert des Players
    public float speedPattern;


    [Header("Cooldown")]
    bool attackCooldownBool; //Entscheidend, ob ein Angriff ausgeführt werden kann, oder nich
    public float attackCooldownLength;


    [Header("Unity Stuff")]
    public Image healthBarPauseMenu;//Referenz zur Healthbar im PauseMenu (Geändert von Rene Jokiel)
    private Image healthBar; //Referenz zur HealthBar
    private Animator animator; // Animator des Players
    public GameOver gameOver;   // Die GameOver UI 

    [Header("Item related Stuff")]  // Eingeführt für Items mit temporärer Wirkung (Rene Jokiel, 25.07.2019)
    public float strengthCooldown;
    public bool strengthItemActive;
    public float speedCooldown;
    public bool speedItemActive;


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
        Player_Movement.speed = speedPattern;
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
        if (HP > 0 && enemy.CompareTag("Projectile"))
        {
            HP -= enemy.GetComponent<Projectile>().strength;
            StartCoroutine(ColorChangeForSeconds(0.35f));
        }
        if (HP > 0 && enemy.CompareTag("SpecialHitbox"))    //Von R. Jokiel
        {
            HP = HP / 2;
            StartCoroutine(ColorChangeForSeconds(0.35f));
        }

        //kein Knockback bei GameOver
        if (HP > 0 && enemy.CompareTag("Enemy"))
        {
            StartCoroutine(GetComponent<Player_Movement>().KnockbackCo((transform.position - enemy.transform.position), enemy.GetComponent<Enemy>().attackKnockback));
        }
    }

    /// <summary>
    /// Erstellt von Benedikt
    /// Generische Methode für alle Items - Teil der Item-API
    /// </summary>
    /// <param name="effect">Der Itemseffekt (Einer aus dem enum ItemEffect)</param>
    /// <param name="value">Der Wert des Effekts (im Falle von Speed und Protection die Länge des Effekts)</param>
    public void UseItem(ItemEffect effect, float value)
    {
        switch (effect)
        {
            case ItemEffect.HEAL:
                HP += value;
                if (HP > maxHP)
                {
                    HP = maxHP;
                }
                Debug.Log("Health set to: " + HP);
                break;
            case ItemEffect.STRENGTH:
                StrengthUp(value, 5); // Wert hardgecodetet, da value für die Zeit genutzt wird und wir ganz sicher nicht mehrere Werte brauchen ^^
                break;
            case ItemEffect.SPEED:
                SpeedUp(value, 200);
                break;
        }

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

            Direction viewDirection = getViewDirection();

            if (viewDirection == Direction.RIGHT)
            {
                // Spieler schaut nach rechts
                StartCoroutine("attackRight");
                // Debug.Log("Debug von Flo: Angriff nach Rechts");
            }

            else if (viewDirection == Direction.UP)
            {
                // Spieler schaut nach oben
                StartCoroutine("attackTop");
                // Debug.Log("Debug von Flo: Angriff nach Oben");
            }

            else if (viewDirection == Direction.LEFT)
            {
                // Spieler schaut nach links
                StartCoroutine("attackLeft");
                // Debug.Log("Debug von Flo: Angriff nach Links");

            }

            else if (viewDirection == Direction.DOWN)
            {
                // Spieler schaut nach unten
                StartCoroutine("attackBottom");
                // Debug.Log("Debug von Flo: Angriff nach Unten");
            }

        }


    }


    //ändert die Farbe des Player Sprites für eine bestimmte Zeit, sodass er rötlich eingefärbt wird
    //von Luca Kellermann am 18.07.2019
    private IEnumerator ColorChangeForSeconds(float time)
    {
        if (!changingColor)
        {
            changingColor = true;
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

        yield return new WaitForSeconds(attackCooldownLength);

        attackCooldownBool = true;
    }

    private IEnumerator attackBottom()
    {
        animator.SetFloat("attack", 1);
        GameObject hitboxGO = transform.Find("hitboxBottom").gameObject;
        hitboxGO.SetActive(true);
        yield return new WaitForSeconds(0.32f);
        animator.SetFloat("attack", 0);
        hitboxGO.SetActive(false);

    }

    private IEnumerator attackRight()
    {
        animator.SetFloat("attack", 2);
        GameObject hitboxGO = transform.Find("hitboxRight").gameObject;
        hitboxGO.SetActive(true);
        yield return new WaitForSeconds(0.32f);
        animator.SetFloat("attack", 0);
        hitboxGO.SetActive(false);
    }

    private IEnumerator attackTop()
    {
        GameObject hitboxGO = transform.Find("hitboxTop").gameObject;
        hitboxGO.SetActive(true);
        animator.SetFloat("attack", 3);
        yield return new WaitForSeconds(0.32f);
        animator.SetFloat("attack", 0);
        hitboxGO.SetActive(false);

    }

    private IEnumerator attackLeft()
    {
        GameObject hitboxGO = transform.Find("hitboxLeft").gameObject;
        hitboxGO.SetActive(true);
        animator.SetFloat("attack", 4);
        yield return new WaitForSeconds(0.32f);
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

        //Zeit des StärkePowerUps wird runtergezählt    (Von Rene Jokiel)
        if (strengthCooldown > 0 && strengthItemActive == true)
        {
            strengthCooldown -= Time.deltaTime;
        }
        if (strengthCooldown <= 0 && strengthItemActive == true) // Wenn die zeit abgelaufen ist, aber das PowerUp noch nicht deaktiviert wurde, wird es hier deaktiviert (Von Rene Jokiel)
        {
            NormalizeStrength();
        }

        //Zeit des SpeedPowerUps wird runtergezählt    (Von Rene Jokiel)
        if (speedCooldown > 0 && speedItemActive == true)
        {
            speedCooldown -= Time.deltaTime;
        }
        if (speedCooldown <= 0 && speedItemActive == true) // Wenn die zeit abgelaufen ist, aber das PowerUp noch nicht deaktiviert wurde, wird es hier deaktiviert (Von Rene Jokiel)
        {
            NormalizeSpeed();
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
    public static Direction getViewDirection()
    {

        // Wenn der Sp. in keine Richtung schaut, dann schaut er nach unten; wichtig wenn der Spieler vorher noch nicht gelaufen ist}
        Direction viewDirection = Direction.DOWN;
        float moveX = GameObject.Find("Player").GetComponent<Player_Movement>().moveX;
        float moveY = GameObject.Find("Player").GetComponent<Player_Movement>().moveY;

        float absMoveX = Mathf.Abs(moveX);
        float absMoveY = Mathf.Abs(moveY);

        // Die Richtung in die der Spieler schaut, wird bestimmt

        if (moveX > 0 && absMoveX > absMoveY)
        {
            // Spieler schaut nach rechts
            viewDirection = Direction.RIGHT;
        }

        else if (moveY > 0 && absMoveY > absMoveX)
        {
            // Spieler schaut nach oben
            viewDirection = Direction.UP;
        }

        else if (moveX < 0 && absMoveX > absMoveY)
        {
            // Spieler schaut nach links
            viewDirection = Direction.LEFT;
        }

        else if (moveY < 0 && absMoveY > absMoveX)
        {
            // Spieler schaut nach unten
            viewDirection = Direction.DOWN;
        }

        return viewDirection;
    }

    /// <summary>
    /// Diese Methode bewirkt einen Stärkeschub beim Player (Rene Jokiel)
    /// </summary>

    public void StrengthUp(float time, float additon)
    {
        strength += additon;
        strengthCooldown = time;
        strengthItemActive = true;

    }
    public void NormalizeStrength()
    {
        strength = strengthPattern;
        strengthItemActive = false;
    }

    /// <summary>
    /// Diese Methode bewirkt einen Speedschub beim Player (Rene Jokiel)
    /// </summary>

    public void SpeedUp(float time, float additon)
    {
        Player_Movement.speed += additon;
        speedCooldown = time;
        speedItemActive = true;
    }

    public void NormalizeSpeed()
    {
        Player_Movement.speed = speedPattern;
        speedItemActive = false;
    }
}
