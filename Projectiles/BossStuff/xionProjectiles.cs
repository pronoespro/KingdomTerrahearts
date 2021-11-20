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
            Projectile.width = 331;
            Projectile.height = 461;
            Projectile.scale = 1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 0.75f;
            Projectile.timeLeft = 250;
            Projectile.ignoreWater = true;
        }

    }



    public class xion_finalPhase_arms : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xion's Keyblade");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 686;
            Projectile.height = 243;
            Projectile.scale = 1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 250;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.light = 1;
        }

        public override void AI()
        {
            Projectile.frame = (int)Projectile.ai[0]+1;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
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
            Projectile.width = 450;
            Projectile.height = 10;
            Projectile.scale = 1;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            Projectile.light = 1;
            //Projectile.hide = true;
        }

        public override void AI()
        {

            for(int i = 0; i < 50; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height * 100, DustID.GoldCoin);
            }

            if (Projectile.timeLeft > 50 && Projectile.timeLeft < 150)
            {

                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && Main.player[i].Center.X > Projectile.position.X && Main.player[i].Center.X < Projectile.Center.X + Projectile.width/2f)
                    {
                        Main.player[i].Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Main.player[i].name + " saw the light."), Projectile.damage, 0);
                    }
                }

            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 originalPos = new Vector2(Projectile.position.X - Main.screenPosition.X, Projectile.position.Y - Main.screenPosition.Y);
            for (int i = 100 - Projectile.timeLeft * 4; i < Math.Min(800 - Projectile.timeLeft * 4, 100); i++)
            {
                Color c = new Color(255f, 255f, 255f, 0f);
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/BossStuff/xion_finalPhase_lightBeam"), originalPos + new Vector2(0, 10 * i), c);
            }
            return false;
        }

    }

}
