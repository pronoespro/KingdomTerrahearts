using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    class athleticFlowShotLock : ModProjectile
    {

        int level=1;

        public override void SetStaticDefaults()
        {
            projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 7;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.damage = 1;
            projectile.tileCollide = false;
            projectile.timeLeft *= 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.NewText("Ouch");
        }

        public override bool? CanUseGrapple(Player player)
        {

            Vector2 velNorm = Main.MouseWorld - player.position;
            velNorm.Normalize();

            SoraPlayer sp = Main.player[projectile.owner].GetModPlayer<SoraPlayer>();
            level = sp.CheckPlayerLevel()+1;

            if (Math.Abs(velNorm.Y) > 0.3f && level<5)
            {
                return false;
            }

            int hookOut = 0;
            for (int i=0; i < 1000; i++){

                if(Main.projectile[i].active && Main.projectile[i].owner==Main.myPlayer && Main.projectile[i].type == projectile.type)
                {
                    hookOut++;
                }

            }
            if (hookOut != 0)
            {
                return false;
            }

            return true;
        }

        public override float GrappleRange()
        {
            return 150+(level*100);
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1;
        }

        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 60*level;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 40*level;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,SamplerState.LinearClamp,DepthStencilState.Default,RasterizerState.CullNone,null,Main.GameViewMatrix.ZoomMatrix);

            Texture2D texture = ModContent.GetTexture("KingdomTerrahearts/Projectiles/athleticFlowShotLock_chain");
            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;

            Rectangle? sourceRectangle = new Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width / 2f, (float)texture.Height / 2f);
            float num1 = (float)texture.Height;
            
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X)-1.57f;
            bool flag = true;
            if((float.IsNaN(position.X)&& float.IsNaN(position.Y))|| (float.IsNaN(vector2_4.X)&&float.IsNaN(vector2_4.Y)))
            {
                flag= false;
            }
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Color color2 = Lighting.GetColor((int)(position.X / 16), (int)((double)position.Y / 16.0));
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0);
                }
            }

        }
        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            return false;
        }

    }
}
