using DernekYonetim.BLL;
using DernekYonetim.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DernekYonetim.UI
{
    public partial class frmYoneticiler : Form
    {
        private YoneticiService yoneticiService;
        private YoneticiDetayVM _yoneticiDetay;
        private YoneticiDetayVM yoneticiDetayVM
        {
            get { return _yoneticiDetay; }
            set
            {
                _yoneticiDetay = value;
                RefreshDetaySection();
            }
        }
        public frmYoneticiler()
        {
            InitializeComponent();
            yoneticiService = new YoneticiService();
        }

        private void frmYoneticiler_Load(object sender, EventArgs e)
        {
            RefreshListYonetici();
        }

        private void RefreshListYonetici()
        {
            lst_Yoneticiler.DataSource = null;
            lst_Yoneticiler.DataSource = yoneticiService.YoneticiListesi().Select(x => new YoneticiListeVM() { UnvanId = x.UnvanId, AdSoyad = x.ToString() }).ToList();
            lst_Yoneticiler.ValueMember = "UnvanId";
            lst_Yoneticiler.DisplayMember = "AdSoyad";
        }

        private void RefreshDetaySection()
        {
            txtYoneticiAd.Text = yoneticiDetayVM.Yonetici.Ad;
            txtYoneticiSoyad.Text = yoneticiDetayVM.Yonetici.Soyad;
            txtYoneticiTel.Text = yoneticiDetayVM.Yonetici.Telefon;
            txtYoneticiEMail.Text = yoneticiDetayVM.Yonetici.EMail;
            txtYoneticiUnvan.Text = yoneticiDetayVM.Yonetici.Unvan;
            dtpYoneticiBaslangic.Value = yoneticiDetayVM.Yonetici.BaslangicTarihi;
            dtpYoneticiBitis.Value = yoneticiDetayVM.Yonetici.BitisTarihi.HasValue ? yoneticiDetayVM.Yonetici.BitisTarihi.Value : dtpYoneticiBitis.MinDate;
        }

        private void lst_Yoneticiler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lst_Yoneticiler.SelectedItem == null)
                return;
            var selected = (YoneticiListeVM)lst_Yoneticiler.SelectedItem;
            var detay = new YoneticiDetayVM()
            {
                Yonetici = yoneticiService.YoneticiyeGoreUnvanGetir(selected.Id)
            };
            yoneticiDetayVM = detay;
        }
    }
}
