using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLproj
{
    public enum BuildingTypes
    {
        Laboratory,
        Experimental,
        Research,
        Innovative,
        Scientific,
        Technological,
        Futuristic
    }

    // информация которую содержат здания
    public class BuildingData
    {
        public string TexturePath { get; set; }
        public string SelectedTexturePath { get; set; }
        public string ClickedTexturePath { get; set; }
        public string Name { get; set; }
        public Vector2f Position { get; set; }
        public uint BaseCost { get; set; }
        public uint AutoClickCost { get; set; }
        public uint ClickEarnings { get; set; }
    }

    public class Building
    {
        private static readonly Dictionary<BuildingTypes, BuildingData> BuildingDataMap = new Dictionary<BuildingTypes, BuildingData>
        {
            {
                BuildingTypes.Laboratory,
                new BuildingData
                {
                    TexturePath = "sprites/buildings/Laboratory.png",
                    SelectedTexturePath = "sprites/buildings/LaboratorySelected.png",
                    Name = "Laboratory House",
                    Position = new Vector2f(504, 498),
                    BaseCost = 30,
                    AutoClickCost = 300,
                    ClickEarnings = 3
                }
            },
            {
                BuildingTypes.Experimental,
                new BuildingData
                {
                    TexturePath = "sprites/buildings/Experimental.png",
                    SelectedTexturePath = "sprites/buildings/ExperimentalSelected.png",
                    Name = "Experimental House",
                    Position = new Vector2f(222, 666),
                    BaseCost = 200,
                    AutoClickCost = 2000,
                    ClickEarnings = 28
                }
            },
            {
                BuildingTypes.Research,
                new BuildingData
                {
                    TexturePath = "sprites/buildings/Research.png",
                    SelectedTexturePath = "sprites/buildings/ResearchSelected.png",
                    Name = "Research House",
                    Position = new Vector2f(168, 378),
                    BaseCost = 3000,
                    AutoClickCost = 30000,
                    ClickEarnings = 300
                }
            },
            {
                BuildingTypes.Innovative,
                new BuildingData
                {
                    TexturePath = "sprites/buildings/Innovative.png",
                    SelectedTexturePath = "sprites/buildings/InnovativeSelected.png",
                    Name = "Innovative House",
                    Position = new Vector2f(388, 152),
                    BaseCost = 15000,
                    AutoClickCost = 150000,
                    ClickEarnings = 3000
                }
            },
            {
                BuildingTypes.Scientific,
                new BuildingData
                {
                    TexturePath = "sprites/buildings/Scientific.png",
                    SelectedTexturePath = "sprites/buildings/ScientificSelected.png",
                    Name = "Scientific House",
                    Position = new Vector2f(760, 132),
                    BaseCost = 75000,
                    AutoClickCost = 750000,
                    ClickEarnings = 15000
                }
            },
            {
                BuildingTypes.Technological,
                new BuildingData
                {
                    TexturePath = "sprites/buildings/Technological.png",
                    SelectedTexturePath = "sprites/buildings/TechnologicalSelected.png",
                    Name = "Technological House",
                    Position = new Vector2f(807, 339),
                    BaseCost = 300000,
                    AutoClickCost = 900000,
                    ClickEarnings = 60000
                }
            },
            {
                BuildingTypes.Futuristic,
                new BuildingData
                {
                    TexturePath = "sprites/buildings/Futuristic.png",
                    SelectedTexturePath = "sprites/buildings/FuturisticSelected.png",
                    Name = "Futuristic House",
                    Position = new Vector2f(801, 669),
                    BaseCost = 1500000,
                    AutoClickCost = 15000000,
                    ClickEarnings = 300000
                }
            }
        };

        public BuildingTypes BuildingType;

        public string Name;
        public bool isBuildingOwned = false;
        public uint Level = 1;

        public Vector2f Position;

        public Texture texture;
        public Texture textureSelected;
        public Sprite sprite;
        public Sprite spriteSelected;

        public uint baseCost;
        public float upgradeFactor = 1.2f;
        public uint UpgradeCost;
        public uint clickEarnings;
        public uint perClick;
        public float perClickFactor = 1.04f;
        public uint perClickNextLevel;
        public uint autoClickerCost;


        public Texture contextMenuTexture;
        public Sprite contextMenuSprite;

        // текст для меню
        public Text nameText;
        public Text costText;
        public Text upgradeCost;
        public Text moneyPerClickText;
        public Text moneyPerClick;

        public Text autoClickCost;

        public bool AutoClickEnabled = false;
        public float AutoClickInterval { get; set; } = 1f;
        private float autoClickTimer = 0f;
        private float timeSinceLastAutoClick = 0f;

        // иконка покупки домика
        public Texture buyButtonTexture;
        public Sprite buyButtonSprite;

        // иконка улучшения
        public Texture upgradeButtonTexture;
        public Sprite upgradeButtonSprite;
        
        // иконка автоклика
        public Texture autoClickButtonTexture;
        public Texture autoClickButtonOwnedTexture;
        public Sprite autoClickButtonSprite;
        public Sprite autoClickButtonOwnedSprite;

        public Building(BuildingTypes buildingType)
        {
            BuildingType = buildingType;
            LoadContent();
            UpgradeCost = Convert.ToUInt32(baseCost * Math.Pow(Level, upgradeFactor));
            perClickNextLevel = Convert.ToUInt32(clickEarnings * Math.Pow(Level + 1, perClickFactor));
        }

        public void LoadContent()
        {
            // определяем какие спрайты будут у здания, какие для него загружать
            if (BuildingDataMap.ContainsKey(BuildingType))
            {
                // сам домик и информация о нём
                BuildingData buildingData = BuildingDataMap[BuildingType];
                texture = new Texture(buildingData.TexturePath);
                textureSelected = new Texture(buildingData.SelectedTexturePath);
                sprite = new Sprite(texture);
                spriteSelected = new Sprite(textureSelected);
                Name = buildingData.Name;
                Position = buildingData.Position;

                sprite.Position = Position;
                spriteSelected.Position = Position;
                FloatRect bounds = sprite.GetGlobalBounds();

                baseCost = buildingData.BaseCost;
                clickEarnings = buildingData.ClickEarnings;
                perClick = clickEarnings;
                autoClickerCost = buildingData.AutoClickCost;

                // контекстное меню
                contextMenuTexture = new Texture("sprites/ContextMenu.png");
                contextMenuSprite = new Sprite(contextMenuTexture);
                contextMenuSprite.Position = new Vector2f(Position.X + (bounds.Width / 2) - 132, Position.Y - 106);

                buyButtonTexture = new Texture("sprites/icons/Lock.png");
                buyButtonSprite = new Sprite(buyButtonTexture);
                buyButtonSprite.Position = new Vector2f(contextMenuSprite.Position.X + 119, contextMenuSprite.Position.Y + 62);
                upgradeButtonTexture = new Texture("sprites/icons/Upgrade.png");
                upgradeButtonSprite = new Sprite(upgradeButtonTexture);
                upgradeButtonSprite.Position = new Vector2f(contextMenuSprite.Position.X + 10, contextMenuSprite.Position.Y + 34);
                autoClickButtonTexture = new Texture("sprites/icons/AutoCLick.png");
                autoClickButtonOwnedTexture = new Texture("sprites/icons/AutoCLickOwned.png");
                autoClickButtonSprite = new Sprite(autoClickButtonTexture);
                autoClickButtonSprite.Position = new Vector2f(contextMenuSprite.Position.X + 10, contextMenuSprite.Position.Y + 62);
                autoClickButtonOwnedSprite = new Sprite(autoClickButtonOwnedTexture);
                autoClickButtonOwnedSprite.Position = autoClickButtonSprite.Position;


                // текст, считается один раз
                nameText = new Text(Name, TextFont.font, 20);
                nameText.Position = new Vector2f(contextMenuSprite.Position.X + (contextMenuSprite.GetLocalBounds().Width / 2 - nameText.GetLocalBounds().Width / 2), contextMenuSprite.Position.Y + 8);
                nameText.FillColor = new Color(Color.Black);
                costText = new Text($"Cost: {baseCost}", TextFont.font, 20);
                costText.Position = new Vector2f(contextMenuSprite.Position.X + (contextMenuSprite.GetLocalBounds().Width / 2 - costText.GetLocalBounds().Width / 2), contextMenuSprite.Position.Y + 30);
                costText.FillColor = new Color(Color.Black);
                moneyPerClickText = new Text("Money Per Click:", TextFont.font, 20);
                moneyPerClickText.Position = new Vector2f(contextMenuSprite.Position.X + 43, contextMenuSprite.Position.Y + 28);
                moneyPerClickText.FillColor = new Color(Color.Black);

                autoClickCost = new Text("AutoClick Cost: " + autoClickerCost, TextFont.font, 20);
                autoClickCost.Position = new Vector2f(contextMenuSprite.Position.X + 37, contextMenuSprite.Position.Y + 64);
                autoClickCost.FillColor = new Color(Color.Black);
            }
        }

        public void Upgraded()
        {
            Level++;
            perClick = Convert.ToUInt32(clickEarnings * Math.Pow(Level, perClickFactor));
            perClickNextLevel = Convert.ToUInt32(clickEarnings * Math.Pow(Level + 1, perClickFactor));
            UpgradeCost = Convert.ToUInt32(baseCost * Math.Pow(Level, upgradeFactor));
        }

        public void Update(GameTime gameTime)
        {
            autoClickTimer += gameTime.deltaTime;

            if (autoClickTimer >= AutoClickInterval && AutoClickEnabled)
            {
                Score.AddMoneyByFactor(perClick);
                autoClickTimer = 0f;
            }
        }

        public void DrawContextMenu(RenderWindow window)
        {
            window.Draw(contextMenuSprite);

            if (isBuildingOwned)
            {
                // текст, считается много раз
                upgradeCost = new Text("Upgrade Costs: " + UpgradeCost.ToString(), TextFont.font, 20);
                upgradeCost.Position = new Vector2f(contextMenuSprite.Position.X + 8, contextMenuSprite.Position.Y + 8);
                upgradeCost.FillColor = new Color(Color.Black);
                moneyPerClick = new Text(perClick + " -> " + perClickNextLevel, TextFont.font, 20);
                moneyPerClick.Position = new Vector2f(contextMenuSprite.Position.X + 43, contextMenuSprite.Position.Y + 47);
                moneyPerClick.FillColor = new Color(Color.Black);

                window.Draw(upgradeCost);
                window.Draw(moneyPerClickText);
                window.Draw(moneyPerClick);
                window.Draw(upgradeButtonSprite);
                if (AutoClickEnabled)
                    window.Draw(autoClickButtonOwnedSprite);
                else
                {
                    window.Draw(autoClickButtonSprite);
                    window.Draw(autoClickCost);
                }
            }
            else
            {
                window.Draw(nameText);
                window.Draw(costText);
                window.Draw(buyButtonSprite);
            }
        }
        public bool ContainsPoint(Vector2f point)
        {
            FloatRect bounds = sprite.GetGlobalBounds();
            return bounds.Contains(point.X, point.Y);
        }

        public void DrawData(RenderWindow window, Vector2i mousePos)
        {
            if (sprite.GetGlobalBounds().Contains(mousePos.X, mousePos.Y))
                window.Draw(spriteSelected);
            else
                window.Draw(sprite);
        }
    }
}
