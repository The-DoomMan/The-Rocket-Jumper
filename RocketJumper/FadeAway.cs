using System;
using UnityEngine;

namespace RocketJumper
{
    class FadeAway : MonoBehaviour
    {
        Vector3 Fullscale;
        void Awake()
        {
            Fullscale = transform.localScale;
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        void Update()
        {
            transform.GetComponentInChildren<Renderer>().materials[0].color = Vector4.Lerp(transform.GetComponentInChildren<Renderer>().materials[0].color, new Vector4(1, 1, 1, 0), 5f * Time.deltaTime);
            transform.localScale = Vector3.Slerp(transform.localScale, Fullscale, 5f * Time.deltaTime);
            if (transform.localScale == Fullscale)
            {
                Destroy(transform.gameObject);
            }
        }
    }
}
