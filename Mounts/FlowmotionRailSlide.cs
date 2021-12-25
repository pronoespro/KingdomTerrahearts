using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Mounts
{
    public class FlowmotionRailSlide:ModMount
	{

		public override void SetStaticDefaults()
		{
			// What separates mounts and minecarts are these 3 lines
			MountData.Minecart = true;
			// This makes the minecarts item autoequip in the minecart slot
			MountID.Sets.Cart[ModContent.MountType<FlowmotionRailSlide>()] = true;
			// The specified method takes care of spawning dust when stopping or jumping. Use DelegateMethods.Minecart.Sparks for normal sparks.
			
			//MountData.MinecartDust = GreenSparks;

			MountData.spawnDust = 16;
			//MountData.buff = ModContent.BuffType<ExampleMinecartBuff>(); // serves the same purpose as for Car.cs

			// Movement fields:
			MountData.flightTimeMax = 20; // always set flight time to 0 for minecarts
			MountData.fallDamage = 0f; // how much fall damage will the player take in the minecart
			MountData.runSpeed = 20f; // how fast can the minecart go
			MountData.acceleration = 1f; // how fast does the minecart accelerate
			MountData.jumpHeight = 75; // how far does the minecart jump
			MountData.jumpSpeed = 5.15f; // how fast does the minecart jump
			MountData.blockExtraJumps = true; // Can the player not use a could in a bottle when in the minecart?
			MountData.heightBoost = 0;

			// Drawing fields:
			MountData.playerYOffsets = new int[] { 0, 0, 0 }; // where is the players Y position on the mount for each frame of animation
			MountData.xOffset = 0; // the X offset of the minecarts sprite
			MountData.yOffset = 0; // the Y offset of the minecarts sprite
			MountData.bodyFrame = 6; // which body frame is being used from the player when the player is boarded on the minecart
			MountData.playerHeadOffset = 0; // Affects where the player head is drawn on the map

			// Animation fields: The following is the mount animation values shared by vanilla minecarts. It can be edited if you know what you are doing.
			MountData.totalFrames = 1;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 1;
			MountData.runningFrameDelay = 12;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = 0;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 0;
			MountData.inAirFrameDelay = 0;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 10;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = false;
			if (Main.netMode != NetmodeID.Server)
			{
				MountData.textureWidth = MountData.frontTexture.Width();
				MountData.textureHeight = MountData.frontTexture.Height();
			}
		}

		// This code adapted from DelegateMethods.Minecart.Sparks. Custom sparks are just and example and are not required.
		private void GreenSparks(Vector2 dustPosition)
		{
			dustPosition += new Vector2((Main.rand.Next(2) == 0) ? 13 : (-13), 0f).RotatedBy(DelegateMethods.Minecart.rotation);
			int num = Dust.NewDust(dustPosition, 1, 1, DustID.Electric, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), Alpha: 200, Scale: 0.7f);
			Main.dust[num].noGravity = true;
			Main.dust[num].fadeIn = Main.dust[num].scale + 0.25f + 0.01f * (float)Main.rand.Next(0, 51);
			Main.dust[num].velocity *= (float)Main.rand.Next(15, 51) * 0.01f;
			Main.dust[num].velocity.X *= (float)Main.rand.Next(25, 101) * 0.01f-Main.player[Main.myPlayer].velocity.X;
			Main.dust[num].velocity.Y -= (float)Main.rand.Next(15, 31) * 0.1f;
			Main.dust[num].position.Y += 22;
			if (Main.rand.Next(3) == 0)
				Main.dust[num].scale *= 0.6f;
		}

		public override void UpdateEffects(Player player)
		{
			if (Math.Abs(player.velocity.X) > 5 || Main.rand.Next(3)==0)
			{
				GreenSparks(player.Center);
            }
			MountData.bodyFrame = (Math.Abs(player.velocity.X)>5)?6:16;

			for (int i = 0; i < player.velocity.X / 5; i++)
			{
				GreenSparks(player.Center);
			}

			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
		}
    }
}
