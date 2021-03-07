using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Firebase_connecting
{
    public partial class Form1 : Form
    {
        DataTable dt = new DataTable();
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "HOTJTyjPnKuefSJPhxMqfl3cFt7yFBzUPWIHNglE",
            BasePath = "https://test-79672-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);
            if(client!=null)
            {
                MessageBox.Show("Connection established");
            }
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Address");
            dt.Columns.Add("Age");

            dataGridView1.DataSource = dt;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse resp = await client.GetTaskAsync("Information/1");
            Counter_Class1 get = resp.ResultAs<Counter_Class1>();

            MessageBox.Show(get.cnt);
            var data = new Data
            {
                Id = (Convert.ToInt32(get.cnt)+1).ToString(),
                Name = textBox2.Text,
                Address = textBox3.Text,
                Age = textBox4.Text
            };
            SetResponse response = await client.SetTaskAsync("Information/" + data.Id, data);
            Data result = response.ResultAs<Data>();

            MessageBox.Show("Data Inserted" + result.Id);

            var obj = new Counter_Class1
            {
                cnt = data.Id
            };

            SetResponse response1 = await client.SetTaskAsync("Information/1",obj);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetTaskAsync("Information/"+textBox1.Text);
            Data obj = response.ResultAs<Data>();

            textBox1.Text = obj.Id;
            textBox2.Text = obj.Name;
            textBox3.Text = obj.Address;
            textBox4.Text = obj.Age;

            MessageBox.Show("Data retrived successfully");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                Id=textBox1.Text,
                Name=textBox2.Text,
                Address=textBox3.Text,
                Age=textBox4.Text
            };

            FirebaseResponse response = await client.UpdateTaskAsync("Information/" + textBox1.Text,data);
            Data result = response.ResultAs<Data>();
            MessageBox.Show("Data updated at ID: " + result.Id);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Information/" + textBox1.Text);
            MessageBox.Show("Delete record of ID:" +textBox1.Text);
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Information");
            MessageBox.Show("All element deleted");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            export();
        }
        
        private async void export()
        {
            dt.Rows.Clear();
            int i = 0;
            FirebaseResponse resp1 = await client.GetTaskAsync("Information/1");
            Counter_Class1 obj1=resp1.ResultAs<Counter_Class1>();
            int cnt = Convert.ToInt32(obj1.cnt);

            MessageBox.Show(cnt.ToString());

            while(true)
            {
                if(i==cnt)
                    {
                    break;
                    }
                i++;
                try
                {
                    FirebaseResponse resp2 = await client.GetTaskAsync("Information/"+1);
                    Data obj2 = resp2.ResultAs<Data>();

                    DataRow row = dt.NewRow();
                    row["Id"] = obj2.Id;
                    row["Name"] = obj2.Name;
                    row["Address"] = obj2.Address;
                    row["Age"] = obj2.Age;

                    dt.Rows.Add(row);
                }
                catch
                {

                }
                
            }
            MessageBox.Show("Done");
        }
    }

    internal class Data
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Age { get; set; }
    }
}
