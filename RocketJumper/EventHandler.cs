using System;
using System.Reflection;
using UnityEngine;

namespace RocketJumper
{
    class EventHandler : MonoBehaviour
    {

        GameObject Camera;
        GameObject Puncher;

        public NewMovement nmov;
        public float groundTime;
        public string blastsource = "";

        public static void PlayClip(AudioClip clip, Vector3 Pos, float volume, float Maxdistance)
        {
            GameObject aud = Instantiate<GameObject>(new GameObject(), Pos, Quaternion.identity);
            aud.transform.parent = null;
            RemoveInTime(aud, 5f, 0, false);
            aud.AddComponent<AudioSource>();
            aud.GetComponent<AudioSource>().clip = clip;
            aud.GetComponent<AudioSource>().maxDistance = Maxdistance;
            aud.GetComponent<AudioSource>().volume = volume;
            aud.GetComponent<AudioSource>().minDistance = 0f;
            aud.GetComponent<AudioSource>().Play();
        }

        public static void RemoveInTime(GameObject gObject, float Time, float random, bool useaudilenght)
        {
            gObject.AddComponent<RemoveOnTime>();
            gObject.GetComponent<RemoveOnTime>().time = Time;
            gObject.GetComponent<RemoveOnTime>().useAudioLength = useaudilenght;
            gObject.GetComponent<RemoveOnTime>().randomizer = random;
        }

        void Update()
        {
            if (!Camera)
            {
                Camera = GameObject.FindGameObjectWithTag("MainCamera");
                Puncher = Camera.GetComponentInChildren<FistControl>().gameObject;
            }
            if (Puncher && blastsource == "RocketJumper")
            {
                if (Puncher.GetComponentInChildren<Punch>().type == FistType.Standard)
                    typeof(Punch).GetField("damage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Puncher.GetComponentInChildren<Punch>(), 3f);
                else if (Puncher.GetComponentInChildren<Punch>().type == FistType.Heavy)
                    typeof(Punch).GetField("damage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Puncher.GetComponentInChildren<Punch>(), 7.5f);
            }
            else if (Puncher && blastsource != "RocketJumper")
            {
                if (Puncher.GetComponentInChildren<Punch>().type == FistType.Standard)
                    typeof(Punch).GetField("damage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Puncher.GetComponentInChildren<Punch>(), 1f);
                else if (Puncher.GetComponentInChildren<Punch>().type == FistType.Heavy)
                    typeof(Punch).GetField("damage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Puncher.GetComponentInChildren<Punch>(), 2.5f);
            }
            if (!nmov) { nmov = GameObject.FindGameObjectWithTag("Player").GetComponent<NewMovement>(); }
            if (nmov.gc.onGround)
            {
                groundTime += 2f * Time.deltaTime;
            }
            else
            {
                groundTime = 0f;
            }


            if (blastsource == "RocketJumper" && !nmov.gc.onGround)
            {
                nmov.airAcceleration = 10000f;
            }
            else
            {
                nmov.airAcceleration = 6000f;
            }


            if (blastsource == "RocketJumper" && !nmov.gc.onGround)
            {
                nmov.modForcedFrictionMultip = 0.025f;
            }
            else if (groundTime > 75f * Time.deltaTime)
            {
                nmov.modForcedFrictionMultip += 0.05f;
            }

            if (nmov.modForcedFrictionMultip > 1)
            {
                nmov.modForcedFrictionMultip = 1f;
                blastsource = "";
            }

            if (Mathf.Abs(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity.magnitude) < 16.5f && nmov.gc.onGround)
            {
                nmov.modForcedFrictionMultip = 1f;
                blastsource = "";
            }
        }
    }
}
