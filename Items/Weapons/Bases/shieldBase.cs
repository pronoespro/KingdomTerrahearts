using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{

    public abstract class shieldBase:ModItem
    {

		public float xDash = 5;
		public float yDash = 0;
		public int bashDamage = 25;
		public int guardingProj;

        public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
			Item.useStyle = -1;
			Item.holdStyle = -1;
            base.SetDefaults();
        }

        public override void UpdateInventory(Player player)
		{
			Item.holdStyle = -1;
			Item.noUseGraphic = player.ownedProjectileCounts[guardingProj]>0;
		}

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            base.UseItemHitbox(player, ref hitbox, ref noHitbox);
			hitbox.Y += Item.height / 2;
        }

        public override bool AltFunctionUse(Player player)
		{
			return true;
		}

        public override bool CanUseItem(Player player)
		{
            Item.useStyle = ItemUseStyleID.HoldUp;
			Item.damage = 0;

			if (player.altFunctionUse == 2)
			{

				Item.noUseGraphic = true;
				Item.channel = true;
				Item.autoReuse = true;

				Item.shootSpeed = 0;
				Item.shoot = guardingProj;
				Item.damage = 0;
				Item.useAnimation = Item.useTime = 50;

			}
			else
			{

				SoraPlayer sora = player.GetModPlayer<SoraPlayer>();
				sora.AddInvulnerability(20);

				Item.noUseGraphic = false;
				Item.channel = false;
				Item.autoReuse = false;

				Item.shootSpeed = 0;
				Item.shoot = ProjectileID.None;
				Item.damage = bashDamage;
				Item.useAnimation = Item.useTime = 15;

				int target=sora.GetClosestEnemy((int)(xDash+yDash)*Item.useTime);

				if (target == -1)
				{
					player.direction = (int)MathHelp.Sign((Main.MouseWorld - player.Center).X);
                }
                else
                {
					player.direction = (int)MathHelp.Sign((Main.npc[target].Center-player.Center).X);
                }
				player.velocity = new Vector2(player.direction * xDash, yDash);
			}

			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
			player.itemRotation = 0;
			player.itemLocation.X = player.Center.X+(player.direction);
			player.itemLocation.Y = player.Center.Y+Item.height/2;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			Vector2 mousePos = MathHelp.Normalize(Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center);

			player.itemRotation = -(float)Math.Atan2(mousePos.X, mousePos.Y) + (float)Math.PI / 2 + (int)(player.direction== -1 ? (float)Math.PI : 0);

			mousePos = MathHelp.Normalize(mousePos)*6;
			mousePos.Y /= 10f;
			player.itemLocation = player.Center + mousePos+new Vector2(0,5);
		}

		public override void HoldItemFrame(Player player)
		{

			player.bodyFrame.Y = player.bodyFrame.Height * 3;

		}

	}

	public abstract class shieldGuardProjectile:ModProjectile
	{

		public bool blockedAttack=false;
		public int projDamage = 25;
		public bool canChainGuards;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindProjectiles.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            base.ModifyDamageHitbox(ref hitbox);
			hitbox.Width = (int)(hitbox.Width * Projectile.scale);
			hitbox.Height = (int)(hitbox.Height * Projectile.scale);
        }

        public override void AI()
		{
			Projectile.ai[0]++;

			Projectile.scale = Math.Clamp(3f - (Projectile.ai[0] / 10), 1, 2f);

			Player player = Main.player[Projectile.owner];
			SoraPlayer sora = player.GetModPlayer<SoraPlayer>();

			blockedAttack = sora.blockedAttack || blockedAttack;

			if (blockedAttack)
			{
				Projectile.frame = 4;

				Projectile.damage = projDamage;
				Projectile.timeLeft = (Projectile.timeLeft > 3) ? 3 : Projectile.timeLeft;
				if (canChainGuards)
				{
					sora.guardTime = 5;
					sora.guardType = blockingType.normal;
					sora.guardProj = Projectile.whoAmI;
				}
				else
				{
					sora.guardTime = -1;
					sora.guardType = blockingType.none;
					sora.guardProj = -1;
				}
				sora.AddInvulnerability(5);
			}
			else
			{
				Projectile.frameCounter++;
				Projectile.frame = (Projectile.frameCounter / 3) % 4;
				Projectile.damage = 0;

				sora.guardTime = 5;
				sora.guardType = blockingType.normal;
				sora.guardProj = Projectile.whoAmI;
			}

			Vector2 mPos = Main.MouseWorld - player.Center;


			Projectile.spriteDirection = (int)MathHelp.Sign(mPos.X);
			Projectile.rotation = -(float)Math.Atan2(mPos.X, mPos.Y) + (float)Math.PI / 2 + (int)(Projectile.spriteDirection == -1 ? (float)Math.PI : 0);
			Projectile.Center = player.Center + MathHelp.Normalize(mPos) * 10;

			player.direction = Projectile.spriteDirection;


			if (player.dead || !player.controlUseTile)
			{
				Projectile.Kill();
				return;
			}

		}


	}

}
