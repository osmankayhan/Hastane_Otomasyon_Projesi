using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Hastane_Otomasyon_Projesi
{
    public partial class FrmDoktorDetay : Form
    {
        public FrmDoktorDetay()
        {
            InitializeComponent();
        }
        SqlBaglantisi bgl = new SqlBaglantisi();
        public string TC;
        int doktorId;

        private void FrmDoktorDetay_Load(object sender, EventArgs e)
        {
            LblTC.Text = TC; ;

          //  Doktor Ad Soyad
          SqlCommand komut =new SqlCommand("select DoktorAd,DoktorSoyad From Tbl_Doktorlar Where DoktorTC=@p1",bgl.baglanti());
          komut.Parameters.AddWithValue("@p1",LblTC.Text);  
          SqlDataReader dr = komut.ExecuteReader();
          while (dr.Read())
            {
                LblAdSoyad.Text = dr[0] + " " + dr[1];

            }
          bgl.baglanti().Close();


            // randevular
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * From Tbl_Randevular Where RandevuDoktor='" + LblAdSoyad.Text + "'", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;  
        }

        private void BtnBİlgiDüzenle_Click(object sender, EventArgs e)
        {
            FrmDoktorBilgiDuzenle fr = new FrmDoktorBilgiDuzenle();
            fr.TCN0 = LblTC.Text;
            fr.Show();
        }

        private void BtnDuyurular_Click(object sender, EventArgs e)
        {
            FrmDuyurular fr =new FrmDuyurular();
            fr.Show();
        }

        private void BtnÇıkış_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            RchSikayet.Text = dataGridView1.Rows[secilen].Cells[7].Value.ToString();

        }
    }
   
}
