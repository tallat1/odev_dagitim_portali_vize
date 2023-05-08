using odevdagitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace odevdagitim.ViewModel
{
    public class KayitModel
    {
        public string kayitId { get; set; }
        public string kayitOdevId { get; set; }
        public string kayitOgrId { get; set; }
        public string kayitOgretmenId { get; set; }
        public string kayitTarih { get; set; }

        public OgrenciModel ogrBilgi { get; set; }
        public OdevModel odevBilgi { get; set; }
        public OgretmenModel ogretmenBilgi { get; set; }

    }
}