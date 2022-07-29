using System;
using System.Collections.Generic;
using UnityEngine;

namespace RocketJumper
{
    class RocketBehaviour : MonoBehaviour
    {
        float speed = 75f;
        float pushForce = 35f;
        float radius = 15f;

        public GameObject ImpactPart;
        public GameObject ImpactSphere;
        public AudioClip ImpactR;

        public Vector3 Dir;
        public Transform owner;
        Rigidbody rb;

        public void Detonate()
        {
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            Vector3 dir = Player.transform.position - transform.position;
            if (Mathf.Abs(dir.magnitude) <= radius)
            {
                //MonoSingleton<NewMovement>.Instance.Launch(Player.transform.position, force, force * 10);
                if (Player.GetComponent<NewMovement>().gc.onGround)
                {
                    Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 0.25f, Player.transform.position.z);
                    dir = Player.transform.position - transform.position;
                }
                float dist = dir.magnitude;
                float force = pushForce / (dist + 0.1f);
                Player.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.VelocityChange);
                GameObject.FindGameObjectWithTag("GunControl").GetComponent<EventHandler>().blastsource = "RocketJumper";
            }
            GameObject IP = Instantiate<GameObject>(ImpactPart, transform.position, Quaternion.identity);
            IP.AddComponent<RemoveOnTime>();
            IP.GetComponent<RemoveOnTime>().time = 1f;
            IP.GetComponent<RemoveOnTime>().useAudioLength = false;
            IP.GetComponent<RemoveOnTime>().randomizer = 0f;
            IP.transform.parent = null;
            ImpactSphere = Plugin.RocketBundle.LoadAsset<GameObject>("ImpSphere");
            GameObject ImpSphere = Instantiate<GameObject>(ImpactSphere, transform.position, Quaternion.identity);
            ImpSphere.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
            ImpSphere.AddComponent<FadeAway>();
            ImpSphere.transform.parent = null;
            EventHandler.PlayClip(ImpactR, transform.position, 1f, 10f);
            Destroy(transform.gameObject);
        }

        void Awake()
        {
            rb = transform.GetComponent<Rigidbody>();
            rb.mass = 0f;
        }

        void Update()
        {
            rb.velocity = Dir * speed;
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag != "Player" && col.gameObject.tag != "Enemy")
            {
                GameObject Player = GameObject.FindGameObjectWithTag("Player");
                Vector3 dir = Player.transform.position - transform.position;
                if (Mathf.Abs(dir.magnitude) <= radius)
                {
                    if (Player.GetComponent<NewMovement>().gc.onGround)
                    {
                        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + 2f, Player.transform.position.z);
                        dir = Player.transform.position - transform.position;
                    }
                    float dist = dir.magnitude;
                    float force = pushForce / (dist + 0.1f);
                    Player.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.VelocityChange);
                    GameObject.FindGameObjectWithTag("GunControl").GetComponent<EventHandler>().blastsource = "RocketJumper";
                }
                GameObject IP = Instantiate<GameObject>(ImpactPart, transform.position, Quaternion.identity);
                IP.AddComponent<RemoveOnTime>();
                IP.GetComponent<RemoveOnTime>().time = 1f;
                IP.GetComponent<RemoveOnTime>().useAudioLength = false;
                IP.GetComponent<RemoveOnTime>().randomizer = 0f;
                IP.transform.parent = null;
                ImpactSphere = Plugin.RocketBundle.LoadAsset<GameObject>("ImpSphere");
                GameObject ImpSphere = Instantiate<GameObject>(ImpactSphere, transform.position, Quaternion.identity);
                ImpSphere.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
                ImpSphere.AddComponent<FadeAway>();
                ImpSphere.transform.parent = null;
                EventHandler.PlayClip(ImpactR, transform.position, 1f, 10f);
                owner.GetComponent<RocketJumper>().FiredRockets.Remove(transform.gameObject);
                Destroy(transform.gameObject);
            }
        }
    }
}
