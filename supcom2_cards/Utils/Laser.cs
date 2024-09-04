using UnityEngine;

namespace Supcom2Cards
{
    public class Laser
    {
        private Color _color = Color.magenta;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                line.material.color = Color;
            }
        }

        private Material _material = new Material(Shader.Find("UI/Default"));
        public Material Material
        {
            get { return _material; }
            set
            {
                _material = value;
                line.material = Material;
            }
        }

        public string Name
        {
            get { return line.name; }
            set { line.name = value; }
        }

        private float _width = 0.3f;
        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                line.startWidth = Width;
                line.endWidth = Width;
            }
        }

        private float _z = -5;
        public float Z
        {
            get { return _z; }
            set
            {
                _z = value;
                cords[0].z = Z;
                cords[1].z = Z;
            }
        }

        private readonly LineRenderer line;
        private readonly Vector3[] cords = new Vector3[2];

        public Laser()
        {
            line = new GameObject().AddComponent<LineRenderer>();
            line.startColor = Color.white;
            line.endColor = Color.white;
            line.useWorldSpace = true;

            line.material.color = Color;
            line.material = Material;
            Name = "Laser";
            line.startWidth = Width;
            line.endWidth = Width;
            cords[0].z = Z;
            cords[1].z = Z;
        }

        ~Laser()
        {
            GameObject.Destroy(line);
        }

        public void Draw(float x1, float y1, float x2, float y2)
        {
            cords[0].x = x1;
            cords[0].y = y1;
            cords[1].x = x2;
            cords[1].y = y2;

            line.SetPositions(cords);
        }

        public void Draw(Vector3 a, Vector3 b)
        {
            Draw(a.x, a.y, b.x, b.y);
        }

        public void DrawHidden()
        {
            Draw(100, 100, 100, 100);
        }
    }
}
