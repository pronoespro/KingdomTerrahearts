using KingdomTerrahearts.Items.Weapons.Bases;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;


namespace KingdomTerrahearts.Items.Weapons.Org13.Larxene
{
    public class Knives_Orage : ThrowingKnivesBase
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Orage");
            Tooltip.SetDefault("A weapon that embodies speed");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.autoReuse = true;
            Item.damage = 20;
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = 0.5f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Yellow;
            Item.scale = 1f;
            Item.shootSpeed = 25;
            Item.UseSound = SoundID.Item19;
            Item.value = 2000;

            knivesToThrow = 3;
            thrownManaUsage = 5;
            meleeDamage = 5;
            thrownDamage = 7;
            meleeSpeed = 25;
            thrownSpeed = 20;
            spread = 0.25f;
            maxCombo = 3;
            manaSteal = 5;

            gravityScale = 1.5f;
            pierceAmmount = 1;
            collisionProjectile = ProjectileID.HarpyFeather;


            tpProjectile = ProjectileID.HarpyFeather;
            tpProjVelMult = 0.5f;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Knives_Trancheuse>())
                .AddIngredient(ItemID.Feather,15)
                .AddTile(TileID.Anvils).Register();
        }

    }
}
