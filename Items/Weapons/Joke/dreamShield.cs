using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Joke
{
    public class dreamShield: shieldBase
	{

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dream Shield");
            Tooltip.SetDefault("The power of the guardian\nKindness to aid friends\nA shield to repel all");
			base.SetStaticDefaults();
		}

        public override void SetDefaults()
		{
			base.SetDefaults();

			Item.DamageType = DamageClass.Melee;
			Item.width = 26;
			Item.height = 32;
			Item.knockBack = 3;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useAnimation = Item.useTime = 20;

			Item.noUseGraphic = true;
			Item.channel = true;
			Item.autoReuse = true;

			Item.shootSpeed = 0;
			Item.shoot = ModContent.ProjectileType<dreamShield_guard>();
			guardingProj = ModContent.ProjectileType<dreamShield_guard>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.FallenStar)
			.AddTile(TileID.WorkBenches)
			.Register();
		}

    }

	public class dreamShield_guard : shieldGuardProjectile
	{
        public override void SetStaticDefaults()
        {
			Main.projFrames[Projectile.type] = 5;
		}

        public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.timeLeft = 25;
			Projectile.hide = true;
		}

    }
}
