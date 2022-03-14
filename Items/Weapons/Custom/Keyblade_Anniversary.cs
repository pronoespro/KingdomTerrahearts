using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Custom
{
	public class Keyblade_Anniversary : KeybladeBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vicennial Trace");
			Tooltip.SetDefault("Happy 20th anniversary Kingdom Hearts!" +
				"\nDesign made by leytrx" +
				"\ntwitter:@MasterLeytrx");
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 35;
			Item.width = 80;
			Item.height = 80;
			Item.scale = 1f;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.holdStyle = 4;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;

			SaveAtributes();
			keyLevel = 2;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			transSprites = new string[] { "Items/Weapons/Custom/Keyblade_Anniversary" };
			formChanges = new keyDriveForm[] { keyDriveForm.second };
			animationTimes = new int[] { 15, 10 };
			shootProjectile = ModContent.ProjectileType<Projectiles.VicennialProjectile>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ModContent.ItemType<Keyblade_Kingdom>())
			.AddIngredient(ItemID.GoldBar,2)
			.AddIngredient(ItemID.Obsidian,10)
			.AddTile(TileID.Anvils)
			.Register();
		}

		public override void ChangeKeybladeValues()
		{
			magic = keyMagic.slow;
			keybladeElement = keyType.honey;
			comboMax = 5;
			projectileTime = 10000;
			keyTransformations = new keyTransformation[] { keyTransformation.none };
			transSprites = new string[] { "Items/Weapons/Custom/Keyblade_Anniversary" };
			formChanges = new keyDriveForm[] { keyDriveForm.guardian};
			animationTimes = new int[] { 15, 10 };
			shootProjectile = ModContent.ProjectileType<Projectiles.VicennialProjectile>();
			shootSpeed = 10;
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			target.AddBuff(BuffID.Midas, 120*4);
		}

    }
}
