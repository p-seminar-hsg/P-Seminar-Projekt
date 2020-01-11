using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Florian Müller-Martin und Tobias Schwarz <br/>
/// Mitarbeiter: Rene Jokiel (Itemeffekte), Luca Kellermann (Farbänderung und Sound bei Schaden, Sound bei Angriff), 
///              Benedikt Wille (Items) <br/>
/// Zuletzt geändert am: 11.01.2020 <br/>
/// Mainklasse für den Player.
/// </summary>
public class Player_Main : MonoBehaviour
{
    #region Variablen
    [Header("Stats")]
    static float HP; //Aktuelle Lebenspunkte des Players // WarumIstDasNichtStatic(EineAnmerkungVonR.Jokiel)
    public float maxHP; //Maximale Lebenspunkte des Charakters
    public float strengthPattern; //Angriffswert des Players. Wird nicht verändert. Dient zur Wiederherstellung des ursprünglichen Angriffwertes (Rene Jokiel)
    public float strength; //Angriffswert des Players
    public float speedPattern;
    public bool isDead;


    [Header("Cooldown")]
    bool attackCooldownBool; //Entscheidend, ob ein Angriff ausgeführt werden kann, oder nich
    public float attackCooldownLength;


    [Header("Unity Stuff")]
    public Image healthBarPauseMenu;//Referenz zur Healthbar im PauseMenu (Geändert von Rene Jokiel)
    private Image healthBar; //Referenz zur HealthBar
    private Animator animator; // Animator des Players
    public GameOver gameOver;   // Die GameOver UI 
    public GameObject speedPanel;   // Anzeige für den Speed Effekt (Geändert von Rene Jokiel)
    public GameObject strenghtPanel; // Anzeige für den Strenght Effekt (Geändert von Rene Jokiel)

    [Header("Item related Stuff")]  // Eingeführt für Items mit temporärer Wirkung (Rene Jokiel, 25.07.2019)
    public float strengthCooldown;
    public bool strengthItemActive;
    public float speedCooldown;
    public bool speedItemActive;


    private bool changingColor; //ist der Player bereits rötlich gefärbt?
    #endregion

    #region Lifecycle-Methoden
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
        isDead = false;
        Player_Movement.speed = speedPattern;
        strenghtPanel.SetActive(false);
        speedPanel.SetActive(false);
        
    }

    // FixedUpdate wird einmal pro Frame aufgerufen
    private void FixedUpdate()
    {
        //Wenn der Player keine Leben mehr hat wird die Methode die() aufgerufen
        if (HP <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine("die");
        }

        //Die Health Bar wird aktualisiert
        healthBar.fillAmount = HP / maxHP;
        healthBarPauseMenu.fillAmount = HP / maxHP;// Von Rene Jokiel

        // Selbstgeißelung auf Leertaste zu Demonstrationszwecken
        if (Input.GetKey(KeyCode.Space))
        {
            HP -= 1;
        }
        else if (Input.GetKey(KeyCode.H))
            HP += 1;

        // Schlagen am PC
        if (Input.GetKey(KeyCode.A))
            attackMain();

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

    /// <summary>
    /// Sterbemethode des Players
    /// </summary>
    public IEnumerator die()
    {
        if (MapManager.instance.CurrentRoomIsBossRoom())
            PlayBossPlayerDeathSound();

        animator.SetFloat("attack", 0f);
        animator.SetInteger("die", 1);
        yield return new WaitForSeconds(0.666666f);
        animator.SetInteger("die", 0);
        gameOver.GoGameOver();  // Die GameOver UI wird getriggert. Spiel Vorbei (Von Rene Jokiel)
    }

    /// <summary>
    /// Einer von 5 möglichen PlayerDeath Sounds wird abgespielt.
    /// </summary>
    private void PlayBossPlayerDeathSound()
    {
        int randomNumber = Random.Range(1, 6);
        GameManager.PlaySound("BossPlayerDeath" + randomNumber);
    }

    #endregion

    #region Itemeffekte
    /// <summary>
    /// Generische Methode für alle Items - Teil der Item-API. (Benedikt Wille)
    /// </summary>
    /// <param name="effect">Der Itemseffekt (einer aus dem enum ItemEffect).</param>
    /// <param name="value">Der Wert des Effekts (im Falle von Speed und Protection die Länge des Effekts).</param>
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
                //Debug.Log("Health set to: " + HP);
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
    /// Stärkemethode des Players - Erhöht den Schadenswert des Players für eine bestimmte Zeit. (Rene Jokiel)
    /// </summary>
    /// <param name="addition">Der Schadenswert, der zugeführt werden soll.</param>
    /// <param name="time">Der Zeitraum der Schadenserhöhung.</param>
    public void StrengthUp(float time, float addition)
    {
        strength += addition;
        strengthCooldown = time;
        strengthItemActive = true;
        strenghtPanel.SetActive(true);

    }

    public void NormalizeStrength()
    {
        strength = strengthPattern;
        strengthItemActive = false;
        strenghtPanel.SetActive(false);
    }

    /// <summary>
    /// Diese Methode bewirkt einen Speedschub beim Player. (Rene Jokiel)
    /// </summary>
    public void SpeedUp(float time, float additon)
    {
        Player_Movement.speed += additon;
        speedCooldown = time;
        speedItemActive = true;
        speedPanel.SetActive(true);
    }

    public void NormalizeSpeed()
    {
        Player_Movement.speed = speedPattern;
        speedItemActive = false;
        speedPanel.SetActive(false);
    }

    public void enhanceMaxHP(float Increasement, Color colorForHealthBar)
    {
        maxHP += Increasement;
        healthBar.color = colorForHealthBar;
        return;
    }
    #endregion

    #region Combatsystem
    /// <summary>
    /// Die passende Attack-Hitbox wird aktiviert.
    /// </summary>
    public void attackMain()
    {
        if (attackCooldownBool)
        {
            StartCoroutine("attackCooldownCoroutine");
            
            Direction viewDirection = getViewDirection();

            PlaySwordSwingSound();

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

    /// <summary>
    /// Einer von 4 möglichen Schwert Sounds wird abgespielt.
    /// </summary>
    private void PlaySwordSwingSound()
    {
        int randomNumber = Random.Range(1, 5);
        GameManager.PlaySound("SwordSwing" + randomNumber);
    }

    /// <summary>
    /// Schadensmethode des Players - Reduziert die HP des Players um den übergebenen Wert und stößt den Player in die übergebene Richtung weg.
    /// </summary>
    /// <param name="enemy">Der Gegner, der für den Schaden verantwortlich ist.</param>
    public void takeHit(GameObject enemy)
    {
        PlayHitSound();

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
    /// Einer von 5 möglichen Schaden Sounds wird abgespielt.
    /// </summary>
    private void PlayHitSound()
    {
        int randomNumber = Random.Range(1, 6);
        GameManager.PlaySound("Hit" + randomNumber);
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

    #endregion

    /// <summary>
    /// Diese Methode gibt die aktuelle Blickrichtung des Players als Direction zurück.
    /// </summary>
    /// <returns>Die aktuelle Blickrichtung.</returns>
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
    /// Ändert die Farbe des Player Sprites für eine bestimmte Zeit, sodass er rötlich eingefärbt wird.
    /// </summary>
    /// <param name="time">Zeit, für die der Player gefärbt wird.</param>
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
}
