using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public abstract class Keyblade : ModItem
    {

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("No help available yet");
		}

		public enum keyType
		{
			light,
			fire,
			dark,
			jungle,
			digital,
			destiny
		}

		public enum KeyComboType
		{
			normal,
			magic,
			stabbing,
			dual
		}

		public enum keyTransform
		{
			none
		}

		public enum keyDriveForms
		{
			none,
			valor,
			wisdom,
			master,
			final,
			limit,
			anti,
			second,
			strike,
			element,
			guardian,
			blitz,
			rage,
			ultimate
		}

		public int combo = 0;
        public int comboMax = 3;
		public int projectileTime = 10;
		public int lastUsedTime = 0;
		public int extraShootTimes = 0;
		public keyType keybladeElement=keyType.light;
		public KeyComboType keyComboType = KeyComboType.normal;
		public bool canShootAgain = true;
		public int manaConsumed = 0;

		Player wielder;

		public override bool CanUseItem(Player player)
		{
			item.mana = 0;
			for (int i = 0; i < 1000; ++i)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
				{
					if (!canShootAgain)
						return false;
					Main.projectile[i].timeLeft = projectileTime;
				}
			}


			wielder = player;

			switch (combo)
			{
				case 0:
					item.shoot = -1;
					item.useStyle = 1;
					break;
				case 1:
					item.shoot = -1;
					item.useStyle = 3;
					break;
				case 2:
					ShootmagicProjectile();
					break;
				case 3:
					item.shoot = -1;
					item.useStyle = 3;
					break;
				case 4:
					ComboPlus(combo);
					break;
				case 5:
					ComboPlus(combo);
					break;
			}

			combo++;
			if (combo >= comboMax)
			{
				combo = 0;
			}

			return base.CanUseItem(player);
		}

		void ComboPlus(int comboMoment)
		{
			switch (keyComboType) {
				case KeyComboType.normal:
					switch (comboMoment)
					{
						case 4:
							item.shoot = -1;
							item.useStyle = 1;
							break;
						case 5:
							item.shoot = -1;
							item.useStyle = 3;
							break;
					}
				break;
				case KeyComboType.magic:
					switch (comboMoment)
					{
						case 4:
							ShootmagicProjectile();
							break;
						case 5:
							item.shoot = -1;
							item.useStyle = 3;
							break;
					}
					break;
				case KeyComboType.dual:
					switch (comboMoment)
					{
						case 4:
							ShootmagicProjectile();
							break;
						case 5:
							item.shoot = -1;
							item.useStyle = 3;
							break;
					}
					break;
				case KeyComboType.stabbing:

					switch (comboMoment)
					{
						case 4:
							item.shoot = -1;
							item.useStyle = 3;
							break;
						case 5:
							item.shoot = -1;
							item.useStyle = 3;
							break;
					}
					break;
			}

		}

		public override bool UseItem(Player player)
		{
			lastUsedTime = 0;
			return base.UseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += Vector2.Normalize(new Vector2(speedX, speedY))/2;
			for (int i = 0; i < extraShootTimes; i++)
			{
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, item.damage, item.knockBack);
			}

			return true;
		}

		public void ShootmagicProjectile()
		{

			if(manaConsumed>0)
			{
				if (wielder.statMana >= manaConsumed)
				{
					item.mana = 20;
					wielder.statMana -= manaConsumed;
				}
				else
				{
					item.shoot = -1;
					item.useStyle = 3;
					return;
				}
			}

			switch (keybladeElement)
			{
				case keyType.fire:
					item.shoot = ProjectileID.Flamelash;
					item.shootSpeed = 3;
					Main.projectile[item.shoot].timeLeft = projectileTime;
					break;
				case keyType.dark:
					item.shoot = ProjectileID.DemonScythe;
					item.shootSpeed = 1;
					break;
				case keyType.light:
					item.shoot = ProjectileID.AmethystBolt;
					item.shootSpeed = 4;
					break;
				case keyType.jungle:
					item.shoot = ProjectileID.SporeCloud;
					item.shootSpeed = 1;
					break;
				case keyType.digital:
					item.shoot = mod.ProjectileType("tronDisk");
					item.shootSpeed = 7;
					break;
				case keyType.destiny:
					item.shoot = mod.ProjectileType("teleportThrownKey");
					item.shootSpeed = 15;
					break;
			}
			item.useStyle = 5;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Insert(1,new TooltipLine(mod,"transformation","This Keyblade can't transform itself or you"));
		}

		public override void UpdateInventory(Player player)
		{

			if (!(player.inventory[player.selectedItem] == item))
			{
				combo = 0;
				lastUsedTime = 0;
			}
			else
			{
				lastUsedTime++;
				if (lastUsedTime > item.useTime * 1.5f && combo>0)
				{
					combo = 0;
					lastUsedTime = 0;
				}
			}
		}

	}
}
