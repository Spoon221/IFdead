using System;
using System.Linq;
using UnityEngine;
using Photon.Pun;

namespace Character.Player.Skills
{
    public class Teleportation : MonoBehaviourPun
    {
        [SerializeField] private float minTeleportRange = 15f;
        private GameObject target;
        private GameObject[] teleports;
        private bool isTeleporting;
        private float timer;
        [SerializeField] private float teleportTime = 1f; // Время, за которое будет происходить телепортация
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private int manaCost = 90;
        [SerializeField] private CharacterController cc;
        [SerializeField] private AudioSource audio;
        [SerializeField] private AudioClip audioClip;

        private void Start()
        {
            teleports = GameObject.FindGameObjectsWithTag("Teleport");
        }

        private void Update()
        {
            if (photonView.IsMine 
                && !isTeleporting 
                && playerStats.CurrentMana >= manaCost 
                && Input.GetKeyDown(KeyCode.T))
            {
                isTeleporting = true;
                target = GetTargetTeleport();
                PlayAudio();
            }

            if (!isTeleporting) return;
            cc.enabled = false;
            timer += Time.deltaTime;
            var t = timer / teleportTime;
            transform.position = Vector3.Lerp(transform.position, target.transform.position, t);

            if (Math.Abs(Vector3.Distance(transform.position, target.transform.position)) <= 1e-1)
            {
                isTeleporting = false;
                timer = 0f;
                playerStats.SpendMana(manaCost);
                cc.enabled = true;
            }
        }

        private void PlayAudio()
        {
            audio.pitch = 1.6f;
            audio.volume = 1f;
            audio.PlayOneShot(audioClip);
            audio.volume = 0.8f;
            audio.pitch = 1f;
        }

        private GameObject GetTargetTeleport()
        {
            return teleports
                .OrderBy(tp => Vector3.Distance(transform.position, tp.transform.position))
                .First(tp => Vector3.Distance(transform.position, tp.transform.position) > minTeleportRange);
        }
    }
}