using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace KingdomTerrahearts.Mounts
{
    public class LightsOfThePast:FlyingMountBase
    {

        public Projectile[] keybladesFromThePast = new Projectile[50];
        public Vector2[] keyOffsets = new Vector2[50];
        public float[] keySinePos = new float[50];

        public override void SetStaticDefaults()
        {
            MountData.spawnDust = DustID.Confetti;
            //MountData.buff = mod.BuffType("mewwowBuff");
            MountData.heightBoost = 0;
            MountData.fallDamage = 0f;
            MountData.runSpeed =0;
            MountData.dashSpeed = 0;
            MountData.flightTimeMax = 0;
            MountData.fatigueMax = 0;
            MountData.jumpHeight = 0;
            MountData.acceleration = 0.3f;
            MountData.jumpSpeed = 0;
            MountData.blockExtraJumps = false;
            MountData.totalFrames = 1;
            MountData.constantJump = true;
            int[] array = new int[MountData.totalFrames];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
            MountData.playerYOffsets = array;
            MountData.xOffset = 0;
            MountData.yOffset = 0;
            MountData.bodyFrame = 0;
            MountData.playerHeadOffset = 0;
            MountData.standingFrameCount = 0;
            MountData.standingFrameDelay = 0;
            MountData.runningFrameCount = 0;
            MountData.runningFrameDelay = 0;
            MountData.runningFrameStart = 0;
            MountData.flyingFrameCount = 0;
            MountData.flyingFrameDelay = 0;
            MountData.flyingFrameStart = 0;
            MountData.inAirFrameCount = 0;
            MountData.inAirFrameDelay = 0;
            MountData.inAirFrameStart = 0;
            MountData.idleFrameCount = 0;
            MountData.idleFrameDelay = 0;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = false;
            MountData.swimFrameCount = 0;
            MountData.swimFrameDelay = 0;
            MountData.swimFrameStart = 0;
            if (Main.netMode != NetmodeID.Server)
            {
                MountData.textureWidth = MountData.frontTexture.Width();
                MountData.textureHeight = MountData.frontTexture.Height();
            }
        }

        public override void SpecialEffects()
        {

            Player p = Main.player[Main.myPlayer];

            ProjectileSource_BySourceId s = new ProjectileSource_BySourceId(-1);

            if (keybladesFromThePast[0].active == false)
            {
                for(int i = 0; i < keybladesFromThePast.Length; i++)
                {
                    keybladesFromThePast[i]=Main.projectile[Projectile.NewProjectile(s,p.position,Vector2.Zero,ModContent.ProjectileType<Projectiles.ultimate_projectile_stab>(),0,0)];
                    keySinePos[i] = Main.rand.NextFloat(-1f, 1f);
                    keyOffsets[i] = Main.rand.NextVector2Circular(25, 2);
                }
            }

            for(int i = 0; i < keybladesFromThePast.Length; i++)
            {
                keybladesFromThePast[i].timeLeft = 1000;
                keybladesFromThePast[i].Center = keyOffsets[i] + p.Center;
            }

        }

        public override void Dismount(Player player, ref bool skipDust)
        {
            for(int i = 0; i < keybladesFromThePast.Length; i++)
            {
                keybladesFromThePast[i].timeLeft = 0;
            }
            keybladesFromThePast = new Projectile[keybladesFromThePast.Length];
        }

    }
}
