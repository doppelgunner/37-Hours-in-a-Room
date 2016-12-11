using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {


    /*
        mouse -> point
        register target
        player will follow
        
    */

    [SerializeField]
    private float maxHealth;

    private float _health;

    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float nearDistance = 0.2f;
    [SerializeField]
    private bool near = false;
    [SerializeField]
    private bool hasTarget = false;
    [SerializeField]
    private LayerMask interactable;
    [SerializeField]
    private float interactDistance = 0.5f;
    [SerializeField]
    private float attackCD = 0.5f;

    [SerializeField]
    private Animator attackAnimator;
    [SerializeField]
    private Animator playerAnimator;

    private Rigidbody2D _rb;
    private Vector2 _distance;
    private Vector2 _target;
    private Vector3 _direction;
    private Vector3 _mouseInteract;
    private Vector3 _mouseInteractDistance;

    private bool _attacked = false;
    private float _attackCounter = 0;

    [SerializeField]
    private Image fillerGUI; //every damage

    private SpriteRenderer _sr;
    private Color _originalColor;
    private Color _tintColor;

    // Use this for initialization
    void Start () {
        _rb = GetComponent<Rigidbody2D>();
        _distance = new Vector2();
        _target = Vector2.zero;
        _health = maxHealth;

        _sr = GetComponent<SpriteRenderer>();
        _originalColor = _sr.color;
        _tintColor = Color.red;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            _mouseInteract = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(_mouseInteract, Vector2.zero, 0, interactable);

            if (!WithinDistance(_mouseInteract, interactDistance))
            {
                SetTarget(_mouseInteract);
                return;
            }
            _mouseInteractDistance = ComputeDistance(_mouseInteract);

            if (hit.collider != null)
            {
                Collider2D other = hit.collider;
                ComputeAngle(_mouseInteractDistance);
                Debug.Log(other.tag);
                if (!_attacked && (other.tag == "zombie" || other.tag == "breakable"))
                {
                    attackAnimator.SetTrigger("Attack");
                    AudioController.PlayAttackSnd(0.1f);
                    _attacked = true;
                } else if (other.tag == "door") {
                    Breakable breakableScript = other.GetComponent<Breakable>();
                    breakableScript.AddHealth(GameController.GetWood());
                }

            }
        }

        if (Input.GetMouseButtonDown(1)) {
            
        }

        if (_attacked) {
            _attackCounter += Time.deltaTime;
            if (_attackCounter > attackCD) {
                _attacked = false;
                _attackCounter = 0;
            }
        }

        ComputeDistance();

        if (!near && hasTarget)
        {
            ComputeAngle(_distance);
            MoveForward();
        }
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
        if (dead) {
            playerAnimator.SetTrigger("Die");
            Die();
        }

        float healthPercentage = _health / maxHealth;
        fillerGUI.transform.localScale = new Vector3( healthPercentage, 1,1);
        _sr.color = _tintColor;
        StartCoroutine(ReturnColor());
        return dead;
    }

    IEnumerator ReturnColor() {
        yield return new WaitForSeconds(0.5f);
        _sr.color = _originalColor;
    }

    void SetTarget(Vector2 target) {
        _target = target;
        hasTarget = true;
        playerAnimator.SetBool("HasTarget", hasTarget);
    }

    Vector2 ComputeDistance(Vector2 target) {
        Vector2 distance = new Vector2(target.x - transform.position.x, target.y - transform.position.y);
        return distance;
    }

    void ComputeDistance()
    {
        if (!hasTarget) return;

        _distance.x = _target.x - transform.position.x;
        _distance.y = _target.y - transform.position.y;

        float hyp = Mathf.Sqrt(Mathf.Pow(_distance.x, 2) + Mathf.Pow(_distance.y, 2));
        if (hyp <= nearDistance)
        {
            near = true;
            hasTarget = false;
            playerAnimator.SetBool("HasTarget", hasTarget);
        } else
        {
            near = false;
        }
    }

    void ComputeAngle(Vector2 distance) {
        float z = Mathf.Atan2(
         (distance.y),
         (distance.x)
         ) * Mathf.Rad2Deg - 90;

        transform.eulerAngles = new Vector3(0, 0, z);
    }

    void MoveForward()
    {
        _rb.AddForce(transform.up * speed);
    }

    bool WithinDistance(Vector3 target, float distanceLimit) {
        float distance = Vector2.Distance(transform.position, target);
        if (distance < distanceLimit) return true;

        return false;
    }

    void OnDrawGizmos()
    {
        Vector3 playerPosition = transform.position;

        Gizmos.DrawLine(playerPosition, _target);
        Gizmos.DrawWireSphere(playerPosition, nearDistance);


        Gizmos.DrawWireSphere(playerPosition, interactDistance);
    }

    void Die() {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy() {
        yield return new WaitForSeconds(0.5f);
        if (!GameController.IsOver()) {
            GameController.SetOver(true);
            GameController.SetStatusText("Status: You Lose");
            GameController.ShowAllEndGUI();
        }
        Destroy(gameObject);
    }

}
