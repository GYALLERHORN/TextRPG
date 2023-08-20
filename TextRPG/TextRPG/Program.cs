using System;
using System.Threading.Channels;

internal class Text_rpg
{
    public class Introduction
    {
        private static PlayerStatus status;
        //private static PlayerInventory inventory;


        class RPGClass
        {
            static void SetStatus()
            {
                status = new PlayerStatus("BLENDER", 1, 10, 5, 3);
                //inventory = new PlayerInventory();
            }

            static void Main(string[] args)
            {
                SetStatus();
                PrimaryScene primaryScene = new PrimaryScene();
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
                        ShowStatus();
                        break;
                    case 2:
                        Console.WriteLine(input + " 선택");
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

                void ShowStatus()
                {
                    Console.WriteLine("현재 상태입니다.");
                    Console.WriteLine($"NAME : {status.Name}");
                    Console.WriteLine($"LEVEL : {status.Level}");
                    Console.WriteLine($"HP : {status.Hp}");
                    Console.WriteLine($"Atk : {status.Atk}");
                    Console.WriteLine($"DEF : {status.Def}");
                }
            }
        }

        public class PlayerStatus
        {
            //private string name = "BLENDER";
            //private int level = 0;
            //private int hp = 10;
            //private int atk = 5;
            //private int def = 5;

            public string Name { get; }
            public int Level { get; }
            public int Hp { get; }
            public int Atk { get; }
            public int Def { get;  }


            public PlayerStatus(string name, int level, int hp, int atk, int def)
            {
                Name = name;
                Level = level;
                Hp = hp;
                Atk = atk;
                Def = def;
            }

            public class PlayerInventory
            {
                public string dd;
            }

        }

    }
}