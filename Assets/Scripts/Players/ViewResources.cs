using Assets.Scripts.Interface;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class ViewResources
    {
        public int Id;

        public string Text;

        public Color Color;

        public Sprite Image;

        public ViewResources() => set(0, "0", Color.white, null);

        public ViewResources(string text, Color color, Sprite image) => set(text, color, image);

        public ViewResources(int id, string text, Color color, Sprite image) => set(id, text, color, image);

        public ViewResources(ViewResources resources) => set(resources.Text, resources.Color, resources.Image);

        public ViewResources set(int id,  string text, Color color, Sprite image)
        {
            Id = id;
            Text = text;
            Color = color;
            Image = image;

            return this;
        }

        public ViewResources set(string text, Color color, Sprite image)
        {
            Text = text;
            Color = color;
            Image = image;

            return this;
        }

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
