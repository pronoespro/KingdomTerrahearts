using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;

namespace KingdomTerrahearts.Projectiles.ScepTend
{
    public class Wunderwaffe_projectile:ModProjectile
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 176;
            Projectile.height = 144;
            Projectile.friendly = true;
            Projectile.timeLeft = 150;
            Projectile.penetrate = -1;
            Projectile.scale = 0.25f;
            Projectile.light = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            EntitySource_Parent s = new EntitySource_Parent(Projectile);

            int proj = Projectile.NewProjectile(s,target.Center, Vector2.Zero, ModContent.ProjectileType<Wunderwaffe_explosion>(), Projectile.damage, Projectile.knockBack,Projectile.owner);

            if (!isBoss(target))
            {
                target.life = 0;
                target.checkDead();
            }
        }

        public override void Kill(int timeLeft)
        {
            EntitySource_Parent s = new EntitySource_Parent(Projectile);

            int proj = Projectile.NewProjectile(s,Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Wunderwaffe_explosion>(), Projectile.damage, Projectile.knockBack,Projectile.owner);
            SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
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
            Projectile.ai[0] = (Projectile.ai[0] >= 40) ? 0:Projectile.ai[0] + 1;
            Projectile.frame = (int)(Projectile.ai[0]/10);
        }

    }
}
