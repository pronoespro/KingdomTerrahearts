using KingdomTerrahearts.Extra;
using KingdomTerrahearts.Items.Weapons.Bases;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Weapons
{
    public class LarxeneThrowingKnives_projectiles: ModProjectile
    {

        public override string Texture => "KingdomTerrahearts/Items/Weapons/Org13/Larxene/Knives_Trancheuse";

        public string textureToDraw;
        public float gravityEffect=1;
        public int projectileOnColision;
        public int spawnProjTimeLeft = 0;
        public float spawnProjVelMult = 1f;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.scale = 1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ShootCollisionProjectile(-oldVelocity);
            return base.OnTileCollide(oldVelocity);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            ShootCollisionProjectile(-Projectile.oldVelocity);
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public void ShootCollisionProjectile(Vector2 vel)
        {
            if (projectileOnColision > 0)
            {
                EntitySource_Parent s = new EntitySource_Parent(Projectile);
                int proj = Projectile.NewProjectile(s, Projectile.Center, vel * spawnProjVelMult, projectileOnColision, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].penetrate = Projectile.penetrate;
                if (spawnProjTimeLeft > 0)
                {
                    Main.projectile[proj].timeLeft = spawnProjTimeLeft;
                }
            }
        }

        public override void AI()
        {
            Projectile.rotation += (Projectile.velocity.X > 0 ? 1f : -1f) * MathF.PI * 0.01f*MathHelp.Magnitude(Projectile.velocity);
            Projectile.velocity += new Vector2(0, 0.25f)*gravityEffect;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (textureToDraw.Length == 0)
            {
                return base.PreDraw(ref lightColor);
            }
            else
            {

                Texture2D text = (Texture2D)ModContent.Request<Texture2D>(textureToDraw);

                if (text != null)
                {
                    Rectangle rect = new Rectangle(0, 0, text.Width, text.Height);

                    Main.spriteBatch.Draw(text, Projectile.position-Main.screenPosition, rect, lightColor, Projectile.rotation, new Vector2(text.Width / 2, text.Height / 2),Projectile.scale/2f, SpriteEffects.None, 0);

                    return false;
                }
                else
                {
                    return base.PreDraw(ref lightColor);
                }
            }
        }

    }
}
