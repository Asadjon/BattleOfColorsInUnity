using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts
{
    public class ViewResources
    {
        private int m_Id;
        private string m_Text;
        private Color m_Color;
        private Sprite m_Image;

        public int Id { get => m_Id; set => m_Id = value; }

        public string Text {
            get => m_Text;
            set 
            {
                m_Text = value;
                if (onChangeResources != null)
                    onChangeResources.OnChangeResource(ChangedType.Text);
            } 
        }
        public Color Color
        {
            get => m_Color;
            set
            {
                m_Color = value;
                if (onChangeResources != null)
                    onChangeResources.OnChangeResource(ChangedType.Color);
            }
        }
        public Sprite Image
        {
            get => m_Image;
            set
            {
                m_Image = value;
                if (onChangeResources != null)
                    onChangeResources.OnChangeResource(ChangedType.Image);
            }
        }

        public IOnChangeResources onChangeResources { private get; set; } = null;

        public enum ChangedType { Text, Color, Image }

        public ViewResources() => set(0, "0", Color.white, null);

        public ViewResources(string text, Color color, Sprite image) => set(text, color, image);

        public ViewResources(int id, string text, Color color, Sprite image) => set(id, text, color, image);

        public ViewResources(string text, Color color, Sprite image, IOnChangeResources onChange)
        {
            onChangeResources = onChange;
            set(text, color, image);
        }

        public ViewResources(int id, string text, Color color, Sprite image, IOnChangeResources onChange)
        {
            onChangeResources = onChange;
            set(id, text, color, image);
        }

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
                    return resources.Id == Id && resources.Text == Text && resources.Color == Color /*&& resources.Image == Image*/;
            }
        }
    }
}
