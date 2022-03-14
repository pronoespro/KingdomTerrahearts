using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class ultimate_projectile_stab:ModProjectile
    {

        public override string Texture => "KingdomTerrahearts/Projectiles/ultimate_projectile";


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultimate sword");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.light = 1.5f;
            Projectile.maxPenetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<KeybladeDamage>();
            Projectile.penetrate = 1000;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 666666;
            Projectile.scale = 2;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.hide)
            {
                overPlayers.Add(index);
            }
            else
            {
                overPlayers.Remove(index);
            }
        }

        public override void AI()
        {

            KingdomTerrahearts.instance.SetCameraForAllPlayers(Vector2.Zero,1.05f+(1-Projectile.timeLeft/Projectile.ai[1])*0.05f, percentageChange: 100);

            if (Projectile.timeLeft ==666666)
            {
                Projectile.spriteDirection = Projectile.direction = Main.player[Projectile.owner].direction;
                Projectile.timeLeft = (int)Projectile.ai[1];
            }

            float projDir = (float)Math.Sin(Projectile.ai[0] / 10f);

            Projectile.hide = projDir > 0;

            Projectile.Center = Main.player[Projectile.owner].Center + new Vector2((50 - Math.Abs(projDir*30)) *Projectile.spriteDirection, projDir*30f)*3+new Vector2(0, 15);
            Projectile.ai[0]++;

            Main.player[Projectile.owner].direction= Projectile.spriteDirection;
            Main.player[Projectile.owner].velocity = new Vector2(Projectile.spriteDirection * (5+ 55 * (Projectile.timeLeft / Projectile.ai[1])), 0f);
            Main.player[Projectile.owner].gravity = 0;

            if (Main.rand.Next(3) <2)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFairy, -Projectile.velocity.X/3,0);
            }
        }

    }

    public class ultimate_projectile_slice:ModProjectile
    {

        public override string Texture => "KingdomTerrahearts/Projectiles/ultimate_projectile";

        public Vector2 distToMouse;
        public Vector2 initMousePos;
        public float initTime;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ultimate sword");
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.light = 1.5f;
            Projectile.maxPenetrate = -1;
            Projectile.DamageType = ModContent.GetInstance<KeybladeDamage>();
            Projectile.penetrate = 1000;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 666666;

        }

        public override void AI()
        {
            if (Projectile.timeLeft ==666666)
            {
                Projectile.frame = Main.rand.Next(5);
                distToMouse =  new Vector2((float)Math.Sin(Projectile.ai[0]),(float)Math.Cos(Projectile.ai[0]))*150;
                Projectile.timeLeft = (int)Projectile.ai[1];
                Projectile.rotation = (float)Math.Atan2(distToMouse.Y, distToMouse.X);

                initMousePos = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            }

            float distToPos = (Math.Clamp(1 - Projectile.timeLeft * 3f / Projectile.ai[1], -3 + Projectile.timeLeft / Projectile.ai[1] * 2, 1));

            Projectile.Center = initMousePos+distToMouse*distToPos;

            Projectile.scale = 1+Math.Clamp(-distToPos,1,4)*2;

        }

    }
}
