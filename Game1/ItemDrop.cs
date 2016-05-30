using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class ItemDrop
    {
        public AnimatedSprite animatedSprite;
        
        public ItemDrop(string type, ContentManager content)
        {
            Texture2D texture = content.Load<Texture2D>("heartfloat");
            animatedSprite = new AnimatedSprite(texture, 1, 6, 8);
        }
    }
}
