using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used by the Player prefab
 * Called by PlayerController to attack in front of the object
 * based on its current facing direction and uses a drawn circle
 * to the side of the obejct based on the attack positions and the
 * attack range. Anything in the circle of the enemy layer (enemies)
 * has their TakeDamage function called in their controller
 */

public class PlayerAttack : MonoBehaviour {

    [SerializeField] Transform attackRightPos;
    [SerializeField] Transform attackLeftPos;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage;

    //called by characters and enemies to deal damage, animation handled by the specific script
    public void Attack(int damage, int facingDirection) {
        attackDamage = damage;
        Collider2D[] enemiesToDamage = null;
        if(facingDirection == 1) { //facing right
            enemiesToDamage = Physics2D.OverlapCircleAll(attackRightPos.position, attackRange, enemyLayers); //makes array of anyone in the enemyLayers layer mask within the created circle (center point, radius, layer)
        } else if(facingDirection == -1) { //facing left
            enemiesToDamage = Physics2D.OverlapCircleAll(attackLeftPos.position, attackRange, enemyLayers);
        } else {
            Debug.Log("Melee failed");
        }
        foreach(Collider2D enemy in enemiesToDamage) {
            enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);
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