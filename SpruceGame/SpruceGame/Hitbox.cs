using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpruceGame
{
    [Serializable]
    public class Hitbox
    {
        private List<SerializableRectangle> boxes;
        public List<SerializableRectangle> rectangles
        {
            get => boxes==null?new List<SerializableRectangle>():boxes;
        }
        public Hitbox() => boxes = new List<SerializableRectangle>();
        public Hitbox(Rectangle box) : this() => boxes.Add(new SerializableRectangle(box));
        public Hitbox(ICollection<Rectangle> boxes) : this()
        {
            foreach(Rectangle box in boxes){
                this.boxes.Add(new SerializableRectangle(box));
            }
        }
        public void Add(Rectangle box) => boxes.Add(new SerializableRectangle(box));
        public Hitbox Adjust(Coord offset)
        {
            Hitbox returnVal = new Hitbox();
            foreach(SerializableRectangle box in boxes)
            {
                returnVal.Add(new Rectangle(box.rectangle.X+(int)offset.x,box.rectangle.Y+(int)offset.y,box.rectangle.Width,box.rectangle.Height));
            }
            return returnVal;
        }
        public static Boolean IsIn(Hitbox hitbox, Coord pos)
        {
            if (hitbox == null)
            {
                return false;
            }
            foreach (SerializableRectangle rectangle1 in hitbox.boxes)
            {
                if (rectangle1.rectangle.Contains(pos.x,pos.y))
                {
                    return true;
                }
            }
            return false;
        }
        public static Boolean Collides(Hitbox hitbox1, Hitbox hitbox2)
        {
            if(hitbox1==null || hitbox2 == null)
            {
                return false;
            }
            foreach(SerializableRectangle rectangle1 in hitbox1.boxes)
            {
                foreach(SerializableRectangle rectangle2 in hitbox2.boxes)
                {
                    if (rectangle1.rectangle.Intersects(rectangle2.rectangle))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    [Serializable]
    public class SerializableRectangle
    {
        private Coord origin;
        private Coord size;
        public Rectangle rectangle
        {
            get => new Rectangle(origin.ToPoint(),size.ToPoint());
            set
            {
                origin = value.Location.toCoord();
                size = value.Size.toCoord();
            }
        }
        public SerializableRectangle(Rectangle rectangle) => this.rectangle = rectangle;
    }
}
