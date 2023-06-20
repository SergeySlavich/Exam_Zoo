using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Exam_Zoo
{
    //Создаем делегат
    public delegate void FeedDelegate();

    //Базовый класс животные
    public abstract class Animal
    {
        public delegate void FeedDelegate();
        protected static int count;
        static Animal()
        {
            count = 0;
        }
        public Animal()
        {
            this._id = count;
            count++;
        }
        protected int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        abstract public void Feed();
        abstract public void Voice();
    }

    //Дочерний класс Лев
    class Lion : Animal, IComparable
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        double _weight;
        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        double _height;
        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public Lion() : base() { }
        public Lion(string name, double weight, double height) : base()
        {
            Name = name;
            Weight = weight;
            Height = height;
        }

        public override void Feed()
        {
            Console.WriteLine(this.ToString() + " want eat meat.");
        }
        public override void Voice()
        {
            Console.WriteLine(this + " рычит.");
        }
        public override string ToString()
        {
            return "Lion " + this.Name;
        }
        public int CompareTo(object obj)
        {
            if (obj is Lion)
            {
                return Name.CompareTo((obj as Lion).Name);
            }
            throw new NotImplementedException();
        }
    }
    
    //Дочерний класс Слон
    class Elephant : Animal, IComparable
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        double _weight;
        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }
        double _height;
        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public Elephant() : base() { }
        public Elephant(string name, double weight, double height) : base()
        {
            Name = name;
            Weight = weight;
            Height = height;
        }

        public override void Feed()
        {
            Console.WriteLine(this.ToString() + " want eat herb.");
        }
        public override void Voice()
        {
            Console.WriteLine(this + " кричит.");
        }
        public override string ToString()
        {
            return "Elephant " + this.Name;
        }
        public int CompareTo(object obj)
        {
            if (obj is Elephant)
            {
                return Name.CompareTo((obj as Elephant).Name);
            }
            throw new NotImplementedException();
        }
    }

    //Базовый класс Обслуживающий персонал
    partial class Service_managers
    {
        public event FeedDelegate FeedEvent;
        public virtual void Feeding()
        {
            Console.WriteLine("Сотрудник кормит животных. => Employer feeding animals.");
            if (FeedEvent != null)
            {
                FeedEvent();
            }
        }
        public override string ToString()
        {
            return "Сотрудник зоопарка " + this.Name + " " + this.LastName + " родился " + this.BDay.ToString();
        }
        protected void salary_log(in double k, params int[] transaction)
        {
            string fileLog = "log.txt";
            try
            {
                using (FileStream fs = new FileStream(fileLog, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    foreach (int i in transaction)
                    {
                        Console.Write("Совершен платеж на сумму " + k * i);
                        Console.WriteLine(DateTime.Now);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Ошибка! Запись в лог-файл невозможна.");
            }
        }
        public Service_managers(string name, string lastName, DateTime bDay)
        {
            Name = name;
            LastName = lastName;
            BDay = bDay;
        }
        public Service_managers() : this("Имя не указано", "Фамилия не указана", DateTime.Now) { }
    }

    //Дочерний класс Сотрудник зоопарка
    class Keeper : Service_managers
    {
        double KPI;
        public Keeper() : base() 
        {
            KPI = 1.0;
        }
        public Keeper(string name, string lastName, DateTime bDay, double k) : base(name, lastName, bDay)
        {
            this.KPI = k;
        }
        public override void Feeding()
        {
            base.Feeding();
            Console.WriteLine(this.ToString() + " кормит животных.");
            salary_log(this.KPI, 100);
        }
    }

    //Дочерний класс Старший сотрудник зоопарка
    class SeniorKeeper : Service_managers
    {
        double KPI;
        public SeniorKeeper() : base() { }
        public SeniorKeeper(string name, string lastName, DateTime bDay, double k) : base(name, lastName, bDay)
        {
            this.KPI = k;
        }
        public override void Feeding()
        {
            base.Feeding();
            Console.WriteLine(this.ToString() + " ответственно кормит животных.");
            salary_log(this.KPI, 100);
        }
    }
    internal class Program
    {
        
        static int size = 3;
        public class Zoo : IEnumerable
        {
            public delegate void FeedDelegate();
            public Animal[] animals = new Animal[size];

            protected DateTime _time_to_feed;
            public DateTime TimeToFeed
            {
                get { return _time_to_feed; }
                set { _time_to_feed = value; }
            }
            
            public List<Service_managers> employes = new List<Service_managers>();
            public IEnumerator GetEnumerator()
            {
                return animals.GetEnumerator();
            }
            public Animal this[int index]
            {
                get
                {
                    if (index >= 0 && index < animals.Length)
                    {
                        return animals[index];
                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    animals[index] = value;
                }
            }
            public void ToFile()
            {
                XmlSerializer xmlFormat = new XmlSerializer(typeof(Zoo));
                string fileSave = "fileSave.xml";
                try
                {
                    using (FileStream fs = new FileStream(fileSave, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        xmlFormat.Serialize(fs, this);
                    }
                    Console.WriteLine("Сериализация прошла успешно.");
                }
                catch(Exception e)
                {
                    Console.WriteLine("Ошибка! Запись в файл невозможна.");
                }
            }
        }
        static void Main(string[] args)
        {
            FeedDelegate feedDelegate = null;

            //Создаем зоопарк:
            Zoo zoo = new Zoo();
            zoo.TimeToFeed = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 0, 0);

            //Нанимаем персонал:
            Service_managers Myhalich = new Keeper("Михал-Михалыч", "Иванов", new DateTime(1960, 05,05), 1.1);
            zoo.employes.Add(Myhalich);
            Service_managers Ivanich = new Keeper("Иван-Иваныч", "Петров", new DateTime(1965, 04,04), 1.0);
            zoo.employes.Add(Ivanich);
            Service_managers Max = new SeniorKeeper("Максим", "Максимов", new DateTime(1995, 06, 06), 1.4);
            zoo.employes.Add(Max);

            //Заполняем зоопарк животными:
            Lion alex = new Lion("Alex", 200, 120);         //Лев из Мадагаскара
            Lion king = new Lion("Simba", 250, 130);        //Лев из Короля-льва
            Elephant tha = new Elephant("Tha", 1000, 250);  //Слон из маугли
            zoo.animals[0] = alex;
            zoo.animals[1] = king;
            zoo.animals[2] = tha;

            Console.WriteLine("В зоопарке находятся следующие животные:");
            //Проверяем интерфейс IEnumerable:
            foreach (Animal animal in zoo)
            {
                Console.WriteLine(animal);
            }

            Console.WriteLine("Список сотрудников зоопарка:");
            //Проверяем персонал:
            foreach (Service_managers employer in zoo.employes)
            {
                Console.WriteLine(employer);
            }

            //Кормление животных:
            Service_managers keeper = new Service_managers();
            if (DateTime.Now > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0)
                && DateTime.Now <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0))
            {
                keeper = Max;
            }
            else if(DateTime.Now > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 0, 0)
                && DateTime.Now <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59))
            {
                keeper = Myhalich;
            }
            else
            {
                keeper = Ivanich;
            }
                if (DateTime.Now >= zoo.TimeToFeed)
            foreach (Animal animal in zoo.animals)
            {
                keeper.FeedEvent += animal.Feed;
            }
            Myhalich.Feeding();

            //Сериализация
            zoo.ToFile();            
        }
    }
}

