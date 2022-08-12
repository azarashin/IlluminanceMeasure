using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IlluminanceMeasure.Core
{
    public class IlluminanceMeasureController : MonoBehaviour
    {
        [SerializeField]
        RenderTexture _measureTexture;

        [SerializeField]
        Transform _measureCamera;

        [SerializeField]
        Transform _measureObject;

        [SerializeField]
        int _width = 256;

        [SerializeField]
        int _height = 256;

        [SerializeField]
        float _distance = 0.1f;

        [SerializeField]
        float _markerScale = 0.08f;

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        private bool _busy = false;
        private Texture2D _texture; 

        private void Awake()
        {
            EnableCameras(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            _texture = new Texture2D(_measureTexture.width, _measureTexture.height, TextureFormat.RGBAFloat, false);
        }

        private void OnDestroy()
        {
            Destroy(_texture);
        }

        private void EnableCameras(bool enable)
        {
            _measureObject.gameObject.SetActive(enable);
            _measureCamera.gameObject.SetActive(enable);
        }

        public bool Measure(Vector3 dir, Action<Color[,]> notify)
        {
            if(_busy)
            {
                return false; 
            }
            _busy = true; 
            StartCoroutine(CoMeasure(notify, dir));
            return true; 
        }

        private IEnumerator CoMeasure(Action<Color[,]> notify, Vector3 dir)
        {
            _measureCamera.position = _measureObject.position - dir.normalized * _distance;
            _measureCamera.LookAt(_measureObject.position);
            _measureObject.localScale = Vector3.one * _markerScale;

            EnableCameras(true);
            yield return null;
            Color[,] map = GetIlluminationMap();

            EnableCameras(true);
            yield return null;
            _busy = false;
            notify(map);
        }

        private Color[,] GetIlluminationMap()
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = _measureTexture;

            _texture.ReadPixels(new Rect(0, 0, _measureTexture.width, _measureTexture.height), 0, 0);
            _texture.Apply();


            Color[] measure = _texture.GetPixels();
            RenderTexture.active = currentRT;

            Color[,] ret = new Color[_width, _height];

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    float nx = (float)x / _width;
                    float ny = (float)y / _height;
                    int px = Mathf.Clamp((int)(nx * _measureTexture.width), 0, _measureTexture.width - 1);
                    int py = Mathf.Clamp((int)(ny * _measureTexture.height), 0, _measureTexture.height - 1);
                    ret[x, y] = measure[py * _measureTexture.height + px]; 
                }
            }
            return ret; 
        }

        public static Color GetColor(Color[,] map, float range)
        {
            float range2 = range * range;
            float count = 0.0f;
            Color total = Color.black;
            int cx = (int)(map.GetLength(0) * 0.5f);
            int cy = (int)(map.GetLength(1) * 0.5f);
            for (int y=0;y<map.GetLength(1);y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    float nx = (x - cx) * 2.0f / map.GetLength(0);
                    float ny = (y - cy) * 2.0f / map.GetLength(1);
                    if(nx * nx + ny * ny < range2 || (cx == x && cy == y))
                    {
                        count += 1.0f; 
                        total += map[x, y];
                    }

                }
            }
            if(count == 0.0f)
            {
                return Color.black; 
            }
            return total / count; 
        }
    }
}