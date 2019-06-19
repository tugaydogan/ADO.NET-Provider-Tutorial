using DernekYonetim.BLL;
using DernekYonetim.BLL.DTOs;
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
    public partial class frmUyeler : Form
    {
        private UyeService uyeService;
        private bool isEditMode = false;
        private MaliHareketlerService maliHareketlerService;
        private UyeDetayVM _uyeDetay;
        private UyeDetayVM uyeDetayVM { get { return _uyeDetay; }
            set { _uyeDetay = value;
                RefreshDetaySection();
            } }

        public frmUyeler()
        {
            InitializeComponent();
            uyeService = new UyeService();
            maliHareketlerService = new MaliHareketlerService();
        }

        private void frmUyeler_Load(object sender, EventArgs e)
        {
            RefreshListe();
        }

        private void lstUyeler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstUyeler.SelectedItem == null)
                return;
            if(isEditMode)
                CloseEditMode();

            var selected = (UyeListeVM)lstUyeler.SelectedItem;
            var detay = new UyeDetayVM()
            {
                Uye = uyeService.IdyeGoreUyeGetir(selected.UyeId)
            };
            detay.MaliHareketler = maliHareketlerService.KisiIdyeGoreMaliHareketleriGetir(detay.Uye.KisiId);
            detay.GecikmisAidatlar = maliHareketlerService.KisiIdyeGoreGecikmisAidatGetir(detay.Uye.KisiId);
            uyeDetayVM = detay;
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isEditMode)
                return;
            OpenEditMode();
        }

        private void OpenEditMode()
        {
            foreach (var item in grpUyeDetay.Controls)
            {
                (item as Control).Enabled = true;
            }
            isEditMode = true;
        }

        private void CloseEditMode()
        {
            foreach (var item in grpUyeDetay.Controls)
            {
                (item as Control).Enabled = false;
            }
            isEditMode = false;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            UyeDTO duzenlenmis = new UyeDTO()
            {
                Ad = txtAd.Text,
                Soyad = txtSoyad.Text,
                Telefon = txtTelefon.Text,
                EMail = txtEMail.Text,
                AktifMi = chkPasif.Checked,
                KisiId = (lstUyeler.SelectedItem as UyeDTO).KisiId,
                UyeId = (lstUyeler.SelectedItem as UyeDTO).UyeId,
                UyelikBaslangicTarihi = dtpBaslangic.Value,
            };
            uyeService.UyeGuncelle(duzenlenmis);
            RefreshListe();
        }

        private void RefreshListe()
        {
            lstUyeler.DataSource = null;
            lstUyeler.DataSource = uyeService.UyeListesi().Select(x=>new UyeListeVM() { UyeId=x.UyeId,AdSoyad=x.ToString()}).ToList();
            lstUyeler.DisplayMember = "AdSoyad";
            lstUyeler.ValueMember = "UyeId";
        }

        private void RefreshDetaySection()
        {
            txtAd.Text = uyeDetayVM.Uye.Ad;
            txtSoyad.Text = uyeDetayVM.Uye.Soyad;
            txtTelefon.Text = uyeDetayVM.Uye.Telefon;
            txtEMail.Text = uyeDetayVM.Uye.EMail;
            dtpBaslangic.Value = uyeDetayVM.Uye.UyelikBaslangicTarihi;
            dtpBitis.Value = uyeDetayVM.Uye.UyelikBitisTarihi.HasValue ? uyeDetayVM.Uye.UyelikBitisTarihi.Value : dtpBitis.MinDate;
            chkPasif.Checked = !uyeDetayVM.Uye.AktifMi;
            RefreshAidatList();
            RefreshGridSource();
        }

        private void odeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAidatOde aidatForm = new frmAidatOde((AidatDTO)lstOdenmemisAidat.SelectedItem);
            if (aidatForm.ShowDialog() == DialogResult.OK)
            {
                AidatOdendi();
            }
        }

        public void RefreshGridSource()
        {
            grdMaliHareket.DataSource = null;
            grdMaliHareket.DataSource = uyeDetayVM.MaliHareketler;
        }

        public void RefreshAidatList()
        {
            lstOdenmemisAidat.DataSource = null;
            lstOdenmemisAidat.DataSource = uyeDetayVM.GecikmisAidatlar;
        }

        public void AidatOdendi()
        {
            uyeDetayVM.MaliHareketler = maliHareketlerService.KisiIdyeGoreMaliHareketleriGetir(uyeDetayVM.Uye.KisiId);
            uyeDetayVM.GecikmisAidatlar = maliHareketlerService.KisiIdyeGoreGecikmisAidatGetir(uyeDetayVM.Uye.KisiId);
            RefreshAidatList();
            RefreshGridSource();
        }
    }
}
