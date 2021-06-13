using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Mounts
{
    public class mewwow:ModMountData
    {

        bool isGrounded = false;
        bool attacking = false;
        int timeOnGround = 0;

        public override void SetDefaults()
        {
            mountData.spawnDust = DustID.Confetti;
            mountData.buff = mod.BuffType("mewwowBuff");
            mountData.heightBoost = 30;
            mountData.fallDamage = 0f;
            mountData.runSpeed = 4f;
            mountData.dashSpeed = 2f;
            mountData.flightTimeMax = 0;
            mountData.fatigueMax = 0;
            mountData.jumpHeight = 10;
            mountData.acceleration = 0.19f;
            mountData.jumpSpeed = 10f;
            mountData.blockExtraJumps = false;
            mountData.totalFrames = 1;
            mountData.constantJump = true;
            int[] array = new int[mountData.totalFrames];
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = 30;
            }
            mountData.playerYOffsets = array;
            mountData.xOffset = 7;
            mountData.yOffset = 10;
            mountData.bodyFrame =0;
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
            mountData.swimFrameStart= 0;
            if (Main.netMode != NetmodeID.Server)
            {
                mountData.textureWidth = mountData.frontTexture.Width;
                mountData.textureHeight = mountData.frontTexture.Height;
            }

        }


        public override void UpdateEffects(Player player)
        {

            Point playerBottomPosition = (player.Center + new Vector2(0, player.gravDir * (float)player.height / 2f + player.gravDir * 2f)).ToTileCoordinates();
            Point movementBottomPosition = (player.Center + new Vector2(player.velocity.X, player.gravDir * (float)player.height / 2f + player.gravDir * 2f)).ToTileCoordinates();

            if (WorldGen.SolidOrSlopedTile(playerBottomPosition.X,playerBottomPosition.Y)|| WorldGen.SolidOrSlopedTile(movementBottomPosition.X, movementBottomPosition.Y))
            {

                int dustAmmount = 10;

                if (isGrounded)
                {
                    if (Math.Abs(player.velocity.X) > 0.1f)
                    {
                        Main.PlaySound(new LegacySoundStyle(2, 16), x: (int)player.Center.X, y: (int)player.Center.Y);
                        timeOnGround++;
                        if (timeOnGround > 2)
                        {
                            player.velocity.Y = -mountData.jumpHeight / 2;
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
                        Main.PlaySound(new LegacySoundStyle(2, 14), x: (int)player.Center.X, y: (int)player.Center.Y);
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
                        int newDust = Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, mountData.spawnDust = DustID.Confetti);
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
                    SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                    if (!sp.IsInvulnerable())
                    sp.AddInvulnerability(2);
                }
                else
                {
                    attacking = false;
                }
            }
        }

    }
}
