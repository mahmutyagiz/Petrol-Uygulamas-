using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Petrol
{
    public partial class Form4 : Form
    {
        private DataBaseHelper dbHelper;
        private string pompaciAd;
        private string pompaciSoyad;
        private string sube;

        public Form4(string ad, string soyad, string sube)
        {
            InitializeComponent();
            dbHelper = new DataBaseHelper();

            this.pompaciAd = ad;
            this.pompaciSoyad = soyad;
            this.sube = sube;

            label1.Text = $"Hoşgeldiniz Sayın {ad} {soyad}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string plaka = textBox1.Text.Trim();
            string yakitMiktariStr = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(plaka) || string.IsNullOrWhiteSpace(yakitMiktariStr) ||
                !decimal.TryParse(yakitMiktariStr, out decimal yakitMiktari) || yakitMiktari <= 0)
            {
                MessageBox.Show("Geçerli yakıt ve plaka giriniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = dbHelper.GetConnection())
                {
                    string query = @"INSERT INTO bekleyen_islemler (pompaciad, pompacisoyad, plaka, yakit_miktari, sube) 
                                     VALUES (@pompaciAd, @pompaciSoyad, @plaka, @yakitMiktari, @sube)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@pompaciAd", this.pompaciAd);
                        command.Parameters.AddWithValue("@pompaciSoyad", this.pompaciSoyad);
                        command.Parameters.AddWithValue("@plaka", plaka);
                        command.Parameters.AddWithValue("@yakitMiktari", yakitMiktari);
                        command.Parameters.AddWithValue("@sube", this.sube);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Kayıt başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox1.Clear();
                            textBox2.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Kayıt eklenirken  sorun oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
