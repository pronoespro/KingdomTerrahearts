using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles.Pets
{
    public class zafi:ModProjectile
	{

		public float walkSpeed = 1;
		public float runSpeed = 5;
		public float jump=5;

		public float jumpMax=75;
		public float xDifToWalk=100;
		public float desX=0;
		public float tpDist = 175;

		public float maxFallSpeed = 3;
		public float fallSpeed = 0.25f;

		public Vector2 lastPos;
		public bool falling;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost dog");
			Main.projFrames[projectile.type] = 6;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.LightPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 8;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.scale = 2;
			projectile.light = 2;
		}

        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SoraPlayer sora = player.GetModPlayer<SoraPlayer>();
			if (!player.active)
			{
				projectile.active = false;
				return;
			}
            if (player.dead)
				sora.hasZafi = false;
            if (sora.hasZafi)
				projectile.timeLeft = 2;

			Vector2 distToPlayer = player.Center - projectile.Center;

			if (MathHelp.Magnitude(distToPlayer) > tpDist * 4 && MathHelp.Magnitude(player.velocity) == 0)
			{
				if (projectile.alpha < 255)
				{
					projectile.alpha = Math.Min(255, projectile.alpha + 5);
					projectile.Center = player.Center;
					//projectile.frame = 0;
				}
				else
				{
					if (MathHelp.Magnitude(player.velocity) == 0)
					{
						projectile.velocity = Vector2.Zero;
						projectile.Center = player.Center;
					}
				}
			}
			else
			{
				if (!falling)
				{
					if (Math.Abs(distToPlayer.Y) > jumpMax || Math.Abs(distToPlayer.X) > tpDist)
					{
						if (projectile.alpha < 255)
						{
							projectile.alpha = Math.Min(255, projectile.alpha + 5);
							projectile.velocity = Vector2.Zero;
							//projectile.frame = 0;
						}
						else
						{
							if (player.velocity.Y == 0)
							{
								projectile.Center = player.Center;
								projectile.velocity = Vector2.Zero;
							}
						}
					}
					else
					{
						if (projectile.alpha > 0)
						{
							projectile.alpha = Math.Max(0, projectile.alpha - 5);
							//projectile.frame = 0;
						}
						else
						{
							projectile.alpha = 0;
							if (distToPlayer.Y < -jump * 2)
							{
								falling = true;
								projectile.velocity.Y = -jump;
								//projectile.frame = 3;
							}
							else if (distToPlayer.Y > jump * 2)
							{
								falling = true;
								projectile.velocity.Y = jump;
								//projectile.frame = 4;
							}
							else
							{
								desX = (desX == 0) ? Main.rand.Next(-10, 10) : desX;
								projectile.ai[0]++;
								if (projectile.ai[0] > 200)
								{
									projectile.ai[0] = Main.rand.Next(0, 50);
									desX = Main.rand.Next(-70, 70);
								}
								if (Math.Abs(distToPlayer.X) > runSpeed)
								{
									projectile.spriteDirection = (distToPlayer.X > 0) ? 1 : -1;
									projectile.ai[1] = (projectile.ai[1] >= 45) ? 0 : projectile.ai[1]++;
									//projectile.frame = (int)(projectile.ai[1] / 15);
									projectile.velocity.X = projectile.spriteDirection * runSpeed;
								}
								if (Math.Abs(distToPlayer.X + desX) > walkSpeed * 1.5f)
								{
									projectile.spriteDirection = (distToPlayer.X + desX > 0) ? 1 : -1;
									projectile.ai[1] = (projectile.ai[1] >= 39) ? 0 : projectile.ai[1]+1;

									projectile.frame = 3;
									if (projectile.ai[1] < 30)
										projectile.frame = 0;
									if (projectile.ai[1]<20)
										projectile.frame = 2;
									if (projectile.ai[1] < 10)
										projectile.frame = 0;

									projectile.velocity.X = ((distToPlayer.X + desX) / Math.Abs(distToPlayer.X + desX)) * walkSpeed;
								}
								else
								{
									projectile.spriteDirection = (distToPlayer.X > 0) ? 1 : -1;
									projectile.ai[1] = (projectile.ai[1] >= 13) ? 0 : projectile.ai[1]+1;
									projectile.frame = (int)(projectile.ai[1] / 7);

									projectile.velocity = Vector2.Zero;
									if (player.statLife < player.statLifeMax / 2)
									{
										player.AddBuff(BuffID.Lovestruck, 3);
										player.statLife++;
                                    }
								}

							}

						}
					}
				}
				else
				{
					projectile.frame = (projectile.velocity.Y > 2) ? 5 : (projectile.velocity.Y<-2)?4:0;
					projectile.velocity.Y = (projectile.velocity.Y > maxFallSpeed) ? maxFallSpeed : projectile.velocity.Y+fallSpeed;
					projectile.velocity.X /= 2;
					if (projectile.Center == lastPos && projectile.velocity.Y>0)
						falling = false;
				}
			}

			lastPos = projectile.Center;
		}

    }
}
