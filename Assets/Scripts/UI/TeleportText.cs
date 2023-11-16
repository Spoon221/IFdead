using UnityEngine;

namespace UI
{
    public class TeleportText : MonoBehaviour
    {
        public int manaCost = 90;
        private PlayerStats playerStats;
        private PlayerSkillManager skill;
    
        void Start()
        {
            playerStats = transform.GetComponentInParent<PlayerStats>();
            skill = transform.GetComponentInParent<PlayerSkillManager>();
            playerStats.OnManaChanged.AddListener(UpdateVisibility);
        }

        private void UpdateVisibility(float currentMana)
        {
            gameObject.SetActive(currentMana >= manaCost);
        }
    }
}