using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
   public int scoreValue = 0;
   public Text score;

   private void OnEnable() {
       WeaponBase.onEnemyDamaged += changeScore;
   }
    private void OnDisable() {
       WeaponBase.onEnemyDamaged -= changeScore;
       //EnemyBehavior.onPlayerDamaged += resetScore;
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

   /*public void resetScore(enemeyattack) {

   }*/

}
