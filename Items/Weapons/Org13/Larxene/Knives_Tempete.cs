using KingdomTerrahearts.Items.Weapons.Bases;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Org13.Larxene
{
    public class Knives_Tempete : ThrowingKnivesBase
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Tempete");
            Tooltip.SetDefault("Tempestuous fighting is the way");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.autoReuse = true;
            Item.damage = 35;
            Item.useTime = Item.useAnimation = 25;
            Item.knockBack = 0.1f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Yellow;
            Item.scale = 0.5f;
            Item.shootSpeed = 25;
            Item.UseSound = SoundID.Item19;
            Item.value = 2000;

            knivesToThrow = 3;
            thrownManaUsage = 15;
            meleeDamage = 25;
            thrownDamage = 40;
            meleeSpeed = 20;
            thrownSpeed = 20;
            spread = 0.35f;
            maxCombo = 3;

            gravityScale = 1.5f;
            pierceAmmount = 2;
            collisionProjectile = ProjectileID.Electrosphere;
            projectileTimeLeft = 30;
            projectileVelMult = 0f;
            manaSteal = 10;

            dashDir = new Vector2(-15, 15);

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Knives_Tourbillon>())
                .AddIngredient(ModContent.ItemType<Knives_Trancheuse>(),2)
                .AddIngredient(ModContent.ItemType<Materials.lightningShard>(), 10)
                .AddTile(TileID.Anvils).Register();
        }

    }
}
