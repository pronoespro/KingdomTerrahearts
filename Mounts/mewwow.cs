using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Mounts
{
    public class mewwow:ModMount
    {

        bool isGrounded = false;
        bool attacking = false;
        int timeOnGround = 0;

        public override void SetMount(Player player, ref bool skipDust)
        {
            MountData.spawnDust = DustID.Confetti;
            MountData.buff = ModContent.BuffType<Buffs.mewwowBuff>();
            MountData.heightBoost = 30;
            MountData.fallDamage = 0f;
            MountData.runSpeed = 4f;
            MountData.dashSpeed = 2f;
            MountData.flightTimeMax = 0;
            MountData.fatigueMax = 0;
            MountData.jumpHeight = 10;
            MountData.acceleration = 0.19f;
            MountData.jumpSpeed = 10f;
            MountData.blockExtraJumps = false;
            MountData.totalFrames = 1;
            MountData.constantJump = true;
            int[] array = new int[MountData.totalFrames];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 30;
            }
            MountData.playerYOffsets = array;
            MountData.xOffset = 7;
            MountData.yOffset = 10;
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


        public override void UpdateEffects(Player player)
        {


            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            if (sp.Grounded())
            {

                int dustAmmount = 10;

                if (isGrounded)
                {
                    if (Math.Abs(player.velocity.X) > 0.1f)
                    {
                        SoundEngine.PlaySound(SoundID.Item16, new Vector2(player.Center.X, player.Center.Y));
                        timeOnGround++;
                        if (timeOnGround > 2)
                        {
                            player.velocity.Y = -MountData.jumpHeight / 2;
                            timeOnGround = 0;
                        }
                    }
                }
                else { 
                    Rectangle rect = player.getRect();

                    rect.X -= rect.Width*4;
                    rect.Width *= 16;

                    if (attacking)
                    {
                        dustAmmount *= 5;
                        SoundEngine.PlaySound(SoundID.Item14, new Vector2(player.Center.X, player.Center.Y));
                        for (int i = 0; i < Main.npc.Length; i++)
                        {
                            if (Main.npc[i] == null)
                                break;
                            if (Main.npc[i].getRect().Intersects(rect) && Main.npc[i].CanBeChasedBy(player))
                            {
                                Main.npc[i].life -= 25;
                                Main.npc[i].HitEffect(dmg: 25);
                                if (Main.npc[i].life <= 0)
                                {
                                    Main.npc[i].NPCLoot();
                                }
                            }
                        }
                    }

                    rect.X += rect.Width / 16;
                    rect.Width /= 4;

                    rect.Y += (int)(player.height*0.75f);
                    rect.Height /= 2;

                    for (int i = 0; i < dustAmmount; i++)
                    {
                        int newDust = Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, MountData.spawnDust = DustID.Confetti);
                        Main.dust[newDust].color = new Color(Main.rand.Next(50, 360), Main.rand.Next(50, 360), Main.rand.Next(50, 360));
                    }

                    isGrounded = true;
                }
                attacking = false;
            }
            else
            {
                isGrounded = false;
                if (player.velocity.Y > player.maxFallSpeed*0.75f)
                {
                    attacking = true;
                    player.immune = true;
                    player.immuneNoBlink = true; ;
                    sp.SetContactinvulnerability(3);
                }
                else
                {
                    attacking = false;
                }
            }
        }

    }
}
