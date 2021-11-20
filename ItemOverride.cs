using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using KingdomTerrahearts.Items.Weapons;
using KingdomTerrahearts.Extra;

namespace KingdomTerrahearts
{
    class ItemOverride : GlobalItem
    {

        public override bool? UseItem(Item Item, Player player)
        {
            switch (Item.type)
            {
                case ItemID.FallenStar:

                    player.AddBuff(ModContent.BuffType<Buffs.EnlightenedBuff>(), 30*60);

                    break;
            }

            return base.UseItem(Item, player);
        }

        public override bool CanUseItem(Item Item, Player player)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            switch (Item.type)
            {
                case ItemID.WormFood:
                    if (sp.fightingInBattleground)
                        return true;
                    break;
            }

            return base.CanUseItem(Item, player);
        }

        public override void GrabRange(Item Item, Player player, ref int grabRange)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            if (sp.invincible)
            {
                grabRange *= 500;
            }
        }

        public override bool GrabStyle(Item Item, Player player)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            if (sp.invincible && player.CanAcceptItemIntoInventory(Item))
            {
                Item.velocity = (MathHelp.Magnitude(player.Center - Item.Center) <= 30) ?  Vector2.Zero: (MathHelp.Normalize(player.Center - Item.Center) * 30);
                if (MathHelp.Magnitude(player.Center - Item.Center) <= 45)
                    Item.Center = player.Center;
                return false;
            }
            return base.GrabStyle(Item, player);
        }

        public override void UpdateInventory(Item Item, Player player)
        {

            switch (Item.type)
            {
                case ItemID.Keybrand:
                    Item.useAnimation = Item.useTime = 10;
                    Item.damage = (player.HasBuff(ModContent.BuffType<Buffs.EnlightenedBuff>())) ? 550 : 250;
                    Item.shoot = ProjectileID.MagicMissile;
                    break;
            }
        }

        public override bool ConsumeItem(Item Item, Player player)
        {
            return !player.GetModPlayer<SoraPlayer>().invincible && base.ConsumeItem(Item, player);
        }

        public override bool CanBeConsumedAsAmmo(Item ammo, Player player)
        {
            return !player.GetModPlayer<SoraPlayer>().invincible && base.CanBeConsumedAsAmmo(ammo, player);
        }

        public override void ModifyWeaponCrit(Item Item, Player player, ref int crit)
        {
            crit = player.GetModPlayer<SoraPlayer>().invincible ? 10000 : crit;
        }

        public override void ModifyWeaponKnockback(Item Item, Player player, ref StatModifier knockback, ref float flat)
        {
            StatModifier knock = new StatModifier(10000000);
            knockback = player.GetModPlayer<SoraPlayer>().invincible ? knock : knockback;
        }

        public override float UseTimeMultiplier(Item Item, Player player)
        {
            if (Item.pick>0 || Item.axe>0 || Item.hammer>0 || Item.consumable) 
            {
                return base.UseTimeMultiplier(Item, player);
            }
            return (player.GetModPlayer<SoraPlayer>().invincible ? 0.05f : 1);
        }

    }
}
