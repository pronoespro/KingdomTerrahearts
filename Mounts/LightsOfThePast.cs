using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Mounts
{
    public class LightsOfThePast:FlyingMountBase
    {

        public Projectile[] keybladesFromThePast = new Projectile[50];
        public Vector2[] keyOffsets = new Vector2[50];
        public float[] keySinePos = new float[50];

        public override void SetDefaults()
        {
            mountData.spawnDust = DustID.Confetti;
            //mountData.buff = mod.BuffType("mewwowBuff");
            mountData.heightBoost = 0;
            mountData.fallDamage = 0f;
            mountData.runSpeed =0;
            mountData.dashSpeed = 0;
            mountData.flightTimeMax = 0;
            mountData.fatigueMax = 0;
            mountData.jumpHeight = 0;
            mountData.acceleration = 0.3f;
            mountData.jumpSpeed = 0;
            mountData.blockExtraJumps = false;
            mountData.totalFrames = 1;
            mountData.constantJump = true;
            int[] array = new int[mountData.totalFrames];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
            mountData.playerYOffsets = array;
            mountData.xOffset = 0;
            mountData.yOffset = 0;
            mountData.bodyFrame = 0;
            mountData.playerHeadOffset = 0;
            mountData.standingFrameCount = 0;
            mountData.standingFrameDelay = 0;
            mountData.runningFrameCount = 0;
            mountData.runningFrameDelay = 0;
            mountData.runningFrameStart = 0;
            mountData.flyingFrameCount = 0;
            mountData.flyingFrameDelay = 0;
            mountData.flyingFrameStart = 0;
            mountData.inAirFrameCount = 0;
            mountData.inAirFrameDelay = 0;
            mountData.inAirFrameStart = 0;
            mountData.idleFrameCount = 0;
            mountData.idleFrameDelay = 0;
            mountData.idleFrameStart = 0;
            mountData.idleFrameLoop = false;
            mountData.swimFrameCount = 0;
            mountData.swimFrameDelay = 0;
            mountData.swimFrameStart = 0;
            if (Main.netMode != NetmodeID.Server)
            {
                mountData.textureWidth = mountData.frontTexture.Width;
                mountData.textureHeight = mountData.frontTexture.Height;
            }
        }

        public override void SpecialEffects()
        {

            Player p = Main.player[Main.myPlayer];

            if (keybladesFromThePast[0].active == false)
            {
                for(int i = 0; i < keybladesFromThePast.Length; i++)
                {
                    keybladesFromThePast[i]=Main.projectile[Projectile.NewProjectile(p.position,Vector2.Zero,mod.ProjectileType("ultimate_projectile"),0,0)];
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
