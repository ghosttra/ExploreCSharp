using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploreCSharp.HWs
{
    class Person
    {
        public Person(string name, decimal сash)
        {
            Name = name;
            Cash = сash;
        }
        public override string ToString()
        {
            return $"{Name} // Cash: {Cash}";
        }
        public string Name { get; set; }
        public decimal Cash { get; set; }
    }
    class CreditCard
    {
        public event Action<decimal> CardAccount_ValueChanged;
        public event Action<decimal> CardLimit_ValueAchived;
        public event Action<decimal> CreditLimit_ValueOverflow;
        public event Action<decimal> Pin_ValueChanged;
        Person person;
        public CreditCard(Person person)
        {
            this.person = person;
        }

        Random random = new Random();
        public string NumberCard { get; set; }

        public DateTime ExpirationDate { get; set; }
        public int Pin { get; set; }
        public decimal CreditLimit { get; set; } //кредитний ліміт
        public decimal CreditAccount { get; set; } //кредитний рахунок
        public decimal CardAccount { get; set; } //особовий рахунок

        public void Create(ref Person person)
        {
            Console.Write("Credit Limit: ");
            CreditLimit = Convert.ToDecimal(Console.ReadLine().ToString());
            Console.Write("Credit Account: ");
            CreditAccount = Convert.ToDecimal(Console.ReadLine().ToString());
            Console.Write("Card Account: ");
            CardAccount = Convert.ToDecimal(Console.ReadLine().ToString());
            NumberCard = random.Next(10000000, 99999999).ToString() + random.Next(10000000, 99999999).ToString();
            Console.WriteLine($"Number card = {NumberCard}");
            Pin = random.Next(1000, 9999);
            Console.WriteLine($"Your PIN is {Pin}, if you don't like it, then you can change it in your personal cabinet!");
            ExpirationDate = DateTime.Now;
            ExpirationDate = ExpirationDate.AddYears(4);
        }
        public override string ToString()
        {
            return $"NUMBER CARD: {NumberCard}\nPIN: {Pin}\nExpiration date: {ExpirationDate.Date}\nCredit limit: {CreditLimit}\nCredit account: {CreditAccount}\nCard account: {CardAccount}";
        }
        public void ChangePIN()
        {
            Console.Clear();
            Console.Write("Enter your previous PIN:");
            int PrevPIN = Convert.ToInt32(Console.ReadLine().ToString());
            if (PrevPIN == Pin)
            {
                Console.Write("Enter your new PIN: ");
                PrevPIN = Convert.ToInt32(Console.ReadLine().ToString());
                Pin_ValueChanged?.Invoke(PrevPIN);
                Pin = PrevPIN;
            }
        }
        public void topUp()
        {
            Console.Clear();
            Console.Write("Enter amount of your cash to top your card account up:");
            decimal cash = Convert.ToDecimal(Console.ReadLine().ToString());
            if (cash < person.Cash)
            {
                CardAccount_ValueChanged?.Invoke(cash);
                person.Cash -= cash;
                CardAccount += cash;
            }
            else
            {
                Console.WriteLine("Not enough cash");
            }
        }
        public void withdraw()
        {
            Console.Clear();
            Console.Write("Enter amount of your card's currency units to withdraw:");
            decimal CurrUnits = Convert.ToDecimal(Console.ReadLine().ToString());
            if (CurrUnits <= CreditAccount + CardAccount)
            {
                if (CurrUnits >= CreditLimit)
                {
                    CreditLimit_ValueOverflow?.Invoke(CreditLimit);
                    if (CurrUnits <= CardAccount)
                    {
                        CardAccount_ValueChanged?.Invoke(-CurrUnits);
                        CardAccount -= CreditLimit;
                        person.Cash += CreditLimit;
                    }
                    else
                    {
                        person.Cash += CreditLimit;
                        CardAccount = 0;
                        CreditAccount -= CreditLimit;
                        CardLimit_ValueAchived?.Invoke(CreditAccount);
                    }
                }
                    
                else
                {
                    CreditLimit -= CurrUnits;
                    if (CurrUnits >= CardAccount)
                    {
                        person.Cash += CurrUnits;
                        CurrUnits -= CardAccount;
                        CardAccount = 0;
                        CreditAccount -= CurrUnits;
                        CardLimit_ValueAchived?.Invoke(CreditAccount);
                    }
                    else if (CurrUnits < CardAccount)
                    {
                        CardAccount_ValueChanged?.Invoke(-CurrUnits);
                        CardAccount -= CurrUnits;
                        person.Cash += CurrUnits;
                    }
                }
            }
            else
            {
                Console.WriteLine("Not enough currency units");
            }
        }
    }
    class BankApplication
    {
        Person person;
        CreditCard creditCard;
        public void CreditCard_Pin_ValueChanged(decimal obj)
        {
            Console.WriteLine($"The credit card of user: {person.Name} | {creditCard.NumberCard}, was rePINned for {obj}");
        }
        void CreditCard_CardAccount_ValueChanged(decimal value)
        {
            if (value > 0)
                Console.WriteLine($"The bank account of user: {person.Name} | {creditCard.NumberCard}, was replenished for {value} currency units!");
            else
                Console.WriteLine($"From the bank account of user: {person.Name} | {creditCard.NumberCard}, was removed {value} currency units!");
        }
        private void CreditCard_CreditLimit_ValueOverflow(decimal obj)
        {
            Console.WriteLine($"Achieving the limit of the specified amount of money {obj} curr. units | CCN: {creditCard.NumberCard}");
        }
        private void CreditCard_CardLimit_ValueAchived(decimal obj)
        {
            Console.WriteLine($"Start using credit curr. units: {obj} | CCN: {creditCard.NumberCard}");
        }
        public void mainmenu()
        {
            while (true)
            {
                //Console.Clear();
                switch (CharpUniqLib.menu.Menu.SelectVertical("Create credit card", "Check credit Card", "Change PIN", "Top your account up", "Withdraw money from the account", "Exit"))
                {
                    case 0:
                        Console.Clear();
                        Console.Write("Enter your name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter your cash: ");
                        decimal cash = Convert.ToDecimal(Console.ReadLine().ToString());
                        person = new Person(name, cash);
                        creditCard = new CreditCard(person);
                        creditCard.Pin_ValueChanged += CreditCard_Pin_ValueChanged;
                        creditCard.CardAccount_ValueChanged += CreditCard_CardAccount_ValueChanged;
                        creditCard.CreditLimit_ValueOverflow += CreditCard_CreditLimit_ValueOverflow;
                        creditCard.CardLimit_ValueAchived += CreditCard_CardLimit_ValueAchived;
                        creditCard.Create(ref person);
                        Console.ReadLine();
                        break;
                    case 1:
                        Console.Clear();
                        Console.WriteLine(creditCard);
                        Console.WriteLine("Owner: " + person);
                        Console.ReadLine();
                        break;
                    case 2:
                        creditCard.ChangePIN();
                        break;
                    case 3:
                        creditCard.topUp();
                        break;
                    case 4:
                        creditCard.withdraw();
                        break;
                    case 5:
                        return;

                }
            }
        }


    }
}
