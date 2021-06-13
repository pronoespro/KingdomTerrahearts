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

        public override void HoldItem(Item item, Player player)
        {
            SoraPlayer sora=player.GetModPlayer<SoraPlayer>();
        }

        public override bool UseItem(Item item, Player player)
        {

            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

            switch (item.type)
            {
                case ItemID.FallenStar:

                    player.AddBuff(mod.BuffType("EnlightenedBuff"),30*60);

                    break;
            }

            return base.UseItem(item, player);
        }

        public override bool CanUseItem(Item item, Player player)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            switch (item.type)
            {
                case ItemID.WormFood:
                    if (sp.fightingInBattleground)
                        return true;
                    break;
            }

            return base.CanUseItem(item, player);
        }

        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            if (sp.invincible)
            {
                grabRange *= 50;
            }
        }

        public override bool GrabStyle(Item item, Player player)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            if (sp.invincible)
            {
                item.velocity = (MathHelp.Magnitude(player.Center - item.Center) <= 30) ?  Vector2.Zero: (MathHelp.Normalize(player.Center - item.Center) * 30);
                if (MathHelp.Magnitude(player.Center - item.Center) <= 45)
                    item.Center = player.Center;
                return false;
            }
            return base.GrabStyle(item, player);
        }

        public override void UpdateInventory(Item item, Player player)
        {

            switch (item.type)
            {
                case ItemID.Keybrand:
                    item.useAnimation = item.useTime = 10;
                    item.damage = (player.HasBuff(mod.BuffType("EnlightenedBuff"))) ? 500 : 150;
                    item.shoot = ProjectileID.MagicMissile;
                    break;
            }
        }

        public override bool ConsumeItem(Item item, Player player)
        {
            return player.GetModPlayer<SoraPlayer>().invincible || base.ConsumeItem(item, player);
        }

        public override bool ConsumeAmmo(Item item, Player player)
        {
            return player.GetModPlayer<SoraPlayer>().invincible || base.ConsumeAmmo(item, player);
        }

        public override void GetWeaponCrit(Item item, Player player, ref int crit)
        {
            crit = player.GetModPlayer<SoraPlayer>().invincible ? 10000 : crit;
        }

        public override void GetWeaponKnockback(Item item, Player player, ref float knockback)
        {
            knockback = (player.GetModPlayer<SoraPlayer>().invincible) ? 10000 : knockback;
        }

        public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
        {
            reforgePrice = Main.player[item.owner].GetModPlayer<SoraPlayer>().invincible ? 0 : reforgePrice;
            return base.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            return (player.GetModPlayer<SoraPlayer>().invincible ? 5f : 1);
        }

    }
}
