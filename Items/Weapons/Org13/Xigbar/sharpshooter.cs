using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items.Weapons.Org13.Xigbar
{
    public class sharpshooter:ModItem
    {

        int combo=0;
        int comboMax = 32;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Arrowgun");
            Tooltip.SetDefault("A weapon that shoots out dark arrows" +
                "\nConsumes 1 mana per arrow" +
                "\nHas combos" +
                "\nCan shoot up to 32 shots before needing to reload");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 17*2;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 3;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.knockBack = 3;
            Item.value = 10000;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.JestersArrow;
            Item.shootSpeed = 25;
            Item.noMelee = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override Nullable<bool> UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            if (player.altFunctionUse == 2)
            {
                combo = 0;
                return true;
            }
            else
            {
                return base.UseItem(player);
            }
        }

        public override bool CanUseItem(Player player)
        {

            combo++;
            if (combo >= comboMax)
            {
                Item.reuseDelay = 200;
                combo = 0;
            }
            else
            {
                Item.reuseDelay = 0;
            }

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.FlareGun)
            .AddIngredient(ItemID.Handgun)
            .AddIngredient(ItemID.Flare, 13)
            .AddTile(TileID.Anvils)
            .Register();
        }

    }
}
