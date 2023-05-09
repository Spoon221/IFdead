using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] private List<Skill> availableSkills;
    private List<bool> skillsReady;
    private int currentSkillIndex;
    private PlayerStats playerStats;

    void Start()
    {
        currentSkillIndex = 0;
        skillsReady = new List<bool>(availableSkills.Count);
        foreach (var skill in availableSkills)
            skillsReady.Add(true);
        playerStats = gameObject.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && skillsReady[currentSkillIndex])
        {
            StartCoroutine(StartTimer(currentSkillIndex));
            availableSkills[currentSkillIndex].Activate();
            playerStats.SpendMana(availableSkills[currentSkillIndex].ManaCost);
        }
    }

    private IEnumerator StartTimer(int skillIndex)
    {
        skillsReady[skillIndex] = false;
        yield return new WaitForSeconds(availableSkills[skillIndex].CooldownTime);
        skillsReady[skillIndex] = true;
    }
}