// https://youtu.be/NL1zhckb5hc
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLproj.Game;

namespace SFMLproj
{

    public class ClickerGame : GameLoop
    {
        public const uint DefaultWindowWidth = 1120;
        public const uint DefaultWindowHeight = 900;
        public const string DefaultWindowTitle = "Clicker Game";

        public Dictionary<List<Building>, int> ClickCombinations { get; } = new Dictionary<List<Building>, int>();
        private Queue<Building> clickCombinationQueue = new Queue<Building>();
        public int MaxClickCombinationSize = 3;
        public int ClickCombinationBonus = 100;

        private Map map;

        private Random rand;

        private List<Building> buildings;

        private Building laboratory;
        private Building experimental;
        private Building research;
        private Building innovative;
        private Building scientific;
        private Building technological;
        private Building futuristic;

        private bool isMouseClicked = false;
        private bool isMouseReleased = false;

        private Building selectedBuilding;

        private float timeBeforeEventEnd;

        private List<CombinationsText> combinationTextList = new List<CombinationsText>();

        public ClickerGame() : base (DefaultWindowWidth, DefaultWindowHeight, DefaultWindowTitle, Color.Black)
        {
        }

        public override void LoadContent()
        {
            TextFont.LoadContent();
            map = new Map();
            laboratory = new Building(BuildingTypes.Laboratory);
            experimental = new Building(BuildingTypes.Experimental);
            research = new Building(BuildingTypes.Research);
            innovative = new Building(BuildingTypes.Innovative);
            scientific = new Building(BuildingTypes.Scientific);
            technological = new Building(BuildingTypes.Technological);
            futuristic = new Building(BuildingTypes.Futuristic);
            buildings = new List<Building>
            {
                laboratory, experimental, research, innovative, scientific, technological, futuristic
            };
        }

        public override void Initialize()
        {
            rand = new Random();
            laboratory.isBuildingOwned = true;
            /*Score.Money = 9999;
            laboratory.AutoClickEnabled = true;
            experimental.isBuildingOwned = true;*/
            ClickCombinations.Add(new List<Building> { laboratory, research, experimental }, 800);
            ClickCombinations.Add(new List<Building> { experimental, laboratory, laboratory }, 350);
            ClickCombinations.Add(new List<Building> { research, experimental, experimental }, 1500);
            ClickCombinations.Add(new List<Building> { scientific, innovative, research }, 10000);
            ClickCombinations.Add(new List<Building> { innovative, experimental, innovative }, 5000);
            ClickCombinations.Add(new List<Building> { futuristic, futuristic, laboratory }, 100000);
        }

