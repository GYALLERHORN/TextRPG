using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;
using static Text_rpg.InGame;

internal class Text_rpg
{
    public class InGame
    {
        private static Player player; // PlayerStatus라는 형식(클래스)의 status변수 선언

        class WelcomeScene
        {
            static void SetStatus()
            {
                player = new Player("None", "전사", 1, 100, 10, 5, 1500);
            }

            static void Main(string[] args)
            {
                SetStatus();

                Console.Write("우선, 이름을 입력하세요 : ");
                player.Name = Console.ReadLine();
                Console.WriteLine("소개글");
                Console.WriteLine("시작하려면 아무 키나 누르세요. 0을 누르면 종료합니다.");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.KeyChar == '0')
                {
                    Console.WriteLine("\n게임을 종료합니다.");
                }
                else
                {
                    Console.Clear();
                    PrimaryScene.PrimaryChoice();
                }
            }
        }

        public class PrimaryScene
        {
            static SecondaryScene secondaryScene = new SecondaryScene();
            static InputCheckMachine inputCheck = new InputCheckMachine();
            public static void PrimaryChoice()
            {
                Console.Clear();
                Console.WriteLine("첫 화면입니다.");
                Console.WriteLine("던전 입장 전 상태를 확인하고, 정비하세요. 선택지의 번호를 입력하면 해당 기능이 실행됩니다.");
                Console.WriteLine("\t[1] : 내 현재 상태");
                Console.WriteLine("\t[2] : 내 장비");
                Console.WriteLine("\t[3] : 아직 구현 안한 상점");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("\t[0] : 게임 종료\n");
                Console.WriteLine("숫자를 입력하세요.(0~3) : ");

                int input = inputCheck.CheckAvailable(0, 3);
                switch (input)
                {
                    case 0:
                        Console.WriteLine("게임을 종료합니다.");
                        break;
                    case 1:
                        secondaryScene.ShowStatus();
                        break;
                    case 2:
                        secondaryScene.ShowInventory();
                        break;
                    case 3:
                        secondaryScene.ShowStore();
                        break;
                }

            }
        }

        public class InputCheckMachine
        {
            public int CheckAvailable(int min, int max)
            {
                while (true)
                {
                    string input = Console.ReadLine();
                    bool IsNumber = int.TryParse(input, out int thisIsNumber);
                    if (IsNumber)
                    {
                        if (thisIsNumber >= min && thisIsNumber <= max)
                        {
                            return thisIsNumber;
                        }
                    }
                    Console.WriteLine("올바른 숫자를 입력하세요");
                }
            }

        }

        public class SecondaryScene
        {
            public void ShowStatus()
            {
                Console.Clear();
                Console.WriteLine("현재 상태입니다.");
                Console.WriteLine($"NAME  : {player.Name}");
                Console.WriteLine($"NAME  : {player.Job}");
                Console.WriteLine($"LEVEL : {player.Level}");
                Console.WriteLine($"HP    : {player.Hp}");
                string showATK = player.isATKBuffed ? player.buffedPlayerATK : player.Atk.ToString();
                Console.WriteLine($"ATK   : {showATK}");
                string showDEF = player.isDEFBuffed ? player.buffedPlayerDEF : player.Def.ToString();
                Console.WriteLine($"DEF   : {showDEF}");
                Console.WriteLine($"GOLD  : {player.Gold}");
                Console.WriteLine("=========================");
                Console.Write("첫 화면으로 나가려면 0을 입력하세요 : ");

                InputCheckMachine inputCheck = new InputCheckMachine();
                int input = inputCheck.CheckAvailable(0, 0);
                if (input == 0)
                {
                    PrimaryScene.PrimaryChoice();
                }
            }

