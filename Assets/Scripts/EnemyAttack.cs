using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    [SerializeField] Transform attackRightPos;
    [SerializeField] Transform attackLeftPos;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage = 1;

    // Update is called once per frame
    void Update() {
    }

    //called by characters and enemies to deal damage, animation handled by the specific script
    public void Attack(int facingDirection) {
        Collider2D[] enemiesToDamage = null;
        if(facingDirection == 1) { //facing right
            enemiesToDamage = Physics2D.OverlapCircleAll(attackRightPos.position, attackRange, enemyLayers); //makes array of anyone in the enemyLayers layer mask within the created circle (center point, radius, layer)
        } else if(facingDirection == -1) { //facing left
            enemiesToDamage = Physics2D.OverlapCircleAll(attackLeftPos.position, attackRange, enemyLayers);
        } else {
            Debug.Log("Melee failed");
        }
        foreach(Collider2D enemy in enemiesToDamage) {
            if(enemiesToDamage != null)
                enemy.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }

    //draws representation of the attack area when selected in the editor
    private void OnDrawGizmosSelected() {
        if(attackRightPos != null) {
            Gizmos.DrawWireSphere(attackRightPos.position, attackRange);
        }
        if(attackLeftPos != null) {
            Gizmos.DrawWireSphere(attackLeftPos.position, attackRange);
        }
    }
}
