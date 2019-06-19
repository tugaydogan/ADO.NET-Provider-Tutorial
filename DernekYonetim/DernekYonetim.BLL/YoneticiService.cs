using DernekYonetim.BLL.DTOs;
using DernekYonetim.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DernekYonetim.BLL
{
    public class YoneticiService
    {
        private YoneticiRepo yoneticiRepo;
        private UnvanRepo unvanRepo;
        private KisiRepo kisiRepo;

        public YoneticiService()
        {
            kisiRepo = new KisiRepo();
            unvanRepo = new UnvanRepo();
            yoneticiRepo = new YoneticiRepo();
        }

        public int AktifYoneticiSayısı()
        {
            return yoneticiRepo.GetAll().Where(yon => yon.BitisTarihi == null).Count();
        }

        public List<YoneticiDTO> YoneticiListesi()
        {
            var yoneticiEntities = yoneticiRepo.GetAll();
            var kisiEntities = kisiRepo.GetAll();

            List<YoneticiDTO> yoneticiDTOs = new List<YoneticiDTO>();
            foreach (var item in yoneticiEntities)
            {
                var kisi = kisiEntities.SingleOrDefault(x => x.Id == item.KisiId);
                yoneticiDTOs.Add(new YoneticiDTO()
                {
                    BaslangicTarihi = item.BaslangicTarihi,
                    BitisTarihi = item.BitisTarihi,
                    UnvanId = item.UnvanId,
                    KisiId = kisi.Id,
                    Ad = kisi.Ad,
                    Soyad = kisi.Soyad,
                    EMail = kisi.EMail,
                    Telefon = kisi.Telefon
                });
            }
            return yoneticiDTOs;
        }

        public YoneticiDTO YoneticiyeGoreUnvanGetir(int yoneticiId)
        {
            var yonetici = yoneticiRepo.GetById(yoneticiId);
            var unvan = unvanRepo.GetById(yonetici.UnvanId);
            var kisi = kisiRepo.GetById(yonetici.KisiId);
            var yonDTO = new YoneticiDTO()
            {
                Id = yonetici.Id,
                Ad = kisi.Ad,
                Soyad = kisi.Soyad,
                Telefon = kisi.Telefon,
                KisiId = kisi.Id,
                EMail = kisi.EMail,
                UnvanId = unvan.Id,
                Unvan = unvan.Tanim,
                BaslangicTarihi = yonetici.BaslangicTarihi,
                BitisTarihi = yonetici.BitisTarihi
            };
            return yonDTO;
        }
    }
}
