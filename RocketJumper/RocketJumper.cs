using System;
using System.Collections.Generic;
using UnityEngine;

namespace RocketJumper
{
    class RocketJumper : MonoBehaviour
    {
        public Animator RJAnimator;
        public NewMovement nmov;
        bool detonating;

        public GameObject Projectile;
        public List<GameObject> FiredRockets = new List<GameObject>();
        public float fireCD = 0f;
        public AudioClip Fire;
        public AudioClip Impact;

        public string blastsource = "";
        public Transform FirePoint;
        void OnEnable()
        {
            if (transform.gameObject.activeSelf && transform.GetComponent<WeaponIcon>())
                transform.GetComponent<WeaponIcon>().UpdateIcon();
            nmov = GameObject.FindGameObjectWithTag("Player").GetComponent<NewMovement>();
        }

        void Update()
        {
            if (fireCD > 0)
            {
                fireCD -= 5f * Time.deltaTime;
            }

            if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && GameObject.FindWithTag("GunControl").GetComponent<GunControl>().activated && !MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && RJAnimator && Projectile && fireCD <= 0 && !detonating)
            {
                RJAnimator.SetTrigger("Fire");
                fireCD = 3.75f;
                EventHandler.PlayClip(Fire, transform.position, 1f, 75f);
                GameObject Rocket = Instantiate<GameObject>(Projectile, FirePoint.transform.position + GameObject.FindWithTag("MainCamera").transform.forward, GameObject.FindWithTag("MainCamera").transform.rotation);
                Rocket.AddComponent<RocketBehaviour>();
                Rocket.AddComponent<ShaderSwapper>();
                EventHandler.RemoveInTime(Rocket, 13f, 0, false);
                Rocket.GetComponent<RocketBehaviour>().owner = transform;
                Rocket.GetComponent<RocketBehaviour>().Dir = Rocket.transform.forward;
                Rocket.GetComponent<RocketBehaviour>().ImpactPart = Plugin.RocketBundle.LoadAsset<GameObject>("ExplosionPuff");
                Rocket.GetComponent<RocketBehaviour>().ImpactR = Impact;
                Rocket.transform.localScale = new Vector3(10f, 10f, 10f);
                Rocket.gameObject.layer = 1;
                FiredRockets.Add(Rocket);
                Rocket.transform.parent = null;
            }
            if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && FiredRockets.Count > 0 && RJAnimator)
            {
                detonating = true;
                foreach (GameObject Rocket in FiredRockets)
                {
                    if (Rocket)
                        Rocket.GetComponent<RocketBehaviour>().Detonate();
                }
                FiredRockets.Clear();
                detonating = false;
            }
        }
    }
}
