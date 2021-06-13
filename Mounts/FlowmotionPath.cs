using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Mounts
{
    public class FlowmotionPath:ModMountData
	{

		public override void SetDefaults()
		{
			// What separates mounts and minecarts are these 3 lines
			mountData.Minecart = true;
			// This makes the minecarts item autoequip in the minecart slot
			MountID.Sets.Cart[ModContent.MountType<FlowmotionPath>()] = true;
			// The specified method takes care of spawning dust when stopping or jumping. Use DelegateMethods.Minecart.Sparks for normal sparks.
			mountData.MinecartDust = GreenSparks;

			mountData.spawnDust = 16;
			//mountData.buff = ModContent.BuffType<ExampleMinecartBuff>(); // serves the same purpose as for Car.cs

			// Movement fields:
			mountData.flightTimeMax = 20; // always set flight time to 0 for minecarts
			mountData.fallDamage = 0f; // how much fall damage will the player take in the minecart
			mountData.runSpeed = 20f; // how fast can the minecart go
			mountData.acceleration = 1f; // how fast does the minecart accelerate
			mountData.jumpHeight = 75; // how far does the minecart jump
			mountData.jumpSpeed = 5.15f; // how fast does the minecart jump
			mountData.blockExtraJumps = true; // Can the player not use a could in a bottle when in the minecart?
			mountData.heightBoost = 0;

			// Drawing fields:
			mountData.playerYOffsets = new int[] { 0, 0, 0 }; // where is the players Y position on the mount for each frame of animation
			mountData.xOffset = 0; // the X offset of the minecarts sprite
			mountData.yOffset = 0; // the Y offset of the minecarts sprite
			mountData.bodyFrame = 6; // which body frame is being used from the player when the player is boarded on the minecart
			mountData.playerHeadOffset = 0; // Affects where the player head is drawn on the map

			// Animation fields: The following is the mount animation values shared by vanilla minecarts. It can be edited if you know what you are doing.
			mountData.totalFrames = 1;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 1;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 1;
			mountData.idleFrameDelay = 10;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != NetmodeID.Server)
			{
				mountData.textureWidth = mountData.frontTexture.Width;
				mountData.textureHeight = mountData.frontTexture.Height;
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
			mountData.bodyFrame = (Math.Abs(player.velocity.X)>5)?6:16;

			for (int i = 0; i < player.velocity.X / 5; i++)
			{
				GreenSparks(player.Center);
			}

			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
			sp.formColor = Color.LightBlue;
		}
    }
}
