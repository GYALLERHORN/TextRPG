using System;
using System.Threading.Channels;

internal class Text_rpg
{
    public class Introduction
    {
        private static PlayerStatus status;
        private static PlayerInventory inventory;


        class WelcomeScene
        {
            static void SetStatus()
            {
                status = new PlayerStatus("BLENDER", 1, 10, 5, 3);
            }
            static void SetInventory()
            {

            }

            static void Main(string[] args)
            {
                SetStatus();
                PrimaryScene primaryScene = new PrimaryScene();

                Console.Write("우선, 이름을 입력하세요 : ");
                status.Name = Console.ReadLine();
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
                    primaryScene.PrimaryChoice();
                }
            }
        }

        public class PrimaryScene
        {
            public void PrimaryChoice()
            {
                SecondaryScene secondaryScene = new SecondaryScene();

                Console.WriteLine("던전 입장 전 상태를 확인하고, 정비하세요. 선택지의 번호를 입력하면 해당 기능이 실행됩니다.");
                Console.WriteLine("\t[1] : 내 현재 상태");
                Console.WriteLine("\t[2] : 내 장비");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("\t[0] : 게임 종료\n");


                Console.WriteLine("숫자를 입력하세요.(0~2) : ");
                int input = CheckAvailable(0, 2);
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
                }

                static int CheckAvailable(int min, int max)
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
        }

        public class SecondaryScene
        {
            public void ShowStatus()
            {
                Console.Clear();
                Console.WriteLine("현재 상태입니다.");
                Console.WriteLine($"NAME  : {status.Name}");
                Console.WriteLine($"LEVEL : {status.Level}");
                Console.WriteLine($"HP    : {status.Hp}");
                Console.WriteLine($"Atk   : {status.Atk}");
                Console.WriteLine($"DEF   : {status.Def}");
            }

            PlayerInventory inventory = new PlayerInventory();

            public void ShowInventory()
            {
                Item weapon1 = new Item("나뭇가지".PadLeft(20), 3, "굵은 나뭇가지입니다.");
                Item armor1 = new Item("가죽갑옷".PadLeft(20), 5, "ㅇㅇ알돌ㄴㄴㄴ 갑옷입니다.");
                inventory.additem(weapon1);
                inventory.additem(armor1);

                Console.WriteLine("소유 중인 아이템 목록입니다.");
                inventory.DisplayInventory();
            }

            

        }

        public class PlayerStatus
        {
            public string Name { get; set; }
            public int Level { get; }
            public int Hp { get; }
            public int Atk { get; }
            public int Def { get; }

            public PlayerStatus(string name, int level, int hp, int atk, int def)
            {
                Name = name;
                Level = level;
                Hp = hp;
                Atk = atk;
                Def = def;
            }
        }

        public class Item
        {
            public string ItemName { get; set; }
            public int Effect { get; set; }
            public string Description { get; set; }

            public Item(string itemName, int effect, string description)
            {
                ItemName = itemName;
                Effect = effect;
                Description = description;
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

            public void additem(Item item)
            {
                items.Add(item);
            }

            public void DisplayInventory()
            {
                foreach (Item item in items)
                {
                    Console.WriteLine($"{item.ItemName}, {item.Effect}, {item.Description}");
                }
            }
        }

    }
}