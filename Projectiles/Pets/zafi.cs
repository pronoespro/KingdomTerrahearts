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
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 8;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.scale = 2;
			Projectile.light = 2;
		}

        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			SoraPlayer sora = player.GetModPlayer<SoraPlayer>();
			if (!player.active)
			{
				Projectile.active = false;
				return;
			}

            if (player.dead)
				sora.hasZafi = false;
            if (sora.hasZafi)
				Projectile.timeLeft = 2;

			Vector2 distToPlayer = player.Center - Projectile.Center;

			if (MathHelp.Magnitude(distToPlayer) > tpDist * 4 && MathHelp.Magnitude(player.velocity) == 0)
			{
				if (Projectile.alpha < 255)
				{
					Projectile.alpha = Math.Min(255, Projectile.alpha + 5);
					Projectile.Center = player.Center;
					//Projectile.frame = 0;
				}
				else
				{
					if (MathHelp.Magnitude(player.velocity) == 0)
					{
						Projectile.velocity = Vector2.Zero;
						Projectile.Center = player.Center;
					}
				}
			}
			else
			{
				if (!falling)
				{
					if (Math.Abs(distToPlayer.Y) > jumpMax || Math.Abs(distToPlayer.X) > tpDist)
					{
						if (Projectile.alpha < 255)
						{
							Projectile.alpha = Math.Min(255, Projectile.alpha + 5);
							Projectile.velocity = Vector2.Zero;
							//Projectile.frame = 0;
						}
						else
						{
							if (player.velocity.Y == 0)
							{
								Projectile.Center = player.Center;
								Projectile.velocity = Vector2.Zero;
							}
						}
					}
					else
					{
						if (Projectile.alpha > 0)
						{
							Projectile.alpha = Math.Max(0, Projectile.alpha - 5);
							//Projectile.frame = 0;
						}
						else
						{
							Projectile.alpha = 0;
							if (distToPlayer.Y < -jump * 2)
							{
								falling = true;
								Projectile.velocity.Y = -jump;
								//Projectile.frame = 3;
							}
							else if (distToPlayer.Y > jump * 2)
							{
								falling = true;
								Projectile.velocity.Y = jump;
								//Projectile.frame = 4;
							}
							else
							{
								desX = (desX == 0) ? Main.rand.Next(-10, 10) : desX;
								Projectile.ai[0]++;
								if (Projectile.ai[0] > 200)
								{
									Projectile.ai[0] = Main.rand.Next(0, 50);
									desX = Main.rand.Next(-70, 70);
								}
								if (Math.Abs(distToPlayer.X) > runSpeed)
								{
									Projectile.spriteDirection = (distToPlayer.X > 0) ? 1 : -1;
									Projectile.ai[1] = (Projectile.ai[1] >= 45) ? 0 : Projectile.ai[1]++;
									//Projectile.frame = (int)(Projectile.ai[1] / 15);
									Projectile.velocity.X = Projectile.spriteDirection * runSpeed;
								}
								if (Math.Abs(distToPlayer.X + desX) > walkSpeed * 1.5f)
								{
									Projectile.spriteDirection = (distToPlayer.X + desX > 0) ? 1 : -1;
									Projectile.ai[1] = (Projectile.ai[1] >= 39) ? 0 : Projectile.ai[1]+1;

									Projectile.frame = 3;
									if (Projectile.ai[1] < 30)
										Projectile.frame = 0;
									if (Projectile.ai[1]<20)
										Projectile.frame = 2;
									if (Projectile.ai[1] < 10)
										Projectile.frame = 0;

									Projectile.velocity.X = ((distToPlayer.X + desX) / Math.Abs(distToPlayer.X + desX)) * walkSpeed;
								}
								else
								{
									Projectile.spriteDirection = (distToPlayer.X > 0) ? 1 : -1;
									Projectile.ai[1] = (Projectile.ai[1] >= 13) ? 0 : Projectile.ai[1]+1;
									Projectile.frame = (int)(Projectile.ai[1] / 7);

									Projectile.velocity = Vector2.Zero;
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
					Projectile.frame = (Projectile.velocity.Y > 2) ? 5 : (Projectile.velocity.Y<-2)?4:0;
					Projectile.velocity.Y = (Projectile.velocity.Y > maxFallSpeed) ? maxFallSpeed : Projectile.velocity.Y+fallSpeed;
					Projectile.velocity.X /= 2;
					if (Projectile.Center == lastPos && Projectile.velocity.Y>0)
						falling = false;
				}
			}

			lastPos = Projectile.Center;
		}

    }
}
