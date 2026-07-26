using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KLRB.Utility
{

    [System.Serializable]
    public class ConfigurablePoint
    {
        public string name = "Pivot";
        public Vector3 position;
        public Vector3 direction = Vector3.forward;
        public int display = 0;
        public bool modify = false;
        public Color color = Color.blue;
    }
}