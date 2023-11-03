using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

namespace Character.Player.Skills
{
    public class Teleportation : MonoBehaviour
    {
        public float minTeleportRange = 15f;
        private GameObject target;
        private GameObject[] teleports;
        private bool isTeleporting;
        private float timer;
        public float teleportTime = 1f; // Время, за которое будет происходить телепортация
        private PlayerStats playerStats;
        public int manaCost = 90;

        private void Start()
        {
            teleports = GameObject.FindGameObjectsWithTag("Teleport");
            playerStats = gameObject.GetComponent<PlayerStats>();
        }

        private void Update()
        {
            if (!isTeleporting && playerStats.CurrentMana >= manaCost && Input.GetKeyDown(KeyCode.T))
            {
                isTeleporting = true;
                target = GetTargetTeleport();
            }

            if (!isTeleporting) return;
            timer += Time.deltaTime;
            var t = timer / teleportTime;
            transform.position = Vector3.Lerp(transform.position, target.transform.position, t);

            if (Math.Abs(Vector3.Distance(transform.position, target.transform.position)) <= 1e-1)
            {
                Debug.Log("teleporated");
                isTeleporting = false;
                timer = 0f;
                playerStats.SpendMana(manaCost);
            }
        }

        private GameObject GetTargetTeleport()
        {
            return teleports
                .OrderBy(tp => Vector3.Distance(transform.position, tp.transform.position))
                .First(tp => Vector3.Distance(transform.position, tp.transform.position) > minTeleportRange);
        }
    }
}