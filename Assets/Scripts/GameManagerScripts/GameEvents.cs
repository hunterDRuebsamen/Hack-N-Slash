using System; // Added "using System" for System.Action to function
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private PlayerHealth phObject;
    private Transform playerTrans;
    private SpriteRenderer bloodOverlayRenderer;
    private Animation bloodAnim;
    private AudioSource musicSource;

    [SerializeField] GameObject lampPrefab;

    [SerializeField, Tooltip("List of Bushes")]
    List<GameObject> bushList;

    private GameObject[] lamps = new GameObject[2];

    private void Start() {
        musicSource = GameObject.Find("MusicManager").GetComponent<AudioSource>();  
        phObject = GameObject.Find("PlayerV4").GetComponent<PlayerHealth>();
        bloodOverlayRenderer = GameObject.Find("BloodOverlay").GetComponent<SpriteRenderer>();
        bloodAnim = GameObject.Find("BloodOverlay").GetComponent<Animation>();
        playerTrans = phObject.GetComponent<Transform>();
        // add 2 lamps (lights)
        lamps[0] = Instantiate(lampPrefab, new Vector3(-20, -1.4f, 0), Quaternion.identity);
        lamps[1] = Instantiate(lampPrefab, new Vector3(0, -1.4f, 0), Quaternion.identity);
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
            lamps[0].transform.position = new Vector2(playerTrans.position.x - (playerTrans.position.x % 20),-1.4f);
            lamps[1].transform.position = new Vector2(playerTrans.position.x - (playerTrans.position.x % 20) + 20,-1.4f);
        }
    }
}
