using UnityEngine;
using UnityEngine.UI;

public class SmokeCloudText : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerSkillManager skill;
    public Image image;

    void Start()
    {
        playerStats = transform.GetComponentInParent<PlayerStats>();
        skill = transform.GetComponentInParent<PlayerSkillManager>();
        playerStats.OnManaChanged.AddListener(UpdateVisibility);
    }

    private void UpdateVisibility(float currentMana)
    {
        if (currentMana >= 50 && skill.isSmokeReady)
        {
            var color = image.color;
            color.a = 1f;
            image.color = color;
        }
        else
        {
            var color = image.color;
            color.a = 0.5f;
            image.color = color;
        }
    }
}