using System; // Added "using System" for System.Action to function
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private PlayerHealth phObject;
    private Transform playerTrans;
    private SpriteRenderer bloodOverlayRenderer;
    private Animation bloodAnim;
    private AudioSource musicSource;
    [SerializeField] GameObject container;
    [SerializeField] GameObject lampPrefab;

    [SerializeField, Tooltip("List of Bushes")]
    List<GameObject> bushSpawnList;

    private List<GameObject> bushList = new List<GameObject>();

    private GameObject[] lamps = new GameObject[2];

    private bool cleanup = true;

    const int bushRange = 25;
    const float lampY = -0.4f;

    private void Start() {
        musicSource = GameObject.Find("MusicManager").GetComponent<AudioSource>();  
        phObject = GameObject.Find("PlayerV4").GetComponent<PlayerHealth>();
        bloodOverlayRenderer = GameObject.Find("BloodOverlay").GetComponent<SpriteRenderer>();
        bloodAnim = GameObject.Find("BloodOverlay").GetComponent<Animation>();
        playerTrans = phObject.GetComponent<Transform>();
        // add 2 lamps (lights)
        lamps[0] = Instantiate(lampPrefab, new Vector3(-20, lampY, 0), Quaternion.identity);
        lamps[0].transform.parent = container.transform;
        lamps[1] = Instantiate(lampPrefab, new Vector3(0, lampY, 0), Quaternion.identity);
        lamps[1].transform.parent = container.transform;
        cleanup = true;
        SpawnFoilage();
        ClearFoilage(20f);
    }

    private void OnDestroy() {
        cleanup = false;
    }

    private void OnEnable() {
        EnemyBehavior.onPlayerDamaged += onPlayerHit;
        WeaponBase.onEnemyDamaged += onEnemyHit;
        PlayerHealth.onPlayerHealthChanged += onPlayerHealth;
    } 
    private void OnDisable() {
        EnemyBehavior.onPlayerDamaged -= onPlayerHit;
        WeaponBase.onEnemyDamaged -= onEnemyHit;
        PlayerHealth.onPlayerHealthChanged -= onPlayerHealth;
    } 
    private void onEnemyHit(float damage, GameObject enemyObject) {
        Debug.Log("Enemy has been hit for: "+ damage);
    }
    private void onPlayerHit(EnemyBehavior.AttackType attackType, float damage) {
        Debug.Log("Player has been hit." + damage);
    }

    // called when the player's health changes
    private void onPlayerHealth(int cur_health) {
        if (cur_health > 0 && cur_health <= phObject.criticalHealthLevel) {
            bloodOverlayRenderer.enabled = true;
            bloodAnim.Play();
        } else {
            bloodOverlayRenderer.enabled = false;
            bloodAnim.Stop();
        }
    }

    private void FixedUpdate() {
        if ((playerTrans.position.x > lamps[1].transform.position.x) || 
            (playerTrans.position.x < lamps[0].transform.position.x)) {
            // lamp is beyond player range, move it to correct position
            lamps[0].transform.position = new Vector2(playerTrans.position.x - (playerTrans.position.x % 20), lampY);
            lamps[1].transform.position = new Vector2(playerTrans.position.x - (playerTrans.position.x % 20) + 20, lampY);
        }
    }

    private async void SpawnFoilage() {
        while(cleanup) {
            await Task.Delay(2500);
            int index = UnityEngine.Random.Range(0,bushSpawnList.Count);
            if (bushList.Count > 0) {
                if (bushList.Count <= 5) {
                    float _xSpawnPos;
                    if (bushList[bushList.Count-1].transform.position.x > playerTrans.position.x) {
                        _xSpawnPos = bushList[bushList.Count-1].transform.position.x + UnityEngine.Random.Range(bushRange-10,bushRange+10);
                    } else {
                        _xSpawnPos = playerTrans.position.x + UnityEngine.Random.Range(25,45);
                    }
                    
                    float _ySpawnPos = UnityEngine.Random.Range(-3.5f,-1.3f);
                    GameObject bush = Instantiate(bushSpawnList[index], new Vector3(_xSpawnPos, _ySpawnPos, 0), Quaternion.identity);
                    bush.transform.parent = container.transform;
                    bushList.Add(bush);
                }
            } else {
                // no bushes. Spawn one
                float _xSpawnPos = playerTrans.position.x + bushRange + UnityEngine.Random.Range(-5,10);
                float _ySpawnPos = UnityEngine.Random.Range(-3.5f,-1.3f);
                GameObject bush = Instantiate(bushSpawnList[index], new Vector3(_xSpawnPos, _ySpawnPos, 0), Quaternion.identity);
                bush.transform.parent = container.transform;
                bushList.Add(bush);
                
            }
        }
    }
    
    // this function will periodically destroy the foilage that are TOO far away from the player
    private async void ClearFoilage(float maxDist)
    {
        while(cleanup)
        {
            await Task.Delay(2500);
            if (bushList.Count > 0) {
                foreach (GameObject bush in bushList) {
                    if ((playerTrans.position.x - bush.transform.position.x) > maxDist) {
                        Debug.Log("remove bush");
                        bushList.Remove(bush);
                        Destroy(bush);
                        break;
                    }
                }
            }
        }
    }
}
