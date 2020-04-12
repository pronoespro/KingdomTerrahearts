using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace KingdomTerrahearts
{

    class SoraPlayer:ModPlayer
    {

        public bool canGlide = false;
        public float glideTime = 0;
        public float glideFallSpeed=10;
        
        public bool canDash = false;
        public float dashSpeed=0;
        public int dashReloadSpeed = 100;
        public bool canDashMidAir = false;

        public bool canDoubleJump = false;
        public float doubleJumpHeight = 0;
        public int doubleJumpQuantity = 0;

        public int tpFallImmunity = 0;

        float curGlideTime = 0;
        bool tapped = false;
        float tapTime = 0;
        float reTapTime = 0;
        int lastPress = 0;
        bool jumped = false;
        int jumpCount = 0;
        int curDashReload = 100;

        public override void ResetEffects()
        {
            canGlide = false;
            glideTime = 0;
            glideFallSpeed = 10;

            canDash = false;
            dashSpeed = 0;
            canDashMidAir = false;

            canDoubleJump = false;
            doubleJumpHeight = 0;
            doubleJumpQuantity = 0;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {

            if (canGlide)
            {
                if (triggersSet.Jump && curGlideTime > 0 && player.velocity.Y > glideFallSpeed)
                {
                    curGlideTime--;
                    player.velocity.Y = (player.velocity.Y > glideFallSpeed) ? glideFallSpeed : player.velocity.Y;
                    player.slowFall = true;
                    player.noFallDmg = true;
                }
            }

            if (canDash && player.dash<=0 && curDashReload<=0)
            {
                if (triggersSet.Left || triggersSet.Right)
                {
                    int curPress= (triggersSet.Left) ? -1 : 1;
                    tapTime++;
                    if (tapped)
                    {
                        if (lastPress == curPress && (canDashMidAir ||Math.Abs(player.velocity.Y)<0.15f))
                        {
                            if (Math.Abs(player.velocity.X) < dashSpeed + player.maxRunSpeed)
                            {
                                player.velocity.X += (triggersSet.Left) ? -dashSpeed : dashSpeed;
                                curDashReload = dashReloadSpeed;
                            }
                        }
                        tapped = false;
                    }
                    if (tapTime < 15)
                    {
                        lastPress = curPress;
                    }
                    else
                    {
                        lastPress = 0;
                    }
                }
                else
                {
                    if (lastPress!=0)
                    {
                        tapTime = 0;
                        reTapTime++;
                        tapped = (reTapTime < 15);
                    }
                    else
                    {
                        lastPress = 0;
                        tapTime = 0;
                        reTapTime = 0;
                    }
                }
            }

            if(canDoubleJump && doubleJumpHeight > 0)
            {

                if (player.velocity.Y == 0)
                {
                    jumped = false;
                    jumpCount = 0;
                }
                else
                {

                    if (triggersSet.Jump && player.wingTime<=0)
                    {
                        int initJump = 0;
                        int extradoubleJumps = 0;
                        if (player.doubleJumpBlizzard)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpCloud)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpFart)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpSail)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpSandstorm)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }
                        if (player.doubleJumpUnicorn)
                        {
                            extradoubleJumps++;
                            initJump++;
                        }

                        if (jumped && jumpCount<doubleJumpQuantity+extradoubleJumps)
                        {
                            player.velocity.Y = (jumpCount >= initJump ) ?-doubleJumpHeight:player.velocity.Y;
                            jumpCount++;
                        }

                        jumped = false;
                    }
                    else
                    {
                        jumped = true;
                    }
                }

            }

        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            items.RemoveAt(0);

            Item item = new Item();
            item.SetDefaults(mod.ItemType("Keyblade"));
            item.stack = 1;
            item.Prefix(-1);
            items.Insert(0,item);
        }

        public override void PreUpdate()
        {
            curDashReload--;
            curDashReload = (curDashReload > dashReloadSpeed) ? dashReloadSpeed : curDashReload;
            if (player.velocity.Y == 0)
            {
                curGlideTime = glideTime;
            }
            player.noFallDmg = tpFallImmunity>0;
            tpFallImmunity-=(tpFallImmunity>0)?1:0;
        }

        public void ChangeGlideFallSpeed(float fallspeed)
        {
            glideFallSpeed = (glideFallSpeed > fallspeed) ? fallspeed : glideFallSpeed;
        }

        public void ChangeDashReload(int reload)
        {
            dashReloadSpeed = (dashReloadSpeed < reload) ? reload : dashReloadSpeed;
        }

    }
}
