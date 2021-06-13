using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Escuregot_projectile:ModProjectile
    {

        bool playerNear;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Escuregot projectile");
            Main.projFrames[projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 42;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.penetrate =3;
            projectile.tileCollide = true;
            projectile.timeLeft = 1700;
            projectile.light = 1;
            Player p = Main.player[projectile.owner];
            Projectile.NewProjectile(p.Center, p.velocity, mod.ProjectileType("Escuregot_explosion"), 0, 0);
            projectile.damage =(int)(projectile.damage* 0.25f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity=Vector2.Zero;
            projectile.ai[1] = 1;
            return false;
        }

        public override void AI()
        {

            if (projectile.ai[1] == 0)
                projectile.velocity.Y++;

            playerNear = false;
            for(int i = 0; i < Main.ActivePlayersCount; i++)
            {
                if (Main.player[i].active && Vector2.Distance(Main.player[i].Center, projectile.Center) < 400)
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
                if (Main.npc[i].active && Vector2.Distance(Main.npc[i].Center, projectile.Center) < 500)
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
                projectile.ai[0]=(projectile.ai[0]>=30)?0:projectile.ai[0]+1;
                projectile.frame = (int)(projectile.ai[0] / 20)+3;
            }
            else
            {
                projectile.ai[0] = (projectile.ai[0] >= 30) ? 0 : projectile.ai[0] + 1;
                projectile.frame = (int)(projectile.ai[0] / 20);
            }

        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("Escuregot_explosion"), 0, 0);
            return base.PreKill(timeLeft);
        }

    }
}