        public override void Update(GameTime gameTime)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (!isMouseClicked)
                {
                    isMouseClicked = true;
                    isMouseReleased = false;
                    Vector2i mousePosition = Mouse.GetPosition(Window);
                    CheckBuildingClick(mousePosition, gameTime);
                }
            }
            else
            {
                if (isMouseClicked)
                {
                    isMouseClicked = false;
                    isMouseReleased = true;
                }
            }
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                Vector2i mousePosition = Mouse.GetPosition(Window);
                if (selectedBuilding == null)
                {
                    selectedBuilding = CheckBuildingRightClick(mousePosition);
                }
                else if (!selectedBuilding.ContainsPoint(Window.MapPixelToCoords(mousePosition)))
                {
                    selectedBuilding = null;
                }
            }
            rand = new Random(Convert.ToInt32(gameTime.TotalTimeElapsed));
            if (timeBeforeEventEnd > 0 && timeBeforeEventEnd - gameTime.deltaTime < 0)
            {
                timeBeforeEventEnd = 0;
                Score.MoneyFactor = 1;
                Score.moneyFactorActive = false;
            }
            else if (timeBeforeEventEnd >= 0)
            {
                timeBeforeEventEnd -= gameTime.deltaTime;
                Score.TotalTimeBeforeEventEnd = timeBeforeEventEnd;
            }
            foreach (Building building in buildings)
            {
                    building.Update(gameTime);
            }
            if (clickCombinationQueue.Count >= MaxClickCombinationSize)
            {
                bool combinationFound = false;
                foreach (var entry in ClickCombinations)
                {
                    if (entry.Key.SequenceEqual(clickCombinationQueue.ToList()))
                    {
                        Building building = clickCombinationQueue.ToList()[2];
                        int bonus = entry.Value;
                        Score.AddMoneyByFactor(bonus);
                        var combinationText = new CombinationsText("You performed a click combination!", building.Position);
                        combinationTextList.Add(combinationText);
                        combinationFound = true;
                        break;
                    }
                }

                if (combinationFound)
                {
                    clickCombinationQueue.Clear();
                }
            }
            CheckClickCombination();
            foreach (var combinationText in combinationTextList)
            {
                combinationText.Update(gameTime.deltaTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            map.DrawData(this);
            foreach (Building building in buildings)
                building.DrawData(Window, Mouse.GetPosition(Window));
            //DebugUtility.DrawPerfomanceData(this, Color.White);
            Score.DrawData(this, Color.White);
            if (selectedBuilding != null)
            {
                selectedBuilding.DrawContextMenu(Window);
            }
            if (Score.moneyFactorActive)
                Score.DrawMoneyFactorChanged(this);
            foreach (var combinationText in combinationTextList)
            {
                combinationText.Draw(Window);
            }
            combinationTextList.RemoveAll(text => !text.IsVisible);

            string queueText = "Queue: ";
            foreach (var building in clickCombinationQueue)
            {
                queueText += building.Name + " ";
            }

            var queueDisplayText = new Text(queueText, TextFont.font, 20)
            {
                Position = new Vector2f(10, 60),
                FillColor = Color.White
            };
            Window.Draw(queueDisplayText);
        }

        private void CheckBuildingClick(Vector2i mousePosition, GameTime gameTime)
        {
            // проверка на какой домик нажал игрок лкм
            Vector2f worldPosition = Window.MapPixelToCoords(mousePosition);

            if (laboratory.ContainsPoint(worldPosition) && laboratory.isBuildingOwned)
            {
                HandleBuildingClick(laboratory);
            }
            else if (experimental.ContainsPoint(worldPosition) && experimental.isBuildingOwned)
            {
                HandleBuildingClick(experimental);
            }
            else if (research.ContainsPoint(worldPosition) && research.isBuildingOwned)
            {
                HandleBuildingClick(research);
            }
            else if (innovative.ContainsPoint(worldPosition) && innovative.isBuildingOwned )
            {
                HandleBuildingClick(innovative);
            }
            else if (scientific.ContainsPoint(worldPosition) && scientific.isBuildingOwned)
            {
                HandleBuildingClick(scientific);
            }
            else if (technological.ContainsPoint(worldPosition) && technological.isBuildingOwned)
            {
                HandleBuildingClick(technological);
            }
            else if (futuristic.ContainsPoint(worldPosition) && futuristic.isBuildingOwned)
            {
                HandleBuildingClick(futuristic);
            }

            if (selectedBuilding != null)
            {
                if (selectedBuilding.isBuildingOwned && selectedBuilding.upgradeButtonSprite.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y))
                {
                    if (Score.Money >= selectedBuilding.UpgradeCost)
                    {
                        Score.Money -= selectedBuilding.UpgradeCost;
                        selectedBuilding.Upgraded();
                    }
                }
                if (selectedBuilding.isBuildingOwned && selectedBuilding.autoClickButtonSprite.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y))
                {
                    if (Score.Money >= selectedBuilding.autoClickerCost)
                    {
                        Score.Money -= selectedBuilding.autoClickerCost;
                        selectedBuilding.AutoClickEnabled = true;
                    }
                }
                else if (!selectedBuilding.isBuildingOwned && selectedBuilding.buyButtonSprite.GetGlobalBounds().Contains(mousePosition.X, mousePosition.Y))
                {
                    if (Score.Money >= selectedBuilding.baseCost)
                    {
                        Score.Money -= selectedBuilding.baseCost;
                        selectedBuilding.isBuildingOwned = true;
                    }
                }
            }
            /*foreach (Building building in clickCombinationQueue)
                Console.WriteLine(building.Name);*/
        }
        private Building CheckBuildingRightClick(Vector2i mousePosition)
        {
            Vector2f worldPosition = Window.MapPixelToCoords(mousePosition);
            Building clickedBuilding = null;

            if (laboratory.ContainsPoint(worldPosition))
            {
                clickedBuilding = laboratory;
            }
            else if (experimental.ContainsPoint(worldPosition))
            {
                clickedBuilding = experimental;
            }
            else if (research.ContainsPoint(worldPosition))
            {
                clickedBuilding = research;
            }
            else if (innovative.ContainsPoint(worldPosition))
            {
                clickedBuilding = innovative;
            }
            else if (scientific.ContainsPoint(worldPosition))
            {
                clickedBuilding = scientific;
            }
            else if (technological.ContainsPoint(worldPosition))
            {
                clickedBuilding = technological;
            }
            else if (futuristic.ContainsPoint(worldPosition))
            {
                clickedBuilding = futuristic;
            }
            return clickedBuilding;
        }

        // когда ивент закончился, сбрасываем множитель и считаем шансы на новый ивент
        public void ActiveEvent()
        {
            if (timeBeforeEventEnd <= 0)
            {
                Score.MoneyFactor = 1;
                Score.moneyFactorActive = false;
                var random = rand.Next(0, 100);

                if (random >= 0 && random <= 3)
                {
                    timeBeforeEventEnd = 10;
                    Score.MoneyFactor = 2;
                    Score.moneyFactorActive = true;
                    Console.WriteLine("x2 money");
                }
                else if (random >= 4 && random <= 6)
                {
                    timeBeforeEventEnd = 20;
                    Score.MoneyFactor = 0.5f;
                    Score.moneyFactorActive = true;
                    Console.WriteLine("x0.5 money");
                }
            }
        }

        private void CheckClickCombination()
        {
            if (clickCombinationQueue.Count >= MaxClickCombinationSize)
            {
                List<Building> combination = clickCombinationQueue.ToList();
                bool combinationFound = false;

                foreach (var entry in ClickCombinations)
                {
                    if (entry.Key.SequenceEqual(combination))
                    {
                        Building building = combination[2];
                        int bonus = entry.Value;
                        Score.AddMoneyByFactor(bonus);
                        var combinationText = new CombinationsText("You performed a click combination!", building.Position);
                        combinationTextList.Add(combinationText);
                        combinationFound = true;
                        break;
                    }
                }

                if (!combinationFound)
                {
                    clickCombinationQueue.Clear();
                }
            }
        }
        private void HandleBuildingClick(Building building)
        {
            clickCombinationQueue.Enqueue(building);
            CheckClickCombination();
            ActiveEvent();
            Score.AddMoneyByFactor(building.perClick);
            Score.Clicks++;
        }
    }
}
