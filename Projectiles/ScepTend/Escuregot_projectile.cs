using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Escuregot_projectile:ModProjectile
    {

        bool playerNear;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Escuregot Projectile");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);

            Projectile.netImportant = true;
            Projectile.width = 42;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate =3;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 1700;
            Projectile.light = 1;
            Player p = Main.player[Projectile.owner];
            Projectile.NewProjectile(s,p.Center, p.velocity, ModContent.ProjectileType<Escuregot_explosion>(), 0, 0);
            Projectile.damage =(int)(Projectile.damage* 0.25f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity=Vector2.Zero;
            Projectile.ai[1] = 1;
            return false;
        }

        public override void AI()
        {

            if (Projectile.ai[1] == 0)
                Projectile.velocity.Y++;

            playerNear = false;
            for(int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead && Vector2.Distance(Main.player[i].Center, Projectile.Center) < 400)
                {
                    Main.player[i].lifeRegen += 50;
                    Main.player[i].manaRegenDelay -= 10;
                    playerNear = true;
                    if (Main.rand.Next(5) == 0)
                    {
                        Vector2 dif = new Vector2(Main.player[i].width, Main.player[i].height);
                        int dust=Dust.NewDust(Main.player[i].position-dif,Main.player[i].width*2, Main.player[i].height * 2,DustID.Grass);
                    }
                }
            }
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Vector2.Distance(Main.npc[i].Center, Projectile.Center) < 500)
                {
                    if (Main.npc[i].friendly || Main.npc[i].townNPC)
                        Main.npc[i].lifeRegen += 50;
                    else
                    {
                        Main.npc[i].confused = true;
                        Main.npc[i].poisoned = true;
                    }

                    if (Main.rand.Next(5) == 0)
                    {
                        Vector2 dif = new Vector2(Main.npc[i].width, Main.npc[i].height);
                        int dust = Dust.NewDust(Main.npc[i].position - dif, Main.npc[i].width * 2, Main.npc[i].height * 2, DustID.Grass);
                    }
                }
            }

            if (playerNear)
            {
                Projectile.ai[0]=(Projectile.ai[0]>=30)?0:Projectile.ai[0]+1;
                Projectile.frame = (int)(Projectile.ai[0] / 20)+3;
            }
            else
            {
                Projectile.ai[0] = (Projectile.ai[0] >= 30) ? 0 : Projectile.ai[0] + 1;
                Projectile.frame = (int)(Projectile.ai[0] / 20);
            }

        }

        public override bool PreKill(int timeLeft)
        {
            ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);
            Projectile.NewProjectile(s,Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Escuregot_explosion>(), 0, 0);
            return base.PreKill(timeLeft);
        }

    }
}
