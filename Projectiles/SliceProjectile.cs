using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class SliceProjectile : ModProjectile
    {

        private string currentTexture = "";
        private int distanceToPlayer = 25;
        private Vector2 offset;
        private bool flipVertically;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slice");
            Main.projFrames[Type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.alpha = 150;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override void AI()
        {
            Projectile.ai[0] =(Projectile.ai[0]==0)?Projectile.timeLeft:Projectile.ai[0];

            Projectile.frame = (int)((1f - Projectile.timeLeft / Projectile.ai[0])*8f);

            Projectile.Center = Main.player[Projectile.owner].Center+new Vector2((float)Math.Sin(-Projectile.rotation+(float)Math.PI/2),(float)Math.Cos(-Projectile.rotation + (float)Math.PI / 2))*distanceToPlayer+offset;

        }

        public void ChangeDistanceToPlayer(int newDistance)
        {
            distanceToPlayer = newDistance;
        }

        public void ChangeOffset(Vector2 newOffset)
        {
            offset = newOffset;
        }

        public void ChangeTexture(string textureType)
        {
            currentTexture = textureType;
        }

        public void FlipVertically(bool flip = true)
        {
            flipVertically = flip;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (currentTexture.Length > 0 && currentTexture != "None")
            {

                Texture2D texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + currentTexture).Value;

                if (texture != null)
                {
                    Rectangle rect = new Rectangle(0, (int)(Projectile.frame * (texture.Height/8f)), texture.Width, texture.Height / 8);

                    Main.spriteBatch.Draw(texture, Projectile.Center-Main.screenPosition, rect, lightColor, Projectile.rotation, new Vector2(texture.Width/2f,texture.Height/2f/8f), Projectile.scale,GetSpriteEffect(), 0);
                    return false;
                }
            }
            return base.PreDraw(ref lightColor);
        }

        private SpriteEffects GetSpriteEffect()
        {
            if (flipVertically)
            {
                return SpriteEffects.FlipVertically;
            }
            else
            {
                return (Projectile.direction < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }
        }

    }
}
