using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Used by the Indicator prefabs
 * The indicators constantly follow the player at their assigned offset.
 */

public class Indicator : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] Vector3 offset;

    void Update() {
        transform.position = player.transform.position + offset;
    }
}
