using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class ItemDrop
    {
        public AnimatedSprite animatedSprite;
        public Vector2 position;
        public ItemDrop(string type, ContentManager content)
        {
            position = new Vector2(500, 500);
            Texture2D texture = content.Load<Texture2D>(type);
            animatedSprite = new AnimatedSprite(texture, 1, 6, 8);
        }

        public bool PickUp(Player player, bool inDistance)
        {
            if (inDistance && player.health < player.maxHealth)
            {
                player.health = player.health + 1;
                return true;
            }
            return false;
        }
    }
}
