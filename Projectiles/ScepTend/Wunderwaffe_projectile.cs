using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Wunderwaffe_projectile:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 176;
            projectile.height = 144;
            projectile.friendly = true;
            projectile.timeLeft = 150;
            projectile.penetrate = -1;
            projectile.scale = 0.25f;
            projectile.light = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int proj = Projectile.NewProjectile(target.Center, Vector2.Zero, mod.ProjectileType("Wunderwaffe_explosion"), projectile.damage, projectile.knockBack);
            Main.projectile[proj].owner = projectile.owner;

            if (!isBoss(target))
            {
                target.life = 0;
                target.checkDead();
            }
        }

        public override void Kill(int timeLeft)
        {
            int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("Wunderwaffe_explosion"), projectile.damage, projectile.knockBack);
            Main.projectile[proj].owner = projectile.owner;
            Main.PlaySound(SoundID.Item12, projectile.Center);
        }

        bool isBoss(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.TheDestroyer:
                case NPCID.TheDestroyerBody:
                case NPCID.TheDestroyerTail:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsTail:
                case NPCID.SkeletronHand:
                case NPCID.SkeletronHead:
                case NPCID.Golem:
                case NPCID.GolemFistLeft:
                case NPCID.GolemFistRight:
                case NPCID.GolemHead:
                case NPCID.GolemHeadFree:
                case NPCID.MoonLordCore:
                case NPCID.MoonLordFreeEye:
                case NPCID.MoonLordHand:
                case NPCID.MoonLordHead:
                case NPCID.MoonLordLeechBlob:
                case NPCID.LunarTowerNebula:
                case NPCID.LunarTowerSolar:
                case NPCID.LunarTowerStardust:
                case NPCID.LunarTowerVortex:
                    return true;
            }
            return npc.boss;
        }

        public override void AI()
        {
            projectile.ai[0] = (projectile.ai[0] >= 40) ? 0:projectile.ai[0] + 1;
            projectile.frame = (int)(projectile.ai[0]/10);
        }

    }
}
