using KingdomTerrahearts.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace KingdomTerrahearts.Projectiles.BossStuff
{
    public class Xion_bells:ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight Bell");
        }

        public override void SetDefaults()
        {
            projectile.width = 331;
            projectile.height = 461;
            projectile.scale = 1;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.light = 0.75f;
            projectile.timeLeft = 250;
            projectile.ignoreWater = true;
        }

    }



    public class xion_finalPhase_arms : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xion's Keyblade");
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 686;
            projectile.height = 243;
            projectile.scale = 1;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 250;
            projectile.ignoreWater = true;
            projectile.hide = true;
            projectile.light = 1;
        }

        public override void AI()
        {
            projectile.frame = (int)projectile.ai[0]+1;
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsBehindNPCsAndTiles.Add(index);
        }

    }

    public class xion_finalPhase_lightBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Beam");
        }

        public override void SetDefaults()
        {
            projectile.width = 450;
            projectile.height = 10;
            projectile.scale = 1;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft = 200;
            projectile.ignoreWater = true;
            projectile.light = 1;
            //projectile.hide = true;
        }

        public override void AI()
        {

            for(int i = 0; i < 50; i++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height * 100, DustID.GoldCoin);
            }

            if (projectile.timeLeft > 50 && projectile.timeLeft < 150)
            {

                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && Main.player[i].Center.X > projectile.position.X && Main.player[i].Center.X < projectile.Center.X + projectile.width/2f)
                    {
                        Main.player[i].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Main.player[i].name + " saw the light."), projectile.damage, 0);
                    }
                }

            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 originalPos = new Vector2(projectile.position.X - Main.screenPosition.X, projectile.position.Y-Main.screenPosition.Y);
            for(int i =  100-projectile.timeLeft *4 ; i < Math.Min(800-projectile.timeLeft* 4,100) ; i++)
            {
                Color c = new Color(255f, 255f, 255f, 0f);
                spriteBatch.Draw(mod.GetTexture("Projectiles/BossStuff/xion_finalPhase_lightBeam"), originalPos + new Vector2(0, 10 * i), c);
            }

            return false;
        }

    }

}
