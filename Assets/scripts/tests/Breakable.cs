using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {

    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private Sprite[] changeSprites;
    [SerializeField]
    private int woodDrop = 10;

    private float _basePercentageForChanging;
    private float[] _percentagesCmp;
    private SpriteRenderer _sr;
    private Sprite originalSprite;

    private bool _broken = false;

    private float _health;
	// Use this for initialization
	void Start () {
        _sr = GetComponent<SpriteRenderer>();
        originalSprite = _sr.sprite;
        _health = maxHealth;
	}

    public void AddHealth(float health) {
        _health += health;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool Damage(float f) {
        bool destroyed = false;
        _health -= f;
        if (_health < 0) {
            _health = 0;
            destroyed = true;
        }

        CompareHealthAndChangeSprite();
        return destroyed;
    }

    void CompareHealthAndChangeSprite() {
        if (_broken) return;
        float healthPercentage = _health / maxHealth * 100;
        if (healthPercentage <= 0) {
            ChangeSprite(changeSprites[4]);
            _broken = true;
            StartCoroutine(Destroy());
        } else if (healthPercentage <= 20) {
            ChangeSprite(changeSprites[3]);
        } else if (healthPercentage <= 40) {
            ChangeSprite(changeSprites[2]);
        } else if (healthPercentage <= 60) {
            ChangeSprite(changeSprites[1]);
        } else if (healthPercentage <= 80) {
            ChangeSprite(changeSprites[0]);
        } else {
            ChangeSprite(originalSprite);
        }
    }

    void ChangeSprite(Sprite sprite) {
        _sr.sprite = sprite;
    }

    IEnumerator Destroy() {
        yield return new WaitForSeconds(0.5f);
        GameController.AddWood(woodDrop);
        Destroy(gameObject);
    }
}
