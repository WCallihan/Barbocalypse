using UnityEngine;
using System.Collections;

/* Used by all Enemy prefabs
 * This controller manages and controls the each enemy and thier behaviour. When
 * initiallized, it finds the Player and then continues to find its position and move to it.
 * It continues until it is within attacking range, and then attacks. It also handles
 * taking damage and dying based on its assigned maximum health.
 * This controller also manages all the animation conditions and sound effects for the enemies.
 */

public class EnemyController : MonoBehaviour {

    [SerializeField] float m_speed;
    [SerializeField] Vector2 moveVector;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackDistance;
    [SerializeField] int maxHealth = 2;
    [SerializeField] int pointsValue = 10;

    [SerializeField] AudioClip swordSwipe;
    [SerializeField] AudioClip hurtSound;

    private GameManager gameManager;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private GameObject player;
    private EnemyAttack enemyAttack;
    private AudioSource audioSource;

    private int currentHealth;
    private bool m_combatIdle = false;
    private bool attacking = false;
    private bool touchingPlayer = false;
    private int facingDirection = 1;

    public bool m_isDead = false;

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        enemyAttack = gameObject.GetComponent<EnemyAttack>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("EffectsVolume", 1);
        currentHealth = maxHealth;
    }

    void Update() {
        if(gameManager.gameRunning && !m_isDead) {
            // -- find direction to the player --
            moveVector = (player.transform.position - transform.position);

            // Swap direction of sprite depending on walk direction
            if(moveVector.x > 0) {
                GetComponent<SpriteRenderer>().flipX = true;
                facingDirection = 1;
            } else if(moveVector.x < 0) {
                GetComponent<SpriteRenderer>().flipX = false;
                facingDirection = -1;
            }

            // Move
            if(Mathf.Abs(moveVector.x) <= attackDistance) {
                touchingPlayer = true;
            } else {
                touchingPlayer = false;
            }
            if(!touchingPlayer && !attacking) {
                m_body2d.velocity = moveVector.normalized * new Vector2(m_speed, 0);
            } else {
                m_body2d.velocity = Vector2.zero;
            }

            // -- Handle Animations --
            //Attack when close to the player
            if(touchingPlayer && !attacking) {
                attacking = true;
                m_animator.SetTrigger("Attack"); //actual attack triggered by animation
                audioSource.PlayOneShot(swordSwipe);
                StartCoroutine("AttackCooldown");
            }

            //Run
            if(Mathf.Abs(moveVector.magnitude) > Mathf.Epsilon && !touchingPlayer) {
                m_animator.SetInteger("AnimState", 2);
            }

            //Combat Idle
            else if(m_combatIdle) {
                m_animator.SetInteger("AnimState", 1);
            }

            //Idle
            else {
                m_animator.SetInteger("AnimState", 0);
            }
        } else {
            m_animator.SetInteger("AnimState", 0); //idle when the player dies/game is not running
        }
    }

    //called by the attack animation at proper moment in attack animation
    public void AttackTrigger() {
        enemyAttack.Attack(facingDirection);
    }
    //called by Attack to have the attack recipient take damage
    public void TakeDamage(int damage) {
        if(!m_isDead) {
            currentHealth -= damage;
            m_animator.SetTrigger("Hurt");
            audioSource.PlayOneShot(hurtSound);
            if(currentHealth <= 0) {
                m_isDead = true;
                StartCoroutine("Die");
            }
        }
    }
    //called by TakeDamage when the enemy dies; plays animation and sets the boolean
    IEnumerator Die() {
        m_animator.SetBool("Dead", true); //play death animation
        gameManager.UpdateScore(pointsValue);
        gameObject.layer = 8; //sets layer as dead, so other enemies can pass through
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    //handles the attack cooldown and sets the bool to allow the next attack
    IEnumerator AttackCooldown() {
        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
    }
}
