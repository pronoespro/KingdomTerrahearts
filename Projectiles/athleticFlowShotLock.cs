using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    public class athleticFlowShotLock : ModProjectile
    {

        List<int> targetsLocked;
        int curTarget;
        Vector2 originalPos;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shotlock");
            Main.projHook[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width =200;
            Projectile.height = 200;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 25;
            Projectile.light = 1;
            targetsLocked = new List<int>();
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0) { Projectile.ai[0] = MathHelp.Magnitude(Projectile.velocity); }

            Player p = Main.player[Projectile.owner];
            SoraPlayer sp = p.GetModPlayer<SoraPlayer>();



            Projectile.velocity = MathHelp.Normalize(Projectile.velocity) * (Projectile.ai[0]+(sp.CheckPlayerLevel()/2f));

            Projectile.scale = 0.25f + (sp.CheckPlayerLevel() / 20f);

            if (Projectile.timeLeft>targetsLocked.Count*5)
            {

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && KingdomTerrahearts.instance.IsEnemy(i) && !targetsLocked.Contains(i) && IsInDistance(i))
                    {
                        targetsLocked.Add(i);
                    }
                }

            }
            else
            {
                sp.SetContactinvulnerability(15);

                if (originalPos == Vector2.Zero)
                {
                    originalPos = p.Center;
                }

                if (++Projectile.ai[1] > 5)
                {
                    curTarget++;
                    originalPos = p.Center;
                }

                if (targetsLocked.Count > 0 && curTarget<targetsLocked.Count && Main.npc[targetsLocked[curTarget]].active)
                {
                    p.Center =Vector2.Lerp(originalPos, Main.npc[targetsLocked[curTarget]].Center,Math.Clamp(Projectile.ai[1]/4f,0,1));
                    p.velocity = Vector2.Zero;
                    p.gravity = 0;
                }
            }
        }

        public bool IsInDistance(int npc)
        {
            int minDistance =(int)(Projectile.width*Projectile.scale + (Main.npc[npc].width + Main.npc[npc].height) / 2f);
           return Vector2.Distance(Main.npc[npc].Center, Projectile.Center) < minDistance;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            foreach(int target in targetsLocked)
            {
                Texture2D tex = ModContent.Request<Texture2D>("KingdomTerrahearts/Projectiles/athleticFlowShotLock_target").Value;
                Main.spriteBatch.Draw(tex,Main.npc[target].Center-new Vector2(tex.Width/2,tex.Height/2)-Main.screenPosition,lightColor);
            }
        }

    }
}
