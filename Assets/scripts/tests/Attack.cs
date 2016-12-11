using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

    [SerializeField]
    private Vector2 damageRange;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "breakable") {
            Breakable breakableScript = other.GetComponent<Breakable>();
            breakableScript.Damage(Random.Range(damageRange.x, damageRange.y));
        } else if (other.tag == "zombie") {
            Zombie zombieScript = other.GetComponent<Zombie>();
            zombieScript.Damage(Random.Range(damageRange.x, damageRange.y));
            
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