            PlayerInventory inventory = new PlayerInventory();
            bool isFirstInvenEntry = true;
            public void ShowInventory()
            {
                if (isFirstInvenEntry)
                {
                    Item item1 = new Item("    ", "직검", "ATK +2", "평범한 양손 직검입니다.");
                    inventory.Additem(item1);
                    Item item2 = new Item("    ", "가죽갑옷", "DEF +3", "가죽제 갑옷입니다.");
                    inventory.Additem(item2);
                    isFirstInvenEntry = false;
                }
                inventory.DisplayInventory();
            }

            Store store = new Store();
            bool isFirstStoreEntry = true;
            public void ShowStore()
            {
                Console.WriteLine("상점을 차릴겁니다.\n\n");
                if (isFirstStoreEntry)
                {
                    Product product1 = new Product(" ", "아주 무거운 갑옷", "ATK -80", "지금은 못입어요.", 1000);
                    store.AddProduct(product1);
                    Product product2 = new Product(" ", "파랗게 빛나는 단검", "ATK +999", "뾰족해져요.", 10);
                    store.AddProduct(product2);
                    isFirstStoreEntry = false;
                }
                store.DisplayProduct();
            }

        }

        public class Player
        {
            public string Name { get; set; }
            public string Job { get; set; }
            public int Level { get; }
            public int Hp { get; }
            public int Atk { get; set; }
            public int Def { get; set; }
            public int Gold { get; set; }

            public PlayerInventory myInventory;

            public Player(string name, string job, int level, int hp, int atk, int def, int gold)
            {
                Name = name;
                Job = job;
                Level = level;
                Hp = hp;
                Atk = atk;
                Def = def;
                Gold = gold;
            }
            public void Equip(Item item)
            {
                string[] itemStatus = item.Effect.Split(" ");
                int buff = int.Parse(itemStatus[1]);

                switch (itemStatus[0])
                {
                    case "ATK":
                        player.Atk += buff;
                        ManageBuff(itemStatus);
                        break;
                    case "DEF":
                        player.Def += buff;
                        ManageBuff(itemStatus);
                        break;
                }
                item.IsEquipped = true;
                item.EquippedMark = "[E]";
            }
            public void Unequip(Item item)
            {
                string[] itemStatus = item.Effect.Split(" ");
                int buff = int.Parse(itemStatus[1]);

                switch (itemStatus[0])
                {
                    case "ATK":
                        player.Atk -= buff;
                        isATKBuffed = false;
                        break;
                    case "DEF":
                        player.Def -= buff;
                        isDEFBuffed = false;
                        break;
                }
                item.IsEquipped = false;
                item.EquippedMark = "[U]";
            }

            public bool isATKBuffed = false;
            public bool isDEFBuffed = false;

            public string buffedPlayerATK;
            public string buffedPlayerDEF;
            public void ManageBuff(string[] itemStatus)
            {
                switch (itemStatus[0])
                {
                    case "ATK":
                        buffedPlayerATK = $"{player.Atk} ( {itemStatus[1]} )";
                        isATKBuffed = true;
                        break;
                    case "DEF":
                        buffedPlayerDEF = $"{player.Def} ( {itemStatus[1]} )";
                        isDEFBuffed = true;
                        break;
                }
            }
            public void Buy(Product product)
            {
                PlayerInventory inventory = new PlayerInventory();
                player.Gold -= product.Cost;
                Console.WriteLine("물건을 샀습니다.");
                Thread.Sleep(300);
                Item boughtItem = new Item(product.EquippedMark, product.ProductName, product.Effect, product.Description);
                inventory.Additem(boughtItem);
            }
        }

        public class Item
        {
            public bool IsEquipped { get; set; }
            public string EquippedMark { get; set; }
            public string ItemName { get; set; }
            public string Effect { get; set; }
            public string Description { get; set; }

            public Item(string equippedMark, string itemName, string effect, string description)
            {
                EquippedMark = equippedMark;
                ItemName = itemName;
                Effect = effect;
                Description = description;
            }

            PlayerInventory inventory = new PlayerInventory();
            public void GetItem(Item item)
            {
                inventory.Additem(item);
            }
        }

