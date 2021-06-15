using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    [SerializeField] GameObject swordPowerupIndicator;

    private GameManager gameManager;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private PlayerAttack playerAttack;
    private HealthManager healthManager;

    private PowerupType currentPowerup = PowerupType.None;

    private bool m_grounded = false;
    private bool m_rolling = false;
    private bool blocking = false;
    public bool isDead = false;

    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private int attackDamage = 1;

    // Use this for initialization
    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        playerAttack = gameObject.GetComponent<PlayerAttack>();
        healthManager = gameObject.GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update() {
        if(gameManager.gameRunning && !isDead) {
            // Increase timer that controls attack combo
            m_timeSinceAttack += Time.deltaTime;

            //Check if character just landed on the ground
            if(!m_grounded && m_groundSensor.State()) {
                m_grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
            }

            //Check if character just started falling
            if(m_grounded && !m_groundSensor.State()) {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
            }

            // -- Handle input and movement --
            float inputX = Input.GetAxis("Horizontal");

            // Swap direction of sprite depending on walk direction
            if(inputX > 0) {
                GetComponent<SpriteRenderer>().flipX = false;
                m_facingDirection = 1;
                blocking = false;
            } else if(inputX < 0) {
                GetComponent<SpriteRenderer>().flipX = true;
                m_facingDirection = -1;
                blocking = false;
            }

            // Move
            if(!m_rolling)
                m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

            // -- Handle Animations --
            //Wall Slide
            m_animator.SetBool("WallSlide", (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State()));

            //Attack
            if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling) {
                m_currentAttack++;
                blocking = false;

                // Loop back to one after third attack
                if(m_currentAttack > 3)
                    m_currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if(m_timeSinceAttack > 1.0f)
                    m_currentAttack = 1;

                // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                m_animator.SetTrigger("Attack" + m_currentAttack); //actual attack triggered by animation

                // Reset timer
                m_timeSinceAttack = 0.0f;
            }

            //Blocking Setter
            else if(Input.GetMouseButtonDown(1) && !m_rolling) {
                m_animator.SetTrigger("BeginBlock");
                m_animator.SetBool("IdleBlock", true);
                blocking = true;
            } else if(Input.GetMouseButtonUp(1)) {
                blocking = false;
            }


            //Roll
            else if(Input.GetKeyDown("left shift") && !m_rolling) {
                m_rolling = true;
                blocking = false;
                m_animator.SetTrigger("Roll");
                m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            }


            //Jump
            else if(Input.GetKeyDown("space") && m_grounded && !m_rolling) {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                blocking = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }

            //Run
            else if(Mathf.Abs(inputX) > Mathf.Epsilon) {
                // Reset timer
                m_delayToIdle = 0.05f;
                m_animator.SetInteger("AnimState", 1);
            }

            //Idle
            else {
                // Prevents flickering transitions to idle
                m_delayToIdle -= Time.deltaTime;
                if(m_delayToIdle < 0)
                    m_animator.SetInteger("AnimState", 0);
            }

            //Block stop
            if(!blocking) {
                m_animator.SetBool("IdleBlock", false);
            }
        }
    }

    //Powerup Collisions
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Powerup") && currentPowerup == PowerupType.None) {
            currentPowerup = collision.gameObject.GetComponent<Powerup>().powerupType;
            if(currentPowerup == PowerupType.Sword) {
                StartCoroutine(SwordPowerup());
            }
            Destroy(collision.gameObject);
        }
    }

    //called by the animation at proper moment in attack animation
    public void AttackTrigger() {
        playerAttack.Attack(attackDamage, m_facingDirection);
    }
    //called by attack to have the attack recipient take damage
    public void TakeDamage(int damage, int enemyFacingDirection) {
        if(!isDead && !m_rolling) {
            if(blocking && enemyFacingDirection == -m_facingDirection) { //if the player is blocking and facing the opposite direction as the enemy
                m_animator.SetTrigger("Block");
            } else { //get hit
                healthManager.UpdateLives(1);
                m_animator.SetTrigger("Hurt"); //play hurt animation
                if(healthManager.currentLives <= 0) {
                    isDead = true;
                    StartCoroutine("Die");
                }
            }
        }
    }
    //called by take damage when the enemy dies, plays animation and sets the boolean
    IEnumerator Die() {
        m_animator.SetBool("noBlood", m_noBlood);
        m_animator.SetBool("Dead", true); //play death animation
        yield return new WaitForSeconds(5);
    }

    // Animation Events
    // Called in end of roll animation.
    void AE_ResetRoll() {
        m_rolling = false;
    }
    // Called in slide animation.
    void AE_SlideDust() {
        Vector3 spawnPosition;

        if(m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if(m_slideDust != null) {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }


    //Sword Powerup
    IEnumerator SwordPowerup() {
        attackDamage = 2;
        swordPowerupIndicator.SetActive(true);
        yield return new WaitForSeconds(10);
        attackDamage = 1;
        swordPowerupIndicator.SetActive(false);
    }
}
