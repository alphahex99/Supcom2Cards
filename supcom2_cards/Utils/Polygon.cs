using Supcom2Cards.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Supcom2Cards
{
    public class Polygon
    {
        private int _edges = 3;
        public int Edges
        {
            get => _edges;

            set
            {
                _edges = value;
                a = 6.283185307179586476925286766559f / _edges;
            }
        }

        public float Size = 1f;

        // angle between 2 points
        private float a = 6.283185307179586476925286766559f / 3;

        public void Draw(float x, float y, List<Laser> lasers, float angle = 0f)
        {
            float x1;
            float y1;
            float x2 = x + Size * Mathf.Cos((lasers.Count - 1) * a - angle);
            float y2 = y + Size * Mathf.Sin((lasers.Count - 1) * a - angle);

            for (int i = 0; i < lasers.Count; i++)
            {
                x1 = x2;
                y1 = y2;

                float b = i * a - angle;
                x2 = x + Size * Mathf.Cos(b);
                y2 = y + Size * Mathf.Sin(b);

                lasers[i].Draw(x1, y1, x2, y2);
            }
        }
    }
}
