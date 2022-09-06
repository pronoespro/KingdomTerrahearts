using KingdomTerrahearts.Items.Weapons.Bases;
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
    public class Knives_Tourbillon : ThrowingKnivesBase
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Tourbillon");
            Tooltip.SetDefault("Whirlwinds of lightning empower you");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.autoReuse = true;
            Item.damage = 25;
            Item.useTime = Item.useAnimation = 25;
            Item.knockBack = 0.1f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Yellow;
            Item.scale = 0.5f;
            Item.shootSpeed = 25;
            Item.UseSound = SoundID.Item19;
            Item.value = 2000;

            knivesToThrow = 1;
            thrownManaUsage = 15;
            meleeDamage = 25;
            thrownDamage = 40;
            meleeSpeed = 20;
            thrownSpeed = 45;
            spread = 0f;
            maxCombo = 3;

            gravityScale = 1.5f;
            pierceAmmount = 3;
            collisionProjectile = ProjectileID.Electrosphere;
            projectileTimeLeft = 30;
            projectileVelMult = 0f;
            manaSteal = 10;

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Knives_Trancheuse>(),3)
                .AddIngredient(ItemID.IceBlock, 15)
                .AddIngredient(ModContent.ItemType<Materials.lightningShard>(),5)
                .AddTile(TileID.Anvils).Register();
        }

    }
}
