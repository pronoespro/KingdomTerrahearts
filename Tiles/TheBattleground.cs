using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace KingdomTerrahearts.Tiles
{
    public class TheBattleground :ModTile
    {

        public override void SetStaticDefaults()
        {
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            Main.tileObsidianKill[Type] = false;

            Main.tileNoAttach[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.Width = 18;
            TileObjectData.newTile.Height = 28;
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.CoordinateHeights = new int[28];
            for(int i = 0; i < TileObjectData.newTile.Height;i++)
            {
                TileObjectData.newTile.CoordinateHeights[i] = 16;
            }
            TileObjectData.newTile.Origin = new Point16(9, 27);
            TileObjectData.newTile.AnchorBottom= new AnchorData(AnchorType.SolidTile,4, TileObjectData.newTile.Width / 2 -2);
            TileObjectData.newTile.AnchorTop = AnchorData.Empty;
            TileObjectData.newTile.AnchorLeft = AnchorData.Empty;
            TileObjectData.newTile.AnchorRight = AnchorData.Empty;

            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();

            name.SetDefault("The Battleground");
            AddMapEntry(new Color(150, 30, 30), name);

            AnimationFrameHeight = 504;
            DustType = 11;
            TileID.Sets.DisableSmartCursor[Type] = true;
            MinPick = 2;
            MineResist = 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i*16,j*16,9,28,ModContent.ItemType<TheBattlegroundItem>());
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            makeDust = false;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Player player = Main.LocalPlayer;
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

            float x = i * 16;
            float y = j * 16;

            if ((player.position.X > x - (16 * 9) && player.position.X < x + (16 * 9) && player.position.Y < y - (16 * 28)) || sp.fightingInBattleground)
            {
                Vector2 init = new Vector2(x - (16 * 9), y - (16 * 29) * 2);
                Vector2 end = new Vector2(x + (16 * 9) - 9, y - (16 * 29) + 16-player.height/2);
                sp.SetTrapLimits(init,end);

                bool playerIsTrapped = false;
                for (int e = 0; e < Main.npc.Length; e++)
                {
                    if (sp.isBoss(e) && Main.npc[e].active && Main.npc[e].life > 0 )
                    {
                        playerIsTrapped = true;
                        break;
                    }
                }
                if (playerIsTrapped)
                {
                    player.AddBuff(BuffID.ChaosState, 15, false);
                    player.AddBuff(BuffID.Darkness, 15, false);
                    player.AddBuff(BuffID.Blackout, 15, false);
                    player.AddBuff(BuffID.NoBuilding, 60, false);
                    player.AddBuff(BuffID.Shine, 60, false);
                    if (!player.HasBuff(BuffID.PotionSickness))
                    {
                        player.AddBuff(BuffID.PotionSickness, 30, false);
                    }

                    if (!player.HasBuff(ModContent.BuffType<Buffs.EnlightenedBuff>()))
                    {
                        player.AddBuff(ModContent.BuffType<Buffs.EnlightenedBuff>(), 50,true);
                    }
                }
                sp.fightingInBattleground = (playerIsTrapped || sp.fightingInBattleground)&& !player.dead;
            }

        }


    }


    public class TheBattlegroundItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Battling Heart");
            Tooltip.SetDefault("A heart to call the Battlegrounds");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 13;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.rare = 1;
            Item.createTile = ModContent.TileType<Tiles.TheBattleground>();
            Item.placeStyle = 0;
            Item.consumable = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.DarkenedHeart>())
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}
