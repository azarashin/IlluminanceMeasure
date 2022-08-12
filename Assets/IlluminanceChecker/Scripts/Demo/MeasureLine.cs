using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IlluminanceMeasure.Demo
{
    public class MeasureLine : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _dx;

        [SerializeField]
        TextMeshProUGUI _dy;

        [SerializeField]
        TextMeshProUGUI _dz;

        [SerializeField]
        Image _bar; 

        public void Setup(float dx, float dy, float dz)
        {
            _dx.text = $"{dx}";
            _dy.text = $"{dy}";
            _dz.text = $"{dz}";
            _bar.transform.localScale = new Vector3(0, 1, 1);
            _bar.color = Color.black; 
        }

        public void UpdateState(Color color)
        {
            float length = (new Vector3(color.r, color.g, color.b)).magnitude;
            _bar.color = color;
            _bar.transform.localScale = new Vector3(length, 1, 1); 
        }
    }
}