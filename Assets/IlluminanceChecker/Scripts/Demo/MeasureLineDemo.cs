using IlluminanceMeasure.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IlluminanceMeasure.Demo
{
    public class MeasureLineDemo : MonoBehaviour
    {
        [SerializeField]
        MeasureLine _prefab;

        [SerializeField]
        Transform _parentOfList; 

        private Dictionary<(float dx, float dy, float dz), MeasureLine> _map;

        public void Setup(IlluminanceMeasureController illuminanceMeasureController)
        {
            _map = new Dictionary<(float dx, float dy, float dz), MeasureLine>();
        }

        public void UpdateState(float dx, float dy, float dz, Color[,] map)
        {
            (float dx, float dy, float dz) key = (dx, dy, dz); 
            if(!_map.ContainsKey(key))
            {
                _map[key] = Instantiate(_prefab, _parentOfList);
                _map[key].Setup(dx, dy, dz); 
            }
            Color color = IlluminanceMeasureController.GetColor(map, 0.1f);
            _map[key].UpdateState(color);
        }

    }
}