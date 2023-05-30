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
using System.Diagnostics;

namespace Hastane_Otomasyon_Projesi
{
    public partial class FrmHastaDetay : Form
    {
        public FrmHastaDetay()
        {
            InitializeComponent();
        }

        public string tc;
        SqlBaglantisi bgl = new SqlBaglantisi();
        private void FrmHastaDetay_Load(object sender, EventArgs e)
        {
            LblTC.Text = tc;
             
            // AD soyad Çekme

            SqlCommand Komut = new SqlCommand("select HastaAd,HastaSoyad From Tbl_Hastalar Where HastaTC=@p1",bgl.baglanti());
            Komut.Parameters.AddWithValue("@p1", LblTC.Text);
            SqlDataReader dr = Komut.ExecuteReader();

            while (dr.Read())
            {
                LblAdSoyad.Text = dr[0] + " " + dr[1];
            }
            bgl.baglanti().Close();

            // Randevu Geçmişi
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(" Select * From Tbl_Randevular Where HastaTc='" + tc+"'",bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            
            // baglanti çekme
            SqlCommand komut2 = new SqlCommand("select BransAd From Tbl_Branslar",bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();

            while (dr2.Read())
            {
                CmbBrans.Items.Add(dr2[0]);
            }
            bgl.baglanti().Close();

        }

        private void CmbBranş_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbDoktor.Items.Clear();

            SqlCommand komut3 = new SqlCommand("select DoktorAd,DoktorSoyad From Tbl_Doktorlar Where DoktorBrans=@p1", bgl.baglanti());
            komut3.Parameters.AddWithValue("@p1", CmbBrans.Text);
            SqlDataReader dr3 = komut3.ExecuteReader();

            while (dr3.Read())
            {
                CmbDoktor.Items.Add(dr3[0]);
            }
            bgl.baglanti().Close();
        }

        private void CmbDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {

            
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * From Tbl_Randevular Where RandevuBrans='" + CmbBrans.Text + "'" + " and RandevuDoktor='" + CmbDoktor.Text + "' and RandevuDurum=0", bgl.baglanti());
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void LnkBilgiDuzenle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FRmBİlgiDuzenle fr = new FRmBİlgiDuzenle();
            fr.TCno = LblTC.Text;
            fr.Show();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView2.SelectedCells[0].RowIndex;
            Txtid.Text = dataGridView2.Rows[secilen].Cells[0].Value.ToString();
        }

        private void BtnRandevuAl_Click(object sender, EventArgs e)
        {
            // Hata Var Düzeltilecek
            SqlCommand komut = new SqlCommand("Insert Into Tbl_Randevular (RandevuDurum,RandevuBrans,RandevuDoktor) values (0,@p1,@p2)", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1",CmbBrans.Text);
            komut.Parameters.AddWithValue("@p2", CmbDoktor.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Randevu ALındı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LblTC_Click(object sender, EventArgs e)
        {

        }
    }
}
