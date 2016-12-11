using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zombie: MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector2 attackDamageRange;
    [SerializeField]
    private float attackDelay = 1f;

    private bool _hasAttacked = false;
    private float _attackCounter = 0;

    [SerializeField]
    private bool near = false;
    [SerializeField]
    private float nearDistance = 0.2f;

    private Vector2 _distance;
    private Rigidbody2D _rb;
    private Queue<GameObject> _waypoints;
    private GameObject _currentWaypoint;

    [SerializeField]
    private Animator zombieAnimator;

    [SerializeField]
    private float maxHealth;

    private float _health;

    private SpriteRenderer _sr;
    private Color _originalColor;
    private Color _tintColor;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _distance = new Vector2();
        _waypoints = new Queue<GameObject>();
        _currentWaypoint = null;
    }
    // Use this for initialization
    void Start () {

        //init
        SetCurrentWaypoint();
        _health = maxHealth;

        _sr = GetComponent<SpriteRenderer>();
        _originalColor = _sr.color;
        _tintColor = Color.green;
    }
	
	// Update is called once per frame
	void Update () {
        if (near) {
            if (_currentWaypoint != null && !_hasAttacked)
            {
                if (_currentWaypoint.tag == "door")
                {
                    Breakable doorScript = _currentWaypoint.GetComponent<Breakable>();
                    doorScript.Damage(Random.Range(attackDamageRange.x, attackDamageRange.y));
                    _hasAttacked = true;

                    Debug.Log("ATTACK");

                } else if (_currentWaypoint.tag == "Player") {
                    Player playerScript = _currentWaypoint.GetComponent<Player>();
                    playerScript.Damage(Random.Range(attackDamageRange.x, attackDamageRange.y));
                    _hasAttacked = true;
                    //DO damage

                }
            }
        }

        if (_hasAttacked) {
            _attackCounter += Time.deltaTime;
            if (_attackCounter > attackDelay) {
                _hasAttacked = false;
                _attackCounter = 0;
            }
        }
    }


    void FixedUpdate()
    {

        ComputeDistance();
        if (!near)
        {
            ComputeAngle();
            MoveForward();
        }
    }

    public void AddWaypoint(GameObject waypoint) {
        _waypoints.Enqueue(waypoint);
    }

    public void CheckWayPoints() {
        for (int i = 0; i < _waypoints.Count; i++) {
            if (_waypoints.Peek() == null) {
                _waypoints.Dequeue();

            }
        }

        SetCurrentWaypoint();
    }
    
    void SetCurrentWaypoint() {
        if (_waypoints.Count > 0) {
            _currentWaypoint = _waypoints.Dequeue();
        } else {
            _currentWaypoint = null;
        }
    }

    void MoveForward() {
        if (_currentWaypoint == null) {
            CheckWayPoints();
            return;
        }

        _rb.AddForce(transform.up * speed);
    }

    void ComputeDistance() {
        if (_currentWaypoint == null) {
            CheckWayPoints();
            return;
        }

        _distance.x = _currentWaypoint.transform.position.x - transform.position.x;
        _distance.y = _currentWaypoint.transform.position.y - transform.position.y;

        float hyp = Mathf.Sqrt(Mathf.Pow(_distance.x, 2) + Mathf.Pow(_distance.y,2));
        if (hyp <= nearDistance) {
            near = true;
            zombieAnimator.SetBool("HasTarget", near);
        } else {
            near = false;
            zombieAnimator.SetBool("HasTarget", near);
        }
    }

    void ComputeAngle()
    {
        float z = Mathf.Atan2(
         (_distance.y),
         (_distance.x)
         ) * Mathf.Rad2Deg - 90;

        transform.eulerAngles = new Vector3(0, 0, z);
    }

    void OnDrawGizmos() {
        Vector3 zombiePosition = transform.position;
        
        Gizmos.DrawWireSphere(zombiePosition, nearDistance);
    }

    void Die()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public bool Damage(float f)
    {
        bool dead = false;
        _health -= f;
        if (_health < 0)
        {
            _health = 0;
            dead = true;
        }
        if (dead)
        {
            zombieAnimator.SetTrigger("Die");
        }
        _sr.color = _tintColor;
        StartCoroutine(ReturnColor());
        return dead;
    }

    IEnumerator ReturnColor()
    {
        yield return new WaitForSeconds(0.5f);
        _sr.color = _originalColor;
    }
}
