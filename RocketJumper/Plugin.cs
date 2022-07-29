using BepInEx;
using UnityEngine;
using HarmonyLib;

namespace RocketJumper
{
    [BepInPlugin("RocketJumper", "Rocket Jumper", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static AssetBundle RocketBundle;
        bool weapongiven;

        GameObject cCamera;
        GameObject RocketJumper;
        GameObject gControl;


        private void Awake()
        {
            RocketBundle = AssetBundle.LoadFromMemory(Resource1.rjumper);
            RocketBundle.LoadAllAssets();
            Harmony harmony = new Harmony("RocketJumper");
            harmony.PatchAll(typeof(Plugin));
        }

        void Update()
        {
            if(!weapongiven && RocketBundle)
            {
                GiveRocketJumper();
                weapongiven = true;
            }
            if(!checkgun())
            {
                weapongiven = false;
            }
        }

        public bool checkgun()
        {
            cCamera = GameObject.FindGameObjectWithTag("MainCamera");
            gControl = GameObject.FindGameObjectWithTag("GunControl");
            if (cCamera && gControl)
            {
                for (int i = 0; i < gControl.transform.childCount; i++)
                {
                    if (gControl.transform.GetChild(i).GetComponent<RocketJumper>())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void GiveRocketJumper()
        {
            cCamera = GameObject.FindWithTag("MainCamera");
            gControl = GameObject.FindWithTag("GunControl");
            if (cCamera & gControl)
            {
                GameObject RocketLauncher = RocketBundle.LoadAsset<GameObject>("RocketJumper");
                GameObject RLObject = Object.Instantiate<GameObject>(RocketLauncher, gControl.transform);
                RLObject.transform.localPosition = new Vector3(0.4f, -0.4f, 0.5f);
                RLObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                RLObject.gameObject.layer = 13;
                SkinnedMeshRenderer[] componentsInChildren = RLObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].gameObject.layer = 13;
                }
                RLObject.AddComponent<ShaderSwapper>();
                RLObject.AddComponent<WeaponIcon>();
                RLObject.AddComponent<RocketJumper>();
                gControl.AddComponent<EventHandler>();
                RocketJumper RJ = RLObject.GetComponent<RocketJumper>();
                RJ.RJAnimator = RLObject.GetComponentInChildren<Animator>();
                RJ.Projectile = RocketBundle.LoadAsset<GameObject>("RocketProj");
                RJ.Fire = RocketBundle.LoadAsset<AudioClip>("Fire");
                RJ.Impact = RocketBundle.LoadAsset<AudioClip>("Impact");
                RJ.FirePoint = RJ.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).transform;
                RLObject.GetComponent<WeaponIcon>().weaponIcon = RocketBundle.LoadAsset<Sprite>("Icon");
                RLObject.GetComponent<WeaponIcon>().glowIcon = RocketBundle.LoadAsset<Sprite>("IconGlow");
                gControl.GetComponent<GunControl>().allWeapons.Add(RLObject);
                gControl.GetComponent<GunControl>().slot5.Add(RLObject);
                RocketJumper = RLObject;
                RocketJumper.SetActive(false);
            }
        }
    }
}
