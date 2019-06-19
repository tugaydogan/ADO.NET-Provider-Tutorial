using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DernekYonetim.BLL.DTOs
{
    public class YoneticiDTO : KisiDTO
    {
        public int Id { get; set; }
        public int UnvanId { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string Unvan { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Ad, this.Soyad);
        }
    }
}
