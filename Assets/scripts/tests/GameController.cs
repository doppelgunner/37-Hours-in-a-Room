using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    [SerializeField]
    private GameObject zombiePrefab;

    [SerializeField]
    private float spawnDelay = 10f;

    private static int _currentWood;
    private static GameObject _zombiePrefab;

    private static GameController _instance;

    private static float _spawnDelay;

    //spawn points for zombies
    [SerializeField]
    private GameObject[] spawnPoints;
    [SerializeField]
    private GameObject[] doors;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private int woodPerClick = 20;

    private static GameObject[] _spawnPoints;
    private static GameObject[] _doors;
    private static GameObject _player;
    private static int _woodPerClick;
    // Use this for initialization

    [SerializeField]
    private Text timeClockText;

    private static int _hours;
    private static int _minutes;
    private static Text _timeClockText;
    private static bool _over;
    private static float _timeCounter;

    private const float TIME_RATIO = 0.27f;

    [SerializeField]
    private Animator[] toShowAnimator;
    [SerializeField]
    private Text statusText;
    [SerializeField]
    private int hours = 37;


    [SerializeField]
    private Text woodText;

    private static Text _woodText;
    private static Animator[] _toShowAnimator;
    private static Text _statusText;

    void Start () {
        _zombiePrefab = zombiePrefab;
        _instance = this;

        _spawnDelay = spawnDelay;
        _spawnPoints = spawnPoints;
        _doors = doors;
        _player = player;

        _woodPerClick = woodPerClick;

        //gui
        _woodText = woodText;
        _woodText.text = "" + _currentWood;

        _instance.StartCoroutine(SpawnContinuously());

        _toShowAnimator = toShowAnimator;
        _statusText = statusText;

        //timer
        _hours = hours;
        _minutes = 0;
        _timeClockText = timeClockText;
        UpdateTimeClock();
        _over = false;
        _timeCounter = 0;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!_over) {
            ComputeTime();
        }
	}

    public static bool IsOver() {
        return _over;
    }

    static void ComputeTime() {
        if (_over) return;

        _timeCounter += Time.deltaTime;
        if (_timeCounter >= TIME_RATIO) {
            _timeCounter = 0;

            if (_hours <= 0 &&_minutes <= 0) {
                _over = true;
                _instance.StartCoroutine(WinGame());
                return;
            }
            if (_minutes <= 0) {
                _minutes = 59;
                if (_hours % 10 == 0) _spawnDelay -= 2;
                _hours--;
            } else {
                _minutes--;
            }
            UpdateTimeClock();
        }
    }

    static void UpdateTimeClock() {
        _timeClockText.text = "" + _hours + ":" + _minutes;
    }

    public static void AddWood(int wood) {
        _currentWood += wood;
        _woodText.text = "" + _currentWood;
        Debug.Log(wood);
    }

    public static int GetWood() {
        if (_currentWood <= 0) return 0;

        if (_currentWood >= _woodPerClick) {
            AddWood(-_woodPerClick);
            return _woodPerClick;
        } else {
            int remaining = _currentWood;
            AddWood(-_currentWood);
            return remaining;
        }
    }

    public static IEnumerator SpawnContinuously() {

        yield return new WaitForSeconds(_spawnDelay);
        SpawnEnemy();

        if (!_over) {
            _instance.StartCoroutine(SpawnContinuously());
        }
        
    }

    public static void SpawnEnemy() {
        int spawnRangeIndex = Random.Range(0, _spawnPoints.Length);

        GameObject zombie = Instantiate(_zombiePrefab, _spawnPoints[spawnRangeIndex].transform.position, Quaternion.identity) as GameObject;
        Zombie zombieScript = zombie.GetComponent<Zombie>();
        zombieScript.AddWaypoint(_doors[spawnRangeIndex]);
        zombieScript.AddWaypoint(_player);
    }

    public static List<GameObject> ToList(GameObject[] arrayObjects) {
        List<GameObject> listObjects = new List<GameObject>();
        for (int i = 0; i < arrayObjects.Length; i++) {
            listObjects.Add(arrayObjects[i]);
        }

        return listObjects;
    }

    static IEnumerator WinGame() {
        yield return new WaitForSeconds(3f);
        SetStatusText("Status: You Win");
        ShowAllEndGUI();
    }

    public static void SetStatusText(string text) {
        _statusText.text = text;
    }

    public static void ShowAllEndGUI() {
        for (int i = 0; i < _toShowAnimator.Length; i++) {
            _toShowAnimator[i].SetTrigger("Show");
        }
    }

    public static void SetOver(bool b) {
        _over = b;
    }

}
