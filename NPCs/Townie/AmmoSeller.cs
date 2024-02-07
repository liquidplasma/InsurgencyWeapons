﻿using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using InsurgencyWeapons.Items.Other;
using InsurgencyWeapons.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace InsurgencyWeapons.NPCs.Townie
{
    internal class AmmoSeller : ModNPC
    {
        private static Profiles.StackedNPCProfile NPCProfile;

        private SoundStyle Fire => new("InsurgencyWeapons/Sounds/Weapons/Ins2/aks/shoot")
        {
            Pitch = Main.rand.NextFloat(-0.1f, 0.1f),
            MaxInstances = 0,
            Volume = 0.4f
        };

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
            NPCID.Sets.PrettySafe[Type] = 300;
            NPCID.Sets.AttackType[Type] = 1; // Shoots a weapon.
            NPCID.Sets.AttackTime[Type] = 60; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
            NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

            //This sets entry is the most important part of this NPC. Since it is true, it tells the game that we want this NPC to act like a town NPC without ACTUALLY being one.
            //What that means is: the NPC will have the AI of a town NPC, will attack like a town NPC, and have a shop (or any other additional functionality if you wish) like a town NPC.
            //However, the NPC will not have their head displayed on the map, will de-spawn when no players are nearby or the world is closed, and will spawn like any other NPC.
            NPCID.Sets.ActsLikeTownNPC[Type] = true;

            // This prevents the happiness button
            NPCID.Sets.NoTownNPCHappiness[Type] = true;

            //To reiterate, since this NPC isn't technically a town NPC, we need to tell the game that we still want this NPC to have a custom/randomized name when they spawn.
            //In order to do this, we simply make this hook return true, which will make the game call the TownNPCName method when spawning the NPC to determine the NPC's name.
            NPCID.Sets.SpawnsWithCustomName[Type] = true;

            //The vanilla Bone Merchant cannot interact with doors (open or close them, specifically), but if you want your NPC to be able to interact with them despite this,
            //uncomment this line below.
            NPCID.Sets.AllowDoorInteraction[Type] = true;

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPCProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, -1),
                new Profiles.DefaultNPCProfile(Texture + "_Shimmer", -1)
            );
        }

        public override void SetDefaults()
        {
            NPC.friendly = true; // NPC Will not attack player
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 18;
            NPC.defense = 37;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override bool CanChat()
        {
            return true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Mods.InsurgencyWeapons.NPCs.AmmoSeller.DescriptionBestiary")
            });
            base.SetBestiary(database, bestiaryEntry);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            int num = NPC.life > 0 ? 1 : 5;
            for (int k = 0; k < num; k++)
            {
                Dust dusty = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Scorpion);
                dusty.color = Color.Red;
            }

            if (NPC.life <= 0)
            {
                for (int i = 0; i < 36; i++)
                {
                    Dust dusty = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Scorpion);
                    dusty.color = Color.Red;
                }
            }
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string> {
                "James",
                "John Wick",
                "JC Denton",
                "Randy"
            };
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest && spawnInfo.Player.inventory.Any(item => item.type == ModContent.ItemType<Money>()))
            {
                return 0.27f;
            }

            return 0f;
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new();

            chat.Add(Language.GetTextValue("Mods.InsurgencyWeapons.NPCs.AmmoSeller.StandardDialogue1"));
            chat.Add(Language.GetTextValue("Mods.InsurgencyWeapons.NPCs.AmmoSeller.StandardDialogue2"));
            chat.Add(Language.GetTextValue("Mods.InsurgencyWeapons.NPCs.AmmoSeller.StandardDialogue3"));
            return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = "Shop";
            }
        }

        public override void AddShops()
        {
            NPCShop AmmoShop = new(Type);
            foreach (int AmmoType in Insurgency.AmmoTypes)
            {
                Item AmmoItem = ContentSamples.ItemsByType[AmmoType];
                AmmoItem grabStack = AmmoItem.ModItem as AmmoItem;
                AmmoShop.Add(new Item(AmmoType)
                {
                    shopCustomPrice = (int?)(AmmoItem.value * 0.8f),
                    shopSpecialCurrency = InsurgencyWeapons.MoneyCurrency,
                    stack = grabStack.CraftStack
                });
            }
            AmmoShop.Register();
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 16;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 25;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<NormalBullet>();
            attackDelay = 1;

            // This code progressively delays subsequent shots.
            if (NPC.localAI[3] > attackDelay)
            {
                attackDelay = 6;
            }
            if (NPC.localAI[3] > attackDelay)
            {
                attackDelay = 8;
            }
            if (NPC.localAI[3] > attackDelay)
            {
                attackDelay = 12;
            }
            if (NPC.localAI[3] > attackDelay)
            {
                attackDelay = 14;
            }
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 10f;
            randomOffset = 0.4f;
        }

        public override void OnKill()
        {
            HelperStats.Announce(Color.Red, NPC.GivenName + "Mods.InsurgencyWeapons.NPCs.AmmoSeller.Die");
            base.OnKill();
        }
    }
}