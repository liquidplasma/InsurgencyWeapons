using InsurgencyWeapons.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;

namespace InsurgencyWeapons.Items.Other
{
    internal class MoneyDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!npc.CountsAsACritter && npc.CanBeChasedBy())
            {
                if (!npc.boss)
                {
                    int amount = Math.Clamp(npc.lifeMax / 20, 1, 20);
                    npcLoot.Add(ItemDropRule.Common(Insurgency.Money, 2, 1 + amount, 4 + amount));
                }
                else
                {
                    int amount = Math.Clamp(npc.lifeMax / 250, 1, 70);
                    npcLoot.Add(ItemDropRule.Common(Insurgency.Money, 1, 20 + amount, 70 + amount));
                }
            }
        }
    }

    internal class Money : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1000;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Lime;
            Item.maxStack = Item.CommonMaxStack;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            rotation += Item.velocity.X * 0.1f + Item.velocity.Y * 0.1f;
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Green.ToVector3() * 0.75f);
            if (Main.rand.NextBool(15))
            {
                Dust dust = Dust.NewDustDirect(Item.position, Item.width, Item.height, DustID.GreenTorch);
                dust.noLight = true;
                dust.noGravity = true;
            }
        }
    }
}