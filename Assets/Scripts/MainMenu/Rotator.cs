using UnityEngine;
using System.Collections;

namespace Seni.UI{
    public class Rotator : MonoBehaviour {

        public float speed;

        // Update is called once per frame
        void Update () {
            transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}