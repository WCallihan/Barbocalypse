using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/* Used by all Enemy prefabs
 * Called only by EnemyController to attack based on the object's current facing direction and
 * uses a drawn circle to the side of the object based on the attack positions assigned and the
 * assigned attack range. Anything in the circle that is of the assigned enemy layer (the player)
 * has their TakeDamage function called in their controller.
 */

public class EnemyAttack : MonoBehaviour {

    [SerializeField] Transform attackLeftPos;
    [SerializeField] Transform attackRightPos;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage = 1;

    //called by characters and enemies to deal damage, animation handled by the controller script
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
                enemy.GetComponent<PlayerController>().TakeDamage(attackDamage, facingDirection);
        }
    }

    //draws representation of the attack area when the object is selected in the editor
    private void OnDrawGizmosSelected() {
        if(attackRightPos != null) {
            Gizmos.DrawWireSphere(attackRightPos.position, attackRange);
        }
        if(attackLeftPos != null) {
            Gizmos.DrawWireSphere(attackLeftPos.position, attackRange);
        }
    }
}