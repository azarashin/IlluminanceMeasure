using IlluminanceMeasure.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IlluminanceMeasure.Demo
{
    public class MeasureMapDemo : MonoBehaviour
    {
        [SerializeField]
        RawImage _target;

        private Texture2D _drawTexture;
        private Color32[] buffer;



        // Start is called before the first frame update
        public void Setup(IlluminanceMeasureController illuminanceMeasureController)
        {
            _drawTexture = new Texture2D(illuminanceMeasureController.Width, illuminanceMeasureController.Height, TextureFormat.RGBA32, false);
            _drawTexture.filterMode = FilterMode.Point;
            buffer = new Color32[illuminanceMeasureController.Width * illuminanceMeasureController.Height];
            _target.texture = _drawTexture;

        }

        internal void UpdateState(float dx, float dy, float dz, Color[,] map)
        {
            Color color = IlluminanceMeasureController.GetColor(map, 0.1f);
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            int sx = (int)(width * (dx + 1) / 3.0f);
            int sy = (int)(height * (dy + 1) / 3.0f);
            int cellWidth = (int)(width / 9);
            int cellHeight = (int)(height / 3);
            sx += (int)(cellWidth * (dz + 1));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x >= sx && y >= sy && x < sx + cellWidth && y < sy + cellHeight)
                    {
                        buffer[y * width + x] = color;
                    }
                }
            }
            _drawTexture.SetPixels32(buffer);
            _drawTexture.Apply();
        }
    }
}