using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KursachTest
{
    public partial class Form2 : Form
    {
        public string nameDish = "", // Имя блюда
                      priceDish = "", // Цена блюда
                      countDish = "1", // Количество блюда
                      sotrName = ""; // Имя сотрудника

        public double finishPrice = 0, // Стоимость до скидки
                      finishPriceSale = 0, // Стоимость со скидкой
                      sale = 0; // Процент скидки

        List<Dish> dishList = new List<Dish>(); // Список блюд в заказе
        List<Order> orderList = new List<Order>(); // Список заказов
        List<Button> orderButtons = new List<Button>(); // Список кнопок заказов
        List<Production> productsList = new List<Production>(); // Список продуктов в наличии

        public Form2(string fioSotr)
        {
            InitializeComponent();
            sotrName = fioSotr;
            slidePanel(button2); // показ красной панельки
            seasonSlide1.BringToFront();// показ первой панели при запуске
            updateTime(); // Обновление времени
            Functions.f2 = this; 
            listView1.ColumnWidthChanging += new ColumnWidthChangingEventHandler(ListView1_ColumnWidthChanging);
            Functions.GetProductions(productsList);
        }

        /*WORK WITH ORDERS*/
        public void ShowOrderSlide(string num) {
            Functions.ContainOrderSlide(orderList, num);
            orderSlide1.BringToFront();
        }
        public void DeleteOrderFromList(string tableNum)
        {
            for(int i=0; i<orderList.Count; i++)
            {
                if (orderList[i].numberTable.ToString() == tableNum)
                {
                    orderList.RemoveAt(i);
                    Controls.Remove(orderButtons[i]); // УДАЛЯЕТ НЕ ТЕ КНОПКИ!
                    orderButtons.RemoveAt(i);
                    Functions.MoveOrderButtons(orderButtons, i);
                }
            }
        }
        /*WORK WITH ORDERS---END*/

        /*TIME FUNCTIONS*/
        private void updateTime() // обновление времени на слайде
        {
            ShowDateTime();
            System.Windows.Forms.Timer timerDateTime = new System.Windows.Forms.Timer();
            timerDateTime.Interval = 5000; // интервал 5 сек
            timerDateTime.Tick += new EventHandler(timerDateTime_Tick);
            timerDateTime.Start();
        }
        private void timerDateTime_Tick(object sender, EventArgs e) 
        {
            ShowDateTime();
        } // Тик для таймера
        private void ShowDateTime() // показ времени
        {
            string hour = "";
            string minute = "";
            int h = DateTime.Now.Hour;
            int m = DateTime.Now.Minute;
            
            if (h < 10) hour += "0";
            hour += h.ToString();
            if (m < 10) minute += "0";
            minute += m.ToString();

            label2.Text = hour + ":" + minute;
        }
        /*TIME FUNCTIONS---END*/

        private void slidePanel(Button a) // красная панелька меню
        {
            sidePanel.Height = a.Height;
            sidePanel.Top = a.Top;
        }

        /*ALL CLEAR FUNCTIONS*/
        public void clearLabel() // очистка цены, состава и имени блюда
        {
            label3.Text = "";
            label4.Text = "";
            nameDish = "";
            countDish = "1";
        }
        public void ClearAndUpdatePrice() // Обновление цены и сброс количества и имени
        {
            if (sale != 0) finishPriceSale = finishPrice - (finishPrice * (sale / 100.0f));
            else finishPriceSale = finishPrice;
            label6.Text = finishPrice.ToString();
            label13.Text = finishPriceSale.ToString();
            countDish = "1";
            nameDish = "";
        } // очистка 
        private void ClearAll() // Очистка списков, ListView и стоимости. Обнуление переменных 
        {
            clearLabel();
            label13.Text = "0";
            label12.Text = "0";
            foreach (ListViewItem item in listView1.Items) listView1.Items.Remove(item);
            label6.Text = "0";
            finishPrice = 0;
            finishPriceSale = 0;
            sale = 0;
            dishList.RemoveRange(0, dishList.Count);
            listBox1.ClearSelected();
        }
        /*ALL CLEAR FUNCTIONS---END*/

        /*BUTTONS*/
        private void button1_Click(object sender, EventArgs e) // кнопка выход
        {
            Form form1 = Application.OpenForms[0];
            form1.Close();
        }
        private void button2_Click(object sender, EventArgs e) // кнопка Сезонное
        {
            slidePanel(button2);
            seasonSlide1.BringToFront();
            clearLabel();
        } 
        private void button3_Click(object sender, EventArgs e) // кнопка Основные
        {
            slidePanel(button3);
            mainDishSlide1.BringToFront();
            clearLabel();
        }
        private void button4_Click(object sender, EventArgs e) // кнопка Гарниры
        {
            slidePanel(button4);
            sideDishSlide1.BringToFront();
            clearLabel();
        }
        private void button5_Click(object sender, EventArgs e) // кнопка Салаты
        {
            slidePanel(button5);
            saladDishSlide1.BringToFront();
            clearLabel();
        }
        private void button6_Click(object sender, EventArgs e) // кнопка Закуски
        {
            slidePanel(button6);
            snackDishSlide1.BringToFront();
            clearLabel();
        }
        private void button7_Click(object sender, EventArgs e) // кнопка Напитки
        {
            slidePanel(button7);
            drinksDishSlide1.BringToFront();
            clearLabel();
        }
        private void button8_Click(object sender, EventArgs e) // кнопка Десерты
        {
            slidePanel(button8);
            dessertDishSlide1.BringToFront();
            clearLabel();
        }
        private void Button9_Click(object sender, EventArgs e) // кнопка добавить
        {
            
                    AddDishToList();
                    ListViewItem item1 = listView1.FindItemWithText(nameDish);
                    if (item1 != null)
                    {
                        item1.SubItems[1].Text = countDish;
                    }
                    else
                    {
                        string[] items = { nameDish, countDish, priceDish };
                        listView1.Items.Add(new ListViewItem(items));
                    }

                    finishPrice += Convert.ToInt32(priceDish);
                    ClearAndUpdatePrice();
                
            
        }
        private void Button10_Click(object sender, EventArgs e) // кнопка удалить
        {
            ListView.SelectedIndexCollection collection = listView1.SelectedIndices;
            if (collection.Count != 0)
            {
                int select = listView1.SelectedIndices[0];
                priceDish = listView1.Items[select].SubItems[2].Text;
                countDish = listView1.Items[select].SubItems[1].Text;
                nameDish = listView1.Items[select].SubItems[0].Text;
                listView1.Items.RemoveAt(collection[0]);

                int price = Convert.ToInt32(priceDish);
                int count = Convert.ToInt32(countDish);

                finishPrice -= (price * count);
                DelFromList();
                ClearAndUpdatePrice();
            }
        }
        private void Button11_Click(object sender, EventArgs e) // кнопка плюс
        {
            var collection = listView1.SelectedIndices;
            if (collection.Count != 0)
            {
                int select = listView1.SelectedIndices[0];
                nameDish = listView1.Items[select].SubItems[0].Text;
                countDish = listView1.Items[select].SubItems[1].Text;
                priceDish = listView1.Items[select].SubItems[2].Text;

                int price = Convert.ToInt32(priceDish);

                AddDishToList();

                listView1.Items[select].SubItems[1].Text = countDish;

                finishPrice += price;
                ClearAndUpdatePrice();
            }
        }
        private void Button12_Click(object sender, EventArgs e) //кнопка минус
        {
            var collection = listView1.SelectedIndices;
            if (collection.Count != 0)
            {
                int select = listView1.SelectedIndices[0];
                nameDish = listView1.Items[select].SubItems[0].Text;
                countDish = listView1.Items[select].SubItems[1].Text;
                priceDish = listView1.Items[select].SubItems[2].Text;

                if (Convert.ToInt32(countDish) != 1)
                {

                    int price = Convert.ToInt32(priceDish);

                    for (int i = 0; i < dishList.Count; i++)
                    {
                        if (dishList[i].name == nameDish)
                        {
                            dishList[i].count -= 1;
                            countDish = dishList[i].count.ToString();
                            check = true;
                        }
                    }

                    listView1.Items[select].SubItems[1].Text = countDish;
                    finishPrice -= price;
                    ClearAndUpdatePrice();
                }
            }
        }
        private void Button13_Click(object sender, EventArgs e) // кнопка Добавить скидку
        {
            panel6.Visible = true;
            panel6.Enabled = true;
            maskedTextBox1.Focus();
        }
        private void Button14_Click(object sender, EventArgs e) // кнопка Поиск
        {
            slidePanel(button14);
            searchDishSlide1.BringToFront();
            searchDishSlide1.textBox1.Focus();
            clearLabel();
        }
        private void Button15_Click(object sender, EventArgs e) // кнопка Оформить заказ
        {
            if (label13.Text != "0" & listBox1.SelectedItem != null)
                panel8.Visible = true;
        }
        private void Button16_Click(object sender, EventArgs e) // кнопка добавления скидки
        {

            sale = Functions.CheckClientCard(maskedTextBox1.Text);
            maskedTextBox1.Text = "";
            panel6.Visible = false;
            panel6.Enabled = false;

            ClearAndUpdatePrice();
            label12.Text = sale.ToString() + "%";
        }
        private void Button17_Click(object sender, EventArgs e) // кнопка Закрыть скидку
        {
            panel6.Visible = false;
            panel6.Enabled = false;
            maskedTextBox1.Text = "";
        }
        private void Button18_Click(object sender, EventArgs e) // подтверждение оформления заказа
        {
            bool isRight = CheckCountProducts(dishList, productsList);

            if (isRight)
            {
                Functions.CheckOut(sotrName, label13.Text, dishList);
                Functions.UpdateConnect();
                Functions.RemoveProducts(dishList);
                Functions.UpdateConnect();

                panel7.Visible = true;
                panel7.BringToFront();
                aTimer.Interval = 1000;
                aTimer.Tick += new EventHandler(OnTimeEvent);
                aTimer.Start();
                AddOrderToList();
                ClearAll();

                panel8.Visible = false;
            }
        } 
        private void Button19_Click(object sender, EventArgs e) // отклонение заказа
        {
            panel8.Visible = false;
        }
        private void Button20_Click(object sender, EventArgs e) // Добавление клиента
        {
            Functions.client.BringToFront();
        }
        /*BUTTONS----END*/

        /*CHECKOUT IS RIGHT*/
        Timer aTimer = new Timer();
        int j = 0;
        public void HidePanel()
        {
            if (j == 1)
            {
                panel7.Visible = false;
                panel9.Visible = false;
                aTimer.Stop();
            }else 
            j++;
        }
        private void OnTimeEvent(Object source, EventArgs e)
        {
            HidePanel();
        }
        private bool CheckCountProducts(List<Dish> dishlist, List<Production> prodList)
        {
            for (int i = 0; i < dishList.Count; i++)
            {
                int countDish = dishlist[i].count;
                string nameDish = dishlist[i].name;
                int countDishMax = 0;
                List<Product> containProduct = new List<Product>();
                Functions.GetDishComponent(nameDish, containProduct);

                for (int q = 0; q < containProduct.Count; q++)
                {
                    for (int j = 0; j < prodList.Count; j++)
                    {
                        if (containProduct[q].name == prodList[j].name)
                        {
                            if (containProduct[q].count * countDish > prodList[j].count)
                            {
                                if (prodList[j].count == 0) countDishMax = 0;
                                else countDishMax = prodList[j].count / Convert.ToInt32(containProduct[q].count);
                                label16.Text = "Неудачно! Количество блюд " + nameDish + " максимум " + countDishMax;
                                panel9.Visible = true;
                                panel8.Visible = false;
                                panel9.BringToFront();
                                aTimer.Interval = 1000;
                                aTimer.Tick += new EventHandler(OnTimeEvent);
                                aTimer.Start();
                                return false;
                            }
                        }
                    }
                }

            }
            return true;
        } // проверка на наличие продуктов на складе
        /*CHECKOUT IS RIGHT---END*/

        /*WORK WITH LISTS*/
        bool check = false;
        public void AddDishToList() // Добавление блюда в список заказа
        {
            for (int i = 0; i < dishList.Count; i++)
            {
                if (dishList[i].name == nameDish)
                {
                    dishList[i].count += 1;
                    countDish = dishList[i].count.ToString();
                    check = true;
                }
            }
            if (!check)
            {
                Dish dish = new Dish();
                dish.name = nameDish;
                dish.cost = Convert.ToInt32(priceDish);
                dish.count = 1;
                dishList.Add(dish);
            }
            check = false;
        }
        public void DelFromList() // Удаление блюда из списка
        {
            for (int i = 0; i < dishList.Count; i++)
            {
                if (dishList[i].name == nameDish)
                {
                    dishList.RemoveAt(i);
                    break;
                }
            }
        }
        public void AddOrderToList() // Добавление заказа в список
        {
            Order item = new Order();
            item.dishList = new List<Dish>();
            item.numberTable = listBox1.SelectedIndex + 1;
            item.price = finishPriceSale;
            for (int i = 0; i < dishList.Count; i++)
            {
                item.dishList.Add(dishList[i]);
                label4.Text += "+ ";
            }
            orderList.Add(item);

            Functions.SpawnOrderButtons(orderList, orderButtons);
        }
        /*WORK WITH LISTS---END*/

        /*CANNOT RESIZE COLUMNS*/
        private void ListView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            //функция чтобы нельзя было изменять ширину колонок
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }
        /*CANNOT RESIZE COLUMNS---END*/

        /*EMPTY FUNCTIONS*/
        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void ListView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        /*EMPTY FUNCTIONS---END*/
    }
}
