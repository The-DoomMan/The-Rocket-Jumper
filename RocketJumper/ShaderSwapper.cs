using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RocketJumper
{
    //shout out to eps
    class ShaderSwapper : MonoBehaviour
    {

        public ShaderEnum shadersenum;
        public string[] shaders =
            {
            "psx/vertexlit/vertexlit",
            "psx/unlit/unlit",
            "psx/vertexlit/transparent/transparent",
            "psx/vertexlit/transparent/zwrite",
            "psx/unlit/transparent/unlit"
             };

        public enum ShaderEnum
        {
            psxVertexLit, psxUnlit, psxVertexLitTransparent, psxVertexLitTransparentZwrite, psxUnlitTransparent
        }

        public void Start()
        {
            var shader = Shader.Find(shaders[(int)shadersenum]);

            MeshRenderer[] joe = transform.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer i in joe)
            {
                var fuckoff = i.materials;
                foreach (Material m in fuckoff)
                {
                    foreach (string g in shaders)
                    {
                        if (m.shader.name == g)
                        {
                            m.shader = Shader.Find(g);
                        }
                    }
                }
            }
        }
    }
}