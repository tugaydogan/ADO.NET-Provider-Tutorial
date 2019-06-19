using DernekYonetim.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DernekYonetim.UI.ViewModels
{
    public class YoneticiDetayVM
    {
        public YoneticiDTO Yonetici { get; set; }
        public List<MaliHareketDTO> MaliHareketler { get; set; }
    }
}
