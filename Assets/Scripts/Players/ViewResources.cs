using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class ViewResources
    {
        public int Id = 0;

        public string Text = "";

        public Color Color = Color.white;

        public Sprite Image = null;

        public ViewResources() { }

        public ViewResources(string text, Color color, Sprite image) => set(text, color, image);

        public ViewResources(int id, string text, Color color, Sprite image) => set(id, text, color, image);

        public ViewResources(ViewResources resources) => set(resources.Text, resources.Color, resources.Image);

        public ViewResources set(int id,  string text, Color color, Sprite image)
        {
            Id = id;
            Text = text;
            Color = color;
            Image = image;

            return set(text, color, image);
        }

        public ViewResources set(string text, Color color, Sprite image)
            => set(Id, text, color, image);

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ViewResources)) return false;
            else
            {
                ViewResources resources = (ViewResources)obj;
                if (resources == this) 
                    return true;
                else
                    return resources.Id == Id && resources.Text == Text && resources.Color == Color && resources.Image == Image;
            }
        }
    }
}
