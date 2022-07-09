using KingdomTerrahearts.Items.Weapons.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Org13.Larxene
{
    public class Knives_Trancheuse: ThrowingKnivesBase
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Trancheuse");
            Tooltip.SetDefault("Larxene's most basic weapon." +
                "\nNothing really sets it apart");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.autoReuse = true;
            Item.damage = 5;
            Item.useTime = Item.useAnimation = 30;
            Item.knockBack = 0.5f;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Yellow;
            Item.scale = 0.4f;
            Item.shootSpeed = 15;
            Item.UseSound = SoundID.Item19;
            Item.value = 2000;

            knivesToThrow = 3;
            thrownManaUsage = 10;
            meleeDamage = 2;
            thrownDamage = 5;
            meleeSpeed = 25;
            thrownSpeed = 30;
            spread = 0.5f;
            maxCombo = 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.ThrowingKnife).AddTile(TileID.Anvils).Register();
        }

    }
}
