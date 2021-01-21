using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons
{
    public abstract class Keyblade : ModItem
    {

		public enum keyType
		{
			light,
			fire,
			dark,
			jungle,
			digital,
			destiny,
			star
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

		public int initDamage;
		public int combo = 0;
        public int comboMax = 3;
		public int projectileTime = 10;
		public int lastUsedTime = 0;
		public int extraShootTimes = 0;
		public keyType keybladeElement=keyType.light;
		public KeyComboType keyComboType = KeyComboType.normal;
		public bool canShootAgain = true;
		public int manaConsumed = 0;

		public bool enlightened;
		public float damageMult = 1;

		Player wielder;

		public override bool CanUseItem(Player player)
		{

			ChangeKeybladeValues();

			item.mana = 0;

			for (int i = 0; i < Main.projectile.Length; ++i)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot && Main.projectile[i].timeLeft>projectileTime)
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
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.SwingThrow;
					break;
				case 1:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
					break;
				case 2:
					ShootmagicProjectile();
					break;
				case 3:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
					break;
				case 4:
					ComboPlus(combo);
					break;
				case 5:
					ComboPlus(combo);
					break;
				default:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.SwingThrow;
					break;
			}

			combo++;
			if (combo >= comboMax)
			{
				combo = 0;
			}

			return base.CanUseItem(player);
		}

		public abstract void ChangeKeybladeValues();

		void ComboPlus(int comboMoment)
		{
			switch (keyComboType) {
				case KeyComboType.normal:
					switch (comboMoment)
					{
						case 4:
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.SwingThrow;
							break;
						case 5:
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.Stabbing;
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
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.Stabbing;
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
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.Stabbing;
							break;
					}
					break;
				case KeyComboType.stabbing:

					switch (comboMoment)
					{
						case 4:
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.Stabbing;
							break;
						case 5:
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.Stabbing;
							break;
					}
					break;
			}

		}

		public override bool UseItem(Player player)
		{
			if ((player.whoAmI == Main.myPlayer && Main.netMode==NetmodeID.MultiplayerClient)||Main.netMode==NetmodeID.SinglePlayer)
			{
				lastUsedTime = 0;
				return true;
			}
			return false;
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
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
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
				case keyType.star:
					item.shoot = ProjectileID.FallingStar;
					item.shootSpeed = 25;
					break;
			}
			item.useStyle = ItemUseStyleID.HoldingOut;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tooltip = new TooltipLine(mod, "Transformations", "This Keyblade can't transform itself or you");
			tooltip.overrideColor = Color.LightBlue;
			if (!tooltips.Contains(tooltip))
			{
				tooltips.Add(tooltip);
			}
		}

		public override void UpdateInventory(Player player)
		{

			enlightened = player.HasBuff(mod.BuffType("EnlightenedBuff"));

			item.damage = (int)(item.damage/damageMult);

			damageMult = 1;
			damageMult += (enlightened) ? 0.5f : 0;

			item.damage = (int)(item.damage * damageMult);
			item.color = (enlightened) ? Color.Blue : Color.White;

			if ((player.selectedItem>=0 && player.selectedItem<player.inventory.Length 
				&& player.inventory[player.selectedItem] == item)
				|| player.HeldItem==item)
			{
				lastUsedTime++;
				if (lastUsedTime > item.useTime * 1.5f && combo > 0)
				{
					combo = 0;
					lastUsedTime = 0;
				}

			}
			else
			{
				combo = 0;
				lastUsedTime = 0;
				item.color = Color.White;
			}
		}
	}
}
