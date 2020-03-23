using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace KursachTest
{
    public class Dish // Объект Блюдо
    {
        public string name; // название блюда
        public int cost; // цена
        public int count; // количество
    }
    public class Order // Объект заказ
    {
        public int numberTable;
        public List<Dish> dishList;
        public double price;
    }
    public class Product // Объект продукт
    {
        public string name;
        public double count;
    }
    public class Production
    {
        public string name;
        public int count;
        public int sklad;
    }
    public class Sklad
    {
        public int number;
        public int temp;
    }
    public class DishWithComponent
    {
        public string name;
        public int price;
        public int weight;
        public string category;
        public List<Product> productList;
    }
    

    class Functions
    {
        public static Form2 f2;
        public static MainDishSlide main;
        public static DessertDishSlide dessert;
        public static DrinksDishSlide drinks;
        public static SaladDishSlide salad;
        public static SeasonSlide season;
        public static SideDishSlide side;
        public static SnackDishSlide snack;
        public static SearchDishSlide search;
        public static OrderSlide order;
        public static AddNewClient client;

        static string connString = "Server = localhost; User id = postgres;" +
                "Database = Cafe; Port = 5432; Password = dg4ao9hv; SSLMode = Prefer";
        public static NpgsqlConnection connect = new NpgsqlConnection(connString);

        public static void UpdateConnect()// обновление подключения
        {
            connect = new NpgsqlConnection(connString);
        } 

        public static void GetInformationFromBase(List<string> list, string cat) // Поиск блюд по категориям
        {
            connect.Open();
            
            string dish = "";

            NpgsqlCommand command = new NpgsqlCommand("SELECT DishName FROM public.dish WHERE " +
                "category = '" + cat + "';", connect);

            using (command)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dish = reader[0].ToString();
                    list.Add(dish);
                }
            }
            connect.Close();
        }
        public static void GetInformationFromBaseForSearch(List<string> list, string search)  // Поиск по базе
        {
            if (search.Length != 0)
            {
                connect.Open();

                string dish = "", newDish = search[0].ToString().ToUpper();
                bool find = false;

                for(int i=1; i<search.Length - 1; i++)
                {
                    newDish += search[i].ToString().ToLower();
                }  
                // Поиск блюда если первая буква заглавная
                NpgsqlCommand command = new NpgsqlCommand("SELECT DishName FROM public.dish WHERE " +
                    "dishname LIKE '%" + newDish + "%'", connect);

                using (command)
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dish = reader[0].ToString();
                        list.Add(dish);
                        find = true;
                    }
                }
                connect.Close();
                // если не нашли, то поиск с буквами в нижнем регистре
                if (!find)
                {
                    connect.Open();
                    command = new NpgsqlCommand("SELECT DishName FROM public.dish WHERE " +
                    "dishname LIKE '%" + search.ToLower() + "%'", connect);

                    using (command)
                    {
                        NpgsqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            dish = reader[0].ToString();
                            list.Add(dish);
                        }
                    }
                    connect.Close();
                }
                
            }
        }

        public static string GetDishPrice(string dish) // Запрос цены блюда
        {
            connect.Open();

            string needPrice = "";

            NpgsqlCommand command = new NpgsqlCommand("SELECT Price FROM public.dish WHERE " +
                "DishName = '" + dish + "'", connect);

            using (command)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    needPrice = reader[0].ToString();
                }
            }
            connect.Close();
            return needPrice;
        }
        public static string GetDishComponent(string dish) // Запрос состава блюда
        {
            connect.Open();

            string composite = "";
            string composite1 = "";

            NpgsqlCommand command = new NpgsqlCommand("SELECT ProductName FROM " +
                "public.dishComponent WHERE DishName = '" + dish + "'", connect);

            using (command)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    composite = composite + reader[0].ToString() + ", ";
                }
            }
            if (composite.Length > 2)
            {
                composite1 = composite.Substring(0, composite.Length - 2);
            }
            connect.Close();
            return composite1.ToLower();
        }

        /*SPAWN, REMOVE AND MOVE BUTTONS*/
        public static void spawnButtons(List<string> list, string slide)  // Динамическое добавление кнопок
        {
            int count = 0;
            int max_size = list.Count();
            int margin = 33;
            for (int i = 0; i < (max_size / 3) + 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (count < max_size)
                    {
                        Button button = new Button();
                        button.Name = "button" + (count + 3);
                        button.Text = list[count];
                        button.Size = new Size(211, 80);
                        button.Location = new Point((margin * (j + 1)) + 211 * j, 33 * (i + 1) + 80 * i);
                        button.FlatStyle = FlatStyle.Flat;
                        button.Cursor = Cursors.Hand;
                        button.Click += new EventHandler(button_Click);
                        switch (slide)
                        {
                            case "main":
                                main.Controls.Add(button);
                                if (max_size > 21) main.AutoScroll = true;
                                break;
                            case "dessert":
                                dessert.Controls.Add(button);
                                if (max_size > 21) dessert.AutoScroll = true;
                                break;
                            case "drinks":
                                drinks.Controls.Add(button);
                                if (max_size > 21) drinks.AutoScroll = true;
                                break;
                            case "salad":
                                salad.Controls.Add(button);
                                if (max_size > 21) salad.AutoScroll = true;
                                break;
                            case "season":
                                season.Controls.Add(button);
                                if (max_size > 21) season.AutoScroll = true;
                                break;
                            case "side":
                                side.Controls.Add(button);
                                if (max_size > 21) side.AutoScroll = true;
                                break;
                            case "snack":
                                snack.Controls.Add(button);
                                if (max_size > 21) snack.AutoScroll = true;
                                break;
                        }
                        count++;
                    }
                    else break;
                }
            }
        }
        public static void spawnButtons(List<string> list, int y, List<Button> buttons) // добавление кнопок в поиске
        {
            int count = 0;
            int max_size = list.Count();
            int margin = 33;
            for (int i = 0; i < (max_size / 3) + 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (count < max_size)
                    {
                        Button button = new Button();
                        button.Name = "button" + (count + 3);
                        button.Text = list[count];
                        button.Size = new Size(211, 80);
                        button.Location = new Point((margin * (j + 1)) + 211 * j, 33 * (i + 1) + 80 * i + y);
                        button.FlatStyle = FlatStyle.Flat;
                        button.Cursor = Cursors.Hand;
                        button.Click += new EventHandler(button_Click);
                        search.Controls.Add(button);
                        buttons.Add(button);
                        if (max_size > 21) main.AutoScroll = true;
                        count++;
                    }
                    else break;
                }
            }
        }
        public static void RemoveButtons(List<Button> buttons) // удаление кнопок в поиске
        {
            if (buttons.Count != 0)
            {
                for (int i = 0; i < buttons.Count; i++)
                {
                    search.Controls.Remove(buttons[i]);
                }
            }
        }
        public static void SpawnOrderButtons(List<Order> list, List<Button> btn)
        {
            int count = 0;
            int lastElem = list.Count-1;
                Button button = new Button();
                button.Name = "button" + (count + 15);
                button.Text = "Стол " + list[lastElem].numberTable.ToString();
                button.Size = new Size(110, 40);
                button.Location = new Point(220 + (lastElem) * 125, 13); //
                button.FlatStyle = FlatStyle.Flat;
                button.Cursor = Cursors.Hand;
                button.Click += new EventHandler(button_Click_Order);
                f2.Controls.Add(button);
                button.BringToFront();
                btn.Add(button);
        }
        public static void MoveOrderButtons(List<Button> list, int number) // передвижение кнопок заказов
        {
            for (int i = number; i < list.Count; i++)
            {
                int x = list[i].Location.X;
                for (int j = 0; j < 125; j++)
                {
                    list[i].Location = new Point(x - 1, 13);
                    x--;
                }
            }
        }
        /*SPAWN, REMOVE AND MOVE BUTTONS---END*/

        public static void ContainOrderSlide(List<Order> orderList, string num) // наполнение слайда заказа
        {
            string numberOrder = "";
            for(int i=5; i< num.Length; i++)
            {
                numberOrder += num[i];
            }
            foreach (ListViewItem item in order.listView1.Items) order.listView1.Items.Remove(item);
            for (int i = 0; i < orderList.Count; i++)
            {
                if (orderList[i].numberTable.ToString() == numberOrder)
                {
                    for (int j = 0; j < orderList[i].dishList.Count; j++)
                    {
                        string[] items = { orderList[i].dishList[j].name,
                            orderList[i].dishList[j].count.ToString(),
                            orderList[i].dishList[j].cost.ToString() };
                        order.listView1.Items.Add(new ListViewItem(items));
                        order.label2.Text = numberOrder;
                        order.label3.Text = orderList[i].price.ToString();
                    }
                    i = orderList.Count - 1;
                    
                }
            }
        }

        private static void button_Click_Order(object sender, EventArgs e)
        {
            var button = (Button)sender;
            if (button != null)
            {
                f2.ShowOrderSlide(button.Text);   
            }
        }
        private static void button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            if (button != null)
            {
                string price = GetDishPrice(button.Text);
                f2.label3.Text = price + " ₽";
                f2.label4.Text = "Состав: " + GetDishComponent(button.Text);
                f2.nameDish = button.Text;
                f2.priceDish = price;
            }
        } // функция для кнопок блюд

        public static void CheckOut(string fio, string cost, List<Dish> dishList) // добавление заказа в базу
        {
            using(connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.zakaz " +
                    "(fiosotr, zakcost) VALUES (@fiosotr, @zakcost)", connect);
                command.Parameters.AddWithValue("@FioSotr",fio);
                command.Parameters.AddWithValue("@ZakCost", Convert.ToInt32(cost));
                command.ExecuteNonQuery();
            }

            int zaknumber = 0;
            connect = new NpgsqlConnection(connString);
            connect.Open();
            NpgsqlCommand command1 = new NpgsqlCommand("SELECT zaknumber FROM " +
               "public.zakaz WHERE zakcost = " + cost, connect);
            using (command1)
            {
                NpgsqlDataReader reader = command1.ExecuteReader();
                while (reader.Read())
                {
                    zaknumber = Convert.ToInt32(reader[0]);
                }
            }
            // добавление компонентов заказа в базу
            for(int i=0; i< dishList.Count; i++)
            {
                connect = new NpgsqlConnection(connString);
                using (connect)
                {
                    connect.Open();
                    NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.zakcomponent" +
                        "(zaknumber, dishname, dishcount) VALUES (@zaknumber, @dishname, " +
                        "@dishcount)", connect);
                    command.Parameters.AddWithValue("@zaknumber", zaknumber);
                    command.Parameters.AddWithValue("@dishname", dishList[i].name);
                    command.Parameters.AddWithValue("@dishcount", dishList[i].count);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static double CheckClientCard(string tel) // добавление скидки по номеру телефона
        {
            int percent = 0;
            connect = new NpgsqlConnection(connString);
            connect.Open();
            string telephone = "";

            for(int i=0; i<tel.Length; i++)
            {
                switch (i)
                {
                    case 0:
                    break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 9:
                        break;
                    default:
                    telephone += tel[i].ToString();
                        break;
                }
            }

            NpgsqlCommand command = new NpgsqlCommand("SELECT salepercent FROM " +
               "public.cardclient WHERE telnumber = '" + telephone + "'", connect);

            using (command)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    percent = Convert.ToInt32(reader[0]);
                }
            }
            connect.Close();
            return percent;
        }

        public static string GetHash(string plaintext)
        {
            var sha = new SHA1Managed();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
            return Convert.ToBase64String(hash);
        }

        public static void AddStaff(string fio, string dolzhn, string stazh, string login, string pass)
        {
            connect = new NpgsqlConnection(connString);

            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.sotrudnik" +
                    "(fiosotr, dolzhn, stazh, pass, login) VALUES (@fio, @dolzhn, " +
                    "@stazh, @pass, @log)", connect);
                command.Parameters.AddWithValue("@fio", fio);
                command.Parameters.AddWithValue("@dolzhn", dolzhn);
                command.Parameters.AddWithValue("@stazh", Convert.ToInt32(stazh));
                command.Parameters.AddWithValue("@pass", pass);
                command.Parameters.AddWithValue("@log", login);
                command.ExecuteNonQuery();
            }
        }

        public static string CheckStaff(string login, string pass)
        {
            connect = new NpgsqlConnection(connString);
            connect.Open();
            string needPass = "", dolzhn = "Не найден!", newDolzhn = ""; 
            NpgsqlCommand command = new NpgsqlCommand("SELECT dolzhn, pass FROM sotrudnik" +
                " WHERE login = '" + login + "'", connect);
            using (command)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    newDolzhn = reader[0].ToString();
                    needPass = reader[1].ToString();
                }
            }
            if (needPass == pass)  dolzhn = newDolzhn;

            connect.Close();
            return dolzhn;
        }

        public static string ReturnFio(string login)
        {
            string fio = "";
            connect = new NpgsqlConnection(connString);
            connect.Open();

            NpgsqlCommand command = new NpgsqlCommand("SELECT fiosotr FROM sotrudnik" +
                " WHERE login = '" + login + "'", connect);
            using (command)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    fio = reader[0].ToString();
                }
            }
            connect.Close();
            return fio;
        }

        public static void AddDish(string name, double price, double weight, string category, List<Product> productList)
        {
            connect = new NpgsqlConnection(connString);

            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.dish" +
                    "(dishname, price, weight, category) VALUES (@dishname, @price, " +
                    "@weight, @category)", connect);
                command.Parameters.AddWithValue("@dishname", name );
                command.Parameters.AddWithValue("@price", Convert.ToInt32(price));
                command.Parameters.AddWithValue("@weight", Convert.ToInt32(weight));
                command.Parameters.AddWithValue("@category", category);
                command.ExecuteNonQuery();
            }

            for (int i = 0; i < productList.Count; i++)
            {
                connect = new NpgsqlConnection(connString);
                using (connect)
                {
                    connect.Open();
                    NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.dishcomponent" +
                        "(dishname, productname, countprod) VALUES (@dish, @prod, " +
                        "@count)", connect);
                    command.Parameters.AddWithValue("@dish", name);
                    command.Parameters.AddWithValue("@prod", productList[i].name);
                    command.Parameters.AddWithValue("@count", productList[i].count);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void GetProducts(List<string> products)
        {
            connect.Open();

            string dish = "";

            NpgsqlCommand command = new NpgsqlCommand("SELECT productname FROM public.product", connect);

            using (command)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dish = reader[0].ToString();
                    products.Add(dish);
                }
            }
            connect.Close();
        }

        public static void GetSklad(List<Sklad> skladList)
        {
            skladList.Clear();

            connect = new NpgsqlConnection(connString);
            connect.Open();

            string number = "";
            string temp = "";

            NpgsqlCommand command = new NpgsqlCommand("SELECT skladnumber, temponsklad " +
                "FROM public.sklad", connect);

            using (command)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    number = reader[0].ToString();
                    temp = reader[1].ToString();
                    Sklad item = new Sklad();
                    item.number = Convert.ToInt32(number);
                    item.temp = Convert.ToInt32(temp);
                    skladList.Add(item);
                }
            }
            connect.Close();
        }

        public static void AddSklad(string number, string temp)
        {
            connect = new NpgsqlConnection(connString);

            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.sklad" +
                    "(skladnumber, temponsklad) VALUES (@number, @temp)", connect);
                command.Parameters.AddWithValue("@number", Convert.ToInt32(number));
                command.Parameters.AddWithValue("@temp", Convert.ToInt32(temp));
                command.ExecuteNonQuery();
            }
        }

        public static void GetProductions(List<Production> product)
        {
            connect = new NpgsqlConnection(connString);
            connect.Open();

            string prod = "";
            string count = "";
            string sklad = "";

            NpgsqlCommand command = new NpgsqlCommand("SELECT productname, skladnumber," +
                "quantity FROM public.product", connect);

            using (command)
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    prod = reader[0].ToString();
                    sklad = reader[1].ToString();
                    count = reader[2].ToString();
                    Production item = new Production();
                    item.name = prod;
                    item.count = Convert.ToInt32(count);
                    item.sklad = Convert.ToInt32(sklad);
                    product.Add(item);
                }
            }
            connect.Close();
        }

       public static void UpdateProducts(List<string> prod)
       {
            connect = new NpgsqlConnection(connString);
            int count = Convert.ToInt32(prod[2]);
            string name = prod[0];
            int sklad = Convert.ToInt32(prod[1]);
            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("UPDATE public.product" +
                    " SET quantity = " + count + " WHERE productname = '" + name +
                    "' AND skladnumber = " + sklad, connect);
                command.ExecuteNonQuery();
            }
       }

        public static void AddProduct(string name, int sklad, int count)
        {
            connect = new NpgsqlConnection(connString);

            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.product" +
                    "(productname, skladnumber, quantity) VALUES (@name, @sklad, " +
                    "@count)", connect);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@sklad", sklad);
                command.Parameters.AddWithValue("@count", count);
                command.ExecuteNonQuery(); 
            }
        }

        public static void DeleteProduct(string name, int sklad)
        {
            connect = new NpgsqlConnection(connString);

            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.product" +
                    " WHERE productname = '" + name + "' AND skladnumber = " + sklad, connect);
                command.ExecuteNonQuery();
            }
        }

        public static void GetDishWithComponent(List<DishWithComponent> list)
        {
            connect = new NpgsqlConnection(connString);
            connect.Open();

            

            NpgsqlCommand command = new NpgsqlCommand("SELECT dishname, price, " +
                "weight, category FROM public.dish", connect);

            using (command)
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DishWithComponent item = new DishWithComponent();
                        item.name = reader[0].ToString();
                        item.price = Convert.ToInt32(reader[1]);
                        item.weight = Convert.ToInt32(reader[2]);
                        item.category = reader[3].ToString();
                        item.productList = new List<Product>();
                        list.Add(item);
                    }
                }
            }
            

            for (int i=0; i < list.Count; i++)
            {
                NpgsqlCommand command1 = new NpgsqlCommand("SELECT productname, countprod FROM " +
                    "public.dishcomponent WHERE dishname = '" + list[i].name + "'", connect);
                using (command1)
                {
                    using (NpgsqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product prod = new Product();
                            prod.name = reader[0].ToString();
                            prod.count = Convert.ToDouble(reader[1]);
                            list[i].productList.Add(prod);
                            
                        }
                    }
                }
            }

            connect.Close();
        }

        public static void DeleteDishWithComponent(string name)
        {
            connect = new NpgsqlConnection(connString);

            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.dishcomponent" +
                    " WHERE dishname = '" + name + "'", connect);
                command.ExecuteNonQuery();
            }

            connect = new NpgsqlConnection(connString);

            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("DELETE FROM public.dish" +
                    " WHERE dishname = '" + name + "'", connect);
                command.ExecuteNonQuery();
            }
        }

        public static void AddNewClient(string name, string number, int age, int sale)
        {
            connect = new NpgsqlConnection(connString);

            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.client" +
                    "(telnumber, fioclient, age) VALUES (@tel, @name, " +
                    "@age)", connect);
                command.Parameters.AddWithValue("@tel", number);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                command.ExecuteNonQuery(); 
            }
            // ДОБАВИТЬ ВСТАВКУ КАРТЫ

            int numberCard = 0;
            connect = new NpgsqlConnection(connString);
            connect.Open();
            NpgsqlCommand command1 = new NpgsqlCommand("SELECT MAX(cardnumber) " +
                "FROM public.cardclient", connect);
            using (command1)
            {
                using (NpgsqlDataReader reader = command1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        numberCard = Convert.ToInt32(reader[0]);
                    }
                }
            }
            connect.Close();

            numberCard++;
            connect = new NpgsqlConnection(connString);

            using (connect)
            {
                connect.Open();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO public.cardclient" +
                    "(cardnumber, telnumber, salepercent) VALUES (@card, @tel, " +
                    "@sale)", connect);
                command.Parameters.AddWithValue("@card", numberCard);
                command.Parameters.AddWithValue("@tel", number);
                command.Parameters.AddWithValue("@sale", sale);
                command.ExecuteNonQuery();
            }
        }
    }

    
}
