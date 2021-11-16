using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
   int scoreValue = 0;
   public Text score;

   private void OnEnable() {
       WeaponBase.onEnemyDamaged += changeScore;
   }
    private void onDisable() {
       WeaponBase.onEnemyDamaged -= changeScore;
   }

   void changeScore(float damage, GameObject enemyObject) {
       scoreValue++;
       string result = "Score: " + scoreValue;
       score.text = result;
   }

   public void updateScore(int val) {
       scoreValue += val;
       string result = "Score: " + scoreValue;
       score.text = result;
   }
}