        public class PlayerInventory
        {
            private List<Item> items = new List<Item>();

            public List<Item> Items { get; }

            public PlayerInventory()
            {
                items = new List<Item>();
            }

            public void Additem(Item item)
            {
                Console.WriteLine($"{item}을 샀습니다.");
                Thread.Sleep(6000);
                items.Add(item);
            }

            InputCheckMachine inputCheck = new InputCheckMachine();
            public void DisplayInventory()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("소유 중인 아이템 목록입니다.");
                    Console.WriteLine("아이템 번호 |    아이템명    | [E/U] | 장착효과 | 세부설명");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    int itemNumber = 1;
                    foreach (Item item in items)
                    {
                        item.EquippedMark = item.IsEquipped ? "[E]" : "[U]";
                        Console.WriteLine($"#{string.Format("{0:        ######00}", itemNumber++)} : {item.ItemName.PadLeft(10)} |{item.EquippedMark.PadLeft(4)} | {item.Effect.PadLeft(5)} | {item.Description}");
                    }
                    Console.WriteLine("=========================================================");
                    Console.Write("숫자 입력으로 장착할 아이템을 선택하세요\n[E/U]는 장착/미장착 상태를 나타냅니다.\n" +
                        "이미 장착 중인 아이템을 재입력하면 장착 해제됩니다.\n 0 입력 시 첫 화면으로 돌아갑니다. : ");

                    int input = inputCheck.CheckAvailable(0, items.Count);
                    if (input == 0)
                    {
                        PrimaryScene.PrimaryChoice();
                        break;
                    }
                    else
                    {
                        Item selecteditem = items[input - 1];
                        if (!selecteditem.IsEquipped)
                        {
                            player.Equip(selecteditem);
                        }
                        else
                        {
                            player.Unequip(selecteditem);
                        }
                    } 
                }
                
            }
        }

        public class Product
        {
            public bool IsEquipped { get; set; }
            public string EquippedMark { get; set; }
            public string ProductName { get; set; }
            public string Effect { get; set; }
            public string Description { get; set; }
            public int Cost { get; set; }


            public Product(string equippedMark, string productName, string effect, string description, int cost)
            {
                EquippedMark = equippedMark;
                ProductName = productName;
                Effect = effect;
                Description = description;
                Cost = cost;
            }

            Store store = new Store();
            public void GetProduct(Product product)
            {
                store.AddProduct(product);
            }
        }

        public class Store
        {
            public List<Product> Products { get; set; }

            public Store()
            {
                Products = new List<Product>();
            }

            public void AddProduct(Product product)
            {
                Products.Add(product);
            }

            public void RemoveProduct(Product product)
            {
                Products.Remove(product);
            }

            public void DisplayProduct()
            {
                Console.WriteLine("상점입니다.");

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("판매 중인 아이템 목록입니다.");
                    Console.WriteLine("아이템 번호 |    아이템명    | [E/U] | 장착효과 | 가격");
                    Console.WriteLine("-------------------------------------------------------------------------");

                    int productNumber = 1;
                    foreach (Product product in Products)
                    {
                        Console.WriteLine($"#{string.Format("{0:        ######00}", productNumber++)} : {product.ProductName.PadLeft(10)} |{product.EquippedMark.PadLeft(4)} | {product.Effect.PadLeft(5)} | {product.Cost}");
                    }
                    Console.WriteLine("=========================================================");
                    Console.Write("숫자 입력으로 구입할 아이템을 선택하세요\n 0 입력 시 첫 화면으로 돌아갑니다. : ");
                    InputCheckMachine inputCheck = new InputCheckMachine();

                    int input = inputCheck.CheckAvailable(0, Products.Count);

                    if (input == 0)
                    {
                        PrimaryScene.PrimaryChoice();
                        break;
                    }
                    else
                    {
                        Product selectedProduct = Products[input - 1];
                        player.Buy(selectedProduct);
                        Products.Remove(selectedProduct);
                    }
                }


            }
        }
    }
}