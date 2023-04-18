using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    public Skill skill;
    public KeyCode key;
    private float cooldownTime;
    private float activeTime;

    private enum SkillState
    {
        ready,
        active,
        cooldown
    }
    private SkillState state = SkillState.ready;

    void Update()
    {
        switch (state)
        {
            case SkillState.ready:
                if (Input.GetKeyDown(key))
                {
                    skill.Activate(gameObject);
                    state = SkillState.active;
                    activeTime = skill.activeTime;
                }

                break;
            case SkillState.active:
                if (activeTime > 0)
                    activeTime -= Time.deltaTime;
                else
                {
                    state = SkillState.cooldown;
                    cooldownTime = skill.cooldownTime;
                }

                break;
            case SkillState.cooldown:
                if (cooldownTime > 0)
                    cooldownTime -= Time.deltaTime;
                else
                    state = SkillState.ready;
                break;
        }
    }
}