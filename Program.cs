using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Security;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using static TextRpg.Program;

namespace TextRpg
{
    internal class Program
    {
        // 클래스 모음.
        public class Player // 플레이어의 정보
        {
            public string Name { get; set; } // 이름
            public string job { get; set; } // 직업
            public Dictionary<string, int> stats { get; set; } = new Dictionary<string, int>(); // 능력치를 딕셔너리로 정리
            public int Lv { get; set; } // 레벨
            public int gold { get; set; } // 소지 골드
            public void status() // 스테이터스 창 표시.
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("-캐릭터의 정보가 표시됩니다.-");
                Console.WriteLine("\n레벨 : " + Lv);
                Console.WriteLine(Name + " : " + job);
                Console.WriteLine("공격력 : " + stats["str"] + " ( " + invs.equipmentStr + " )");
                Console.WriteLine("방어력 : " + stats["dex"] + " ( " + invs.equipmentDex + " )");
                Console.WriteLine("체력 : " + stats["hp"]);
            }
        }
        public static Player player = new Player(); // 플레이어를 다른곳에서 쓸수있게 선언.
        public class Item // 아이템정보를 처리하는 클래스
        {
            public int itemid { get; set; } // 아이템의 ID 넘버
            public string ItemName { get; set; } // 아이템의 이름
            public int ItemStr { get; set; } // 아이템의 공격력
            public int ItemDex { get; set; } // 아이템의 방어력
            public string ItemLore { get; set; } // 아이템의 설명
            public int ItemGold { get; set; } // 아이템의 가격
            public bool ItemWear { get; set; } // 아이템 장착여부
            public bool ItemBuying { get; set; } // 아이템의 구매여부
            public Item(int id, string name, int str, int dex, string lore, int gold, bool wear) 
            {
                itemid = id;
                ItemName = name;
                ItemStr = str;
                ItemDex = dex;
                ItemLore = lore;
                ItemGold = gold;
                ItemWear = wear;
                ItemBuying = false;

            }
            public void ItemStatus() // 아이템의 정보를 방어력장비인지 공격장비인지 구분하여, 표기해줌.
            {
                if (ItemDex == 0)
                {
                    Console.Write("  " + ItemName + "     |     " + "공격력 + " + ItemStr + "  | " + ItemLore);
                }
                else
                {
                    Console.Write("  " + ItemName + "     |  " + "방어력 + " + ItemDex + "  | " + ItemLore);
                }
            }
        }
        static Dictionary<int, Item> items = new Dictionary<int, Item>(); // 아이템의 정보를 딕셔너리로 저장해서 사용할수있게함.
        public class inventory()
        {
            public Dictionary<int, Item> inv = new Dictionary<int, Item>(); // 인벤토리에 아이템의 정보를 저장함.
            public int equipmentStr { get; set; } // 착용한 아이템의 공격력을 저장함.
            public int equipmentDex { get; set; } // 착용한 아이템의 방어력을 저장함.
            public int invStack { get; set; } // 아이템이 내 딕셔너리에 몇개가 저장되어있는지 확인하는 장치
        }
        public static inventory invs = new inventory(); // 인벤토리를 다른곳에서 쓸수있게함.
        static void ItemSetting() // 초반 아이템과 인벤토리에 정보를 넣어줌.
        {
            invs.invStack = 0;
            invs.equipmentStr = 0;
            invs.equipmentDex = 0;
            items.Add(1, new Item(1, "수련자의 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000, false));
            items.Add(2, new Item(2, "무쇠갑옷", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000, false));
            items.Add(3, new Item(3, "스파르타의 갑옷", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, false));
            items.Add(4, new Item(4, "낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.", 600, false));
            items.Add(5, new Item(5, "청동 도끼", 5, 0, "어디선가 사용됐던거 같은 도끼입니다.", 1500, false));
            items.Add(6, new Item(6, "스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다..", 3000, false));
        }
        static void startscene() // 초반 플레이어의 정보를 설정함.
        {

            player.Lv = 1;
            player.stats.Add("str", 10);
            player.stats.Add("dex", 5);
            player.stats.Add("hp", 100);
            player.gold = 150000;
            player.job = "전사";
        startpoint:
            Console.Clear();
            Console.WriteLine("플레이어의 이름을 작성해주세요.");
            Console.Write(">>> ");
            string name = Console.ReadLine();
            Console.WriteLine(name + " 이(가) 맞습니까?");
            Console.WriteLine("1. 네 \n2. 아니요.");
            int index = int.Parse(Console.ReadLine());
            switch (index)
            {
                case 1:
                    player.Name = name;
                    break;
                case 2: goto startpoint;
                default: goto startpoint;
            }
        }
        static void MainScene() // 기본적인 마을의 인터페이스 정보를 담고있음.
        {
        mainstage:
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("\n1. 상태 보기\n2. 인벤토리\n3. 상점\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>>> ");
            int index = int.Parse(Console.ReadLine());
            switch (index)
            {
                case 1:
                r_stat:
                    player.status();
                    Console.WriteLine("0. 돌아가기");
                    Console.Write("원하시는 행동을 입력해주세요.\n>>> ");
                    int num = int.Parse(Console.ReadLine());
                    if (num != 0)
                    {
                        Console.WriteLine("올바른 번호가 아닙니다.");
                        Thread.Sleep(1000);
                        goto r_stat;
                    }
                    goto mainstage;
                case 2:
                    inventoryStatus();
                    break;
                case 3:
                    shop();
                    break;
                default: goto mainstage;
            }
        }
        static void shoptxt()//상점에서 쓰이는 텍스트 정보를 가지고있음.
        {
            Console.Clear();

            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]\n" + player.gold + " G\n");
            Console.WriteLine("[아이템목록]");
            foreach (var item in items) // 구매한 아이템의 정보를 가지고 나열해줌.
            {

                Console.Write(" - " + item.Value.itemid + " | ");
                item.Value.ItemStatus();
                if (item.Value.ItemBuying)
                {
                    Console.WriteLine("  |  [구매 완료]");
                }
                else
                {
                    Console.WriteLine("  |  " + item.Value.ItemGold + " G ");
                }
            }
        }
        static void shop() // 상점 인터페이스
        {
        reshop:
            Console.WriteLine("상점");
            shoptxt();
            Console.WriteLine("\n\n1. 아이템 구매\n0. 나가기\n");
            Console.Write("원하시는 행동을 입력해 주세요.\n>>> ");
            int index = int.Parse(Console.ReadLine());

            switch (index)
            {
                case 1:
                    buying();
                    break;
                case 0: MainScene(); break;
                default: Console.WriteLine("잘못 입력하셨습니다."); goto reshop;
            }
        }
        static void buying() // 상점에서 구매모드로 전환하는 장치.
        {
        rebuying:
            Console.WriteLine("상점 - 아이템구매");
            shoptxt();
            Console.WriteLine("\n\n구매하실 아이템 번호를 입력해주세요.\n0. 나가기\n");
            Console.Write("원하시는 행동을 입력해 주세요.\n>>> ");
            int index = int.Parse(Console.ReadLine());

            if (index == 0)
            {
                MainScene();
            }
            else if (index <= 6 || index < 0)
            {
                if (items[index].ItemBuying == false)
                {
                    int item_value = items[index].ItemGold;
                    if (player.gold >= item_value)
                    {
                        invs.invStack++;
                        Console.WriteLine("구매를 완료했습니다.");
                        player.gold -= item_value;
                        items[index].ItemBuying = true;
                        invs.inv.Add(invs.invStack, items[index]);
                        goto rebuying;
                    }
                    else
                    {
                        Console.WriteLine("Gold 가 부족합니다.");
                        goto rebuying;
                    }
                }
                else
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                    Thread.Sleep(1000);
                    goto rebuying;
                }
            }
            else
            {
                Console.WriteLine("올바른 번호를 입력해주세요.");
                Thread.Sleep(1000);
                buying();
            }
        }
        static void invtxt() // 인벤토리 텍스트 관리
        {
            Console.Clear();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("\n[아이템 목록]");
            int index = 1;
            if (invs.invStack == 0)
            {
                Console.WriteLine("아이템이 없습니다. 상점에서 구매해주세요.");
            }
            foreach (var item in invs.inv)
            {
                if (item.Value.ItemWear == true)
                {
                    Console.Write(" [E] " + index);
                }
                else
                {
                    Console.Write(" - " + index);
                }
                item.Value.ItemStatus();
                Console.WriteLine("");
                index++;
            }
        }
        static void inventoryStatus() // 인벤토리 인터페이스.
        {
        invde:
            Console.WriteLine("인벤토리");
            invtxt();
            Console.WriteLine("\n\n1. 장착관리\n0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>> ");
            int index = int.Parse(Console.ReadLine());

            switch (index)
            {
                case 0: MainScene(); break;
                case 1: equipment(); break;
                default: goto invde;
            }
        }
        static void equipment() // 인벤토리 ㅡ 장착관리
        {
        equipment:
            Console.WriteLine("인벤토리 - 장착 관리");
            invtxt();
            Console.WriteLine("\n\n장착하실 아이템의 번호를 입력하세요.\n0. 나가기");
            Console.Write("\n원하시는 행동을 입력해주세요.\n>>> ");
            int index = int.Parse(Console.ReadLine());

            if (index == 0)
            {
                MainScene();
            }
            else if (index < 0 || index <= invs.inv.Count)
            {
                if (invs.inv[index].ItemWear == false)
                {
                    invs.inv[index].ItemWear = true;
                    if (invs.inv[index].ItemDex == 0)
                    {
                        if (invs.equipmentStr == null)
                        {
                            invs.equipmentStr = 0;
                            invs.equipmentStr += invs.inv[index].ItemStr;
                        }
                        else
                        {
                            invs.equipmentStr += invs.inv[index].ItemStr;
                        }
                    }
                    else
                    {
                        if (invs.equipmentDex == null)
                        {
                            invs.equipmentDex = 0;
                            invs.equipmentDex += invs.inv[index].ItemDex;
                        }
                        else
                        {
                            invs.equipmentDex += invs.inv[index].ItemDex;
                        }
                    }
                }
                else
                {
                    invs.inv[index].ItemWear = false;
                    if (invs.inv[index].ItemDex == 0)
                    {
                        invs.equipmentStr -= invs.inv[index].ItemStr;
                    }
                    else
                    {

                        invs.equipmentDex -= invs.inv[index].ItemDex;

                    }
                }
            }
            else
            {
                Console.WriteLine("올바른 숫자를 입력해주세요.");
                goto equipment;
            }
            goto equipment;
        }
        static void Main()// 모든것을 실행시킬 메인
        {
            ItemSetting();
            startscene();
            MainScene();
        }
    }
}
