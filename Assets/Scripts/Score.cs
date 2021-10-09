using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
   int scoreValue = 0;
   public Text score;

   void Awake() {
       score = GetComponent<Text>();
   }

   private void OnEnable() {
       WeaponBase.onWeaponTriggerHit += changeScore;
   }

   void changeScore(float damage) {
       scoreValue++;
       string result = "Score: " + scoreValue;
       score.text = result;
   }
}
