using IlluminanceMeasure.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IlluminanceMeasure.Demo
{
    public class MeasureDemo : MonoBehaviour
    {
        [SerializeField]
        IlluminanceMeasureController _illuminanceMeasureController;

        [SerializeField]
        MeasureMapDemo _measureMapDemo;

        [SerializeField]
        MeasureLineDemo _measureLineDemo; 

        private List<Vector3> _task;
        private int _taskIndex = 0;


        // Start is called before the first frame update
        void Start()
        {
            _task = new List<Vector3>();
            for (int z = -1; z <= 1; z++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (!(x == 0 && y == 0 && z == 0))
                        {
                            _task.Add(new Vector3(x, y, z));
                        }
                    }

                }
            }
            _taskIndex = 0;

            _measureMapDemo.Setup(_illuminanceMeasureController);
            _measureLineDemo.Setup(_illuminanceMeasureController);
        }

        // Update is called once per frame
        void Update()
        {
            bool ret = _illuminanceMeasureController.Measure(_task[_taskIndex], (Color[,] map) =>
            {
                _measureMapDemo.UpdateState(_task[_taskIndex].x, _task[_taskIndex].y, _task[_taskIndex].z, map);
                _measureLineDemo.UpdateState(_task[_taskIndex].x, _task[_taskIndex].y, _task[_taskIndex].z, map);

                _taskIndex = (_taskIndex + 1) % _task.Count;
            });
        }

    }
}