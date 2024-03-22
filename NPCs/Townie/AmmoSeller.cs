using InsurgencyWeapons.Helpers;
using InsurgencyWeapons.Items;
using InsurgencyWeapons.Items.Other;
using InsurgencyWeapons.Projectiles.Grenades;
using System.Collections.Generic;
using System.Linq;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Terraria.Utilities;

namespace InsurgencyWeapons.NPCs.Townie
{
    public class AmmoSeller : ModNPC
    {
        private static Profiles.StackedNPCProfile NPCProfile;

        private static float RandomDiscount;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
            NPCID.Sets.PrettySafe[Type] = 300;
            NPCID.Sets.AttackType[Type] = 0; // Shoots a weapon.
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
                Direction = -1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPCProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, -1),
                new Profiles.DefaultNPCProfile(Texture + "_Shimmer", -1)
            );
        }

        public override void SetDefaults()
        {
            NPC.friendly = true;
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

        public override void OnSpawn(IEntitySource source)
        {
            RandomDiscount = Utils.SelectRandom(Main.rand, 0.667f, 0.7f, 0.85f);
            base.OnSpawn(source);
        }

        public override bool PreAI()
        {
            if ((!Main.dayTime || Main.time >= Time.Its6PM) && !IsNpcOnscreen(NPC.Center)) // If it's past the despawn time and the NPC isn't onscreen
            {
                // Here we despawn the NPC and send a message stating that the NPC has despawned
                // LegacyMisc.35 is {0} has departed!
                if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(Language.GetTextValue("LegacyMisc.35", NPC.FullName), 50, 125, 255);
                else
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("LegacyMisc.35", NPC.GetFullNetName()), new Color(50, 125, 255));

                NPC.active = false;
                NPC.netSkip = -1;
                NPC.life = 0;
                return false;
            }
            return true;
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.Center, Color.Wheat.ToVector3() * 0.25f);
            base.AI();
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
            for (int k = 0; k < 12; k++)
            {
                Dust dusty = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Scorpion);
                dusty.color = Color.Red;
                dusty.noGravity = Main.rand.NextBool();
            }

            if (NPC.life <= 0)
            {
                for (int i = 0; i < 54; i++)
                {
                    Dust dusty = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Scorpion);
                    dusty.scale *= Main.rand.NextFloat(3f);
                    dusty.color = Color.Red;
                    dusty.noGravity = true;
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
                "Randy",
                "Duke"
            };
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool condition =
                NPC.downedBoss1 &&
                Main.dayTime &&
                !NPC.AnyNPCs(Type) &&
                spawnInfo.Player.ZoneOverworldHeight &&
                !spawnInfo.Player.ZoneCorrupt &&
                !spawnInfo.Player.ZoneCrimson &&
                spawnInfo.Player.inventory.Any(item => item.type == ModContent.ItemType<Money>());

            return condition ? 0.015f : 0f;
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new();

            for (int i = 1; i <= 4; i++)
            {
                chat.Add(Language.GetTextValue("Mods.InsurgencyWeapons.NPCs.AmmoSeller.StandardDialogue" + i));
            }

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
                AmmoItem grabStack = (AmmoItem)AmmoItem.ModItem;
                AmmoShop.Add(new Item(AmmoType)
                {
                    shopCustomPrice = grabStack.MoneyCost / grabStack.CraftStack,
                    shopSpecialCurrency = InsurgencyWeapons.MoneyCurrency,
                });
            }
            AmmoShop.Register();
        }

        public override void ModifyActiveShop(string shopName, Item[] items)
        {
            foreach (Item item in items)
            {
                if (item is null)
                    continue;

                item.shopCustomPrice = (int?)(item.shopCustomPrice * RandomDiscount);
                if (item.shopCustomPrice == 0)
                    item.shopCustomPrice = 1;
            }
            base.ModifyActiveShop(shopName, items);
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 60;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 120;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<MK2ExplosiveNPC>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 10f;
            gravityCorrection = 2f;
            randomOffset = 0.4f;
        }

        public override void OnKill()
        {
            LocalizedText deathText = Language.GetText("Mods.InsurgencyWeapons.NPCs.AmmoSeller.Die");
            HelperStats.Announce(Color.Red, NPC.GivenName + " " + deathText.Value);
            base.OnKill();
        }

        private static bool IsNpcOnscreen(Vector2 center)
        {
            int w = NPC.sWidth + NPC.safeRangeX * 2;
            int h = NPC.sHeight + NPC.safeRangeY * 2;
            Rectangle npcScreenRect = new Rectangle((int)center.X - w / 2, (int)center.Y - h / 2, w, h);
            foreach (Player player in Main.player)
            {
                // If any player is close enough to the traveling merchant, it will prevent the npc from despawning
                if (player.active && player.getRect().Intersects(npcScreenRect))
                    return true;
            }
            return false;
        }
    }
}