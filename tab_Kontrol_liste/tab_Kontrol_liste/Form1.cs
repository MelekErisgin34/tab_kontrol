using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace tab_Kontrol_liste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string baglanti = "Server=localhost;Database=sinema_veritabani;Uid=root;Pwd=''";
        string hedefDosya;
        private void FormFilmEkle_Load(object sender, EventArgs e)
        {

        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "resim dosyası |*.jpg;*.nef;*.png|video|*.avi|Tüm dosyalar|*.*";
            dosya.Title = "Dosya Seçin";

            if (dosya.ShowDialog() == DialogResult.OK)
            {
                string kaynakDosya = dosya.FileName;
                hedefDosya = Path.Combine("resimler", Guid.NewGuid() + ".jpg");


                if (!Directory.Exists("resimler"))
                {
                    Directory.CreateDirectory("resimler");
                }

                File.Copy(kaynakDosya, hedefDosya);
                pbEPoster.ImageLocation = hedefDosya;
                pbEPoster.SizeMode = PictureBoxSizeMode.StretchImage;

            }




        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(baglanti))
            {

                string sql = "INSERT INTO filmler (filmad,tur,yil,imdb_puan,film_posteri,ozet) VALUES (@filmad,@tur,@yil,@imdb,@poster,@ozet);";
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@filmad", txtFilmAd.Text);
                cmd.Parameters.AddWithValue("@tur", cmbETür.Text);
                cmd.Parameters.AddWithValue("@yil", txtYil.Text);
                cmd.Parameters.AddWithValue("@imdb", txtImdb.Text);
                cmd.Parameters.AddWithValue("@poster", hedefDosya);
                cmd.Parameters.AddWithValue("@ozet", txtEözet.Text);



                DialogResult result = MessageBox.Show("Film eklensin mi?", "Film Ekle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    cmd.ExecuteNonQuery();

                    txtFilmAd.Clear();
                    txtEözet.Clear();
                    txtImdb.Clear();
                    txtYil.Clear();
                    cmbETür.SelectedIndex = -1;
                }




            }
        }

        private void txtFilmOzet_TextChanged(object sender, EventArgs e)
        {

            txtEözet.ScrollBars = ScrollBars.Vertical;
        }
    }
}
     
    

