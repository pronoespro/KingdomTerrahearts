using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace KingdomTerrahearts.Projectiles.Summons
{
    public class Dumbo: SummonBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dumbo");
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 78/2;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.scale = 2f;
            Projectile.light = 1f;
            Projectile.timeLeft = 1000;
        }

        public override void AI()
        {

            TargetEnemie();

            Projectile.ai[0]++;
            Projectile.frame = (int)(Projectile.ai[0] / 50f)%2;

            if (target!=-1 && Vector2.Distance(Main.npc[target].Center, Projectile.Center)<750)
            {
                if (Vector2.Distance(Main.npc[target].Center, Projectile.Center) > 400)
                {
                    Projectile.velocity = MathHelp.Normalize(Main.npc[target].Center - Projectile.Center);

                    Projectile.spriteDirection = (Projectile.velocity.X == 0) ? Projectile.spriteDirection : ((Projectile.velocity.X > 0) ? 1 : -1);
                    Projectile.rotation = (Projectile.spriteDirection > 0) ? (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) : (float)(Math.PI + Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
                }
                else
                {
                    Projectile.spriteDirection = (Main.npc[target].Center - Projectile.Center).X > 0 ? 1 : -1;
                    if (Projectile.ai[0] % 2 == 0)
                        {
                            EntitySource_Parent s = new EntitySource_Parent(Projectile);

                        int proj = Projectile.NewProjectile(s,Projectile.Center, MathHelp.Normalize(Main.npc[target].Center+new Vector2(0,25) - Projectile.Center) * 5, ProjectileID.WaterStream, Projectile.damage/4, 3, Projectile.owner);
                        Main.projectile[proj].timeLeft = 50;

                        if (Projectile.ai[0] % 120 == 0)
                        {
                            int itm=Item.NewItem(s,Projectile.getRect(), ItemID.Star);
                            Main.item[itm].velocity = new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3));
                        }

                    }

                    Projectile.velocity = Vector2.Zero;

                    Projectile.rotation = 0;
                }
            }
            else
            {
                if(Vector2.Distance(Main.player[Projectile.owner].Center, Projectile.Center) > 200)
                {
                    Projectile.velocity = MathHelp.Normalize(Main.player[Projectile.owner].Center - Projectile.Center);
                    if (Vector2.Distance(Main.player[Projectile.owner].Center, Projectile.Center) > 700)
                        Projectile.timeLeft = 2;

                    Projectile.spriteDirection = (Projectile.velocity.X == 0) ? Projectile.spriteDirection : ((Projectile.velocity.X > 0) ? 1 : -1);
                    Projectile.rotation = (Projectile.spriteDirection > 0) ? (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) : (float)(Math.PI + Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
                }
                else
                {
                    Projectile.velocity = Vector2.Zero;
                    Projectile.rotation = 0;
                }
            }


        }

    }
}
