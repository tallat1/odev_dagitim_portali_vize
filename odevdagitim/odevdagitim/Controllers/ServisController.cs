using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using odevdagitim.Models;
using odevdagitim.ViewModel;

namespace odevdagitim.Controllers
{
    public class ServisController : ApiController
    {
        DB03Entities db = new DB03Entities();
        SonucModel sonuc = new SonucModel();


        #region Öğretmen
        [HttpGet]
        [Route("api/ogretmenliste")]
        public List<OgretmenModel> OgretmenListe()
        {
            List<OgretmenModel> liste = db.Ogretmen.Select(x => new OgretmenModel()
            {
                ogretmenId = x.ogretmenId,
                ogretmenAdSoyad = x.ogretmenAdSoyad,
                ogretmenBolumu = x.ogretmenBolumu,
                ogretmenVerOdevSayisi = x.Kayit.Count()

            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/ogretmenbyid/{ogretmenId}")]
        public OgretmenModel OgretmenById(string ogretmenId)
        {
            OgretmenModel kayit = db.Ogretmen.Where(s => s.ogretmenId == ogretmenId).Select(x => new OgretmenModel()
            {
                ogretmenId = x.ogretmenId,
                ogretmenAdSoyad = x.ogretmenAdSoyad,
                ogretmenBolumu = x.ogretmenBolumu,
                ogretmenVerOdevSayisi = x.Kayit.Count()
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/ogretmenekle")]
        public SonucModel OgretmenEkle(OgretmenModel model)
        {
            

            Ogretmen yeni = new Ogretmen();
            yeni.ogretmenId = Guid.NewGuid().ToString();
            yeni.ogretmenAdSoyad = model.ogretmenAdSoyad;
            yeni.ogretmenBolumu = model.ogretmenBolumu;

            db.Ogretmen.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğretmen Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/ogretmenduzenle")]
        public SonucModel OgretmenDuzenle(OgretmenModel model)
        {
            Ogretmen kayit = db.Ogretmen.Where(s => s.ogretmenId == model.ogretmenId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı!";
                return sonuc;
            }

            kayit.ogretmenAdSoyad = model.ogretmenAdSoyad;
            kayit.ogretmenBolumu = model.ogretmenBolumu;


            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğretmen Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ogretmensil/{ogretmenId}")]
        public SonucModel OgretmenSil(string ogretmenId)
        {
            Ogretmen kayit = db.Ogretmen.Where(s => s.ogretmenId == ogretmenId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı!";
                return sonuc;
            }

            if (db.Kayit.Count(c => c.kayitOgrId == ogretmenId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ödev Veren Öğretmen Silinemez!";
                return sonuc;
            }


            db.Ogretmen.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğretmen Silndi";
            return sonuc;
        }
        #endregion

        #region Öğrenci

        [HttpGet]
        [Route("api/ogrenciliste")]
        public List<OgrenciModel> OgrenciListe()
        {
            List<OgrenciModel> liste = db.Ogrenci.Select(x => new OgrenciModel()
            {
                ogrId = x.ogrId,
                ogrNo = x.ogrNo,
                ogrAdsoyad = x.ogrAdsoyad,
                ogrDogTarih = x.ogrDogTarih,
                ogrFoto = x.ogrFoto,
                ogrOdevSayisi = x.Kayit.Count()
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/ogrencibyid/{ogrId}")]
        public OgrenciModel OgrenciById(string ogrId)
        {
            OgrenciModel kayit = db.Ogrenci.Where(s => s.ogrId == ogrId).Select(x => new OgrenciModel()
            {
                ogrId = x.ogrId,
                ogrNo = x.ogrNo,
                ogrAdsoyad = x.ogrAdsoyad,
                ogrDogTarih = x.ogrDogTarih,
                ogrFoto = x.ogrFoto,
                ogrOdevSayisi = x.Kayit.Count()
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/ogrenciekle")]
        public SonucModel OgrenciEkle(OgrenciModel model)
        {
            if (db.Ogrenci.Count(c => c.ogrNo == model.ogrNo) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Öğrenci Numarası Kayıtlıdır!";
                return sonuc;
            }

            Ogrenci yeni = new Ogrenci();
            yeni.ogrId = Guid.NewGuid().ToString();
            yeni.ogrNo = model.ogrNo;
            yeni.ogrAdsoyad = model.ogrAdsoyad;
            yeni.ogrDogTarih = model.ogrDogTarih;
            yeni.ogrFoto = model.ogrFoto;
            db.Ogrenci.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/ogrenciduzenle")]
        public SonucModel OgrenciDuzenle(OgrenciModel model)
        {
            Ogrenci kayit = db.Ogrenci.Where(s => s.ogrId == model.ogrId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı!";
                return sonuc;
            }

            kayit.ogrNo = model.ogrNo;
            kayit.ogrAdsoyad = model.ogrAdsoyad;
            kayit.ogrDogTarih = model.ogrDogTarih;
            kayit.ogrFoto = model.ogrFoto;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ogrencisil/{ogrId}")]
        public SonucModel OgrenciSil(string ogrId)
        {
            Ogrenci kayit = db.Ogrenci.Where(s => s.ogrId == ogrId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı!";
                return sonuc;
            }

            if (db.Kayit.Count(c => c.kayitOgrId == ogrId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ödeve Kayıtlı Öğrenci Silinemez!";
                return sonuc;
            }


            db.Ogrenci.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Silndi";
            return sonuc;
        }


        [HttpPost]
        [Route("api/ogrfotoguncelle")]
        public SonucModel OgrFotoGuncelle(OgrFotoModel model)
        {
            Ogrenci ogr = db.Ogrenci.Where(s => s.ogrId == model.ogrId).SingleOrDefault();
            if (ogr == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı!";
                return sonuc;
            }
            if (ogr.ogrFoto != "profil.png")
            {
                string yol = System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + ogr.ogrFoto);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
            }

            string data = model.fotoData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imgBytes = Convert.FromBase64String(base64);
            string dosyaAdi = ogr.ogrId + model.fotoUzanti.Replace("image/", ".");
            using (var ms = new MemoryStream(imgBytes, 0, imgBytes.Length))
            {
                Image img = Image.FromStream(ms, true);
                img.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + dosyaAdi));
            }
            ogr.ogrFoto = dosyaAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Fotograf Güncellendi";

            return sonuc;
        }


        #endregion

        #region Ders
        [HttpGet]
        [Route("api/dersliste")]
        public List<DersModel> DersListe()
        {
            List<DersModel> liste = db.Ders.Select(x => new DersModel()
            {
                dersId = x.dersId,
                dersAdi = x.dersAdi,
                dersKodu = x.dersKodu,
                dersKredi = x.DersKredi,
                dersOdevSayisi = x.Odev.Count()
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/dersbyid/{dersId}")]
        public DersModel DersById(string dersId)
        {
            DersModel kayit = db.Ders.Where(s => s.dersId == dersId).Select(x => new DersModel()
            {
                dersId = x.dersId,
                dersAdi = x.dersAdi,
                dersKodu = x.dersKodu,
                dersKredi = x.DersKredi,
                dersOdevSayisi = x.Odev.Count()
            }).SingleOrDefault();
            return kayit;
        }
        [HttpPost]
        [Route("api/dersekle")]
        public SonucModel DersEkle(DersModel model)
        {
            if (db.Ders.Count(c => c.dersKodu == model.dersKodu) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Ders Kayıtlıdır!";
                return sonuc;
            }

            Ders yeni = new Ders();
            yeni.dersId = Guid.NewGuid().ToString();
            yeni.dersKodu = model.dersKodu;
            yeni.dersAdi = model.dersAdi;
            yeni.DersKredi = model.dersKredi;
            db.Ders.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Eklendi";

            return sonuc;
        }
        [HttpPut]
        [Route("api/dersduzenle")]
        public SonucModel DersDuzenle(DersModel model)
        {
            Ders kayit = db.Ders.Where(s => s.dersId == model.dersId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.dersKodu = model.dersKodu;
            kayit.dersAdi = model.dersAdi;
            kayit.DersKredi = model.dersKredi;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Düzenlendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/derssil/{dersId}")]
        public SonucModel DersSil(string dersId)
        {
            Ders kayit = db.Ders.Where(s => s.dersId == dersId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Odev.Count(c => c.odevDersId == dersId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İçinde Ödev Olan Ders Silinemez!";
                return sonuc;
            }

            db.Ders.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Silindi";

            return sonuc;
        }
        #endregion

        #region Ödev
        [HttpGet]
        [Route("api/odevliste")]
        public List<OdevModel> OdevListe()
        {
            List<OdevModel> liste = db.Odev.Select(x => new OdevModel()
            {
                odevId = x.odevId,
                odevAdi = x.odevAdi,
                odevDersId = x.odevDersId,
                odevDersAdi = x.Ders.dersAdi
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/odevbyid/{odevId}")]
        public OdevModel OdevById(string odevId)
        {
            OdevModel kayit = db.Odev.Where(s => s.odevId == odevId).Select(x => new OdevModel()
            {
                odevId = x.odevId,
                odevAdi = x.odevAdi,
                odevDersId= x.odevDersId,
                odevDersAdi = x.Ders.dersAdi
            }).SingleOrDefault();
            return kayit;
        }

        [HttpGet]
        [Route("api/odevlistebydersid/{dersId}")]
        public List<OdevModel> OdevListeByDersId(string dersId)
        {
            List<OdevModel> liste = db.Odev.Where(s => s.odevDersId == dersId).Select(x => new OdevModel()
            {
                odevId = x.odevId,
                odevAdi = x.odevAdi,
                odevDersId = x.odevDersId,
                odevDersAdi = x.Ders.dersAdi

            }).ToList();
            return liste;
        }

        [HttpPost]
        [Route("api/odevekle")]
        public SonucModel DersEkle(OdevModel model)
        {
            if (db.Odev.Count(c => c.odevAdi == model.odevAdi &&  c.odevDersId == model.odevDersId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Ödev Kayıtlıdır!";
                return sonuc;
            }

            Odev yeni = new Odev();
            yeni.odevId = Guid.NewGuid().ToString();
            yeni.odevAdi = model.odevAdi;
            yeni.odevDersId = model.odevDersId;
            db.Odev.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ödev Eklendi";

            return sonuc;
        }
        [HttpPut]
        [Route("api/odevduzenle")]
        public SonucModel OdevDuzenle(OdevModel model)
        {
            Odev kayit = db.Odev.Where(s => s.odevId == model.odevId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.odevAdi = model.odevAdi;
            kayit.odevDersId = model.odevDersId;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Düzenlendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/odevsil/{odevId}")]
        public SonucModel OdevSil(string odevId)
        {
            Odev kayit = db.Odev.Where(s => s.odevId == odevId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Kayit.Count(c => c.kayitOdevId == odevId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Öğrenciye Kayıtlı Ödev Silinemez!";
                return sonuc;
            }

            db.Odev.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ödev Silindi";

            return sonuc;
        }
        #endregion

        #region Kayıt

        [HttpGet]
        [Route("api/ogrenciodevliste/{ogrId}")]
        public List<KayitModel> OgrenciOdevListe(string ogrId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitOgrId == ogrId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitOdevId = x.kayitOdevId,
                kayitOgrId = x.kayitOgrId,
                kayitOgretmenId = x.kayitOgretmenId,
                kayitTarih = x.kayitTarih
                
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.ogrBilgi = OgrenciById(kayit.kayitOgrId);
                kayit.odevBilgi = OdevById(kayit.kayitOdevId);
                kayit.ogretmenBilgi = OgretmenById(kayit.kayitOgretmenId);

            }
            return liste;

        }
        [HttpGet]
        [Route("api/odevogrenciliste/{odevId}")]
        public List<KayitModel> OdevOgrencisListe(string odevId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitOdevId == odevId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitOdevId = x.kayitOdevId,
                kayitOgrId = x.kayitOgrId,
                kayitOgretmenId = x.kayitOgretmenId,
                kayitTarih = x.kayitTarih
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.ogrBilgi = OgrenciById(kayit.kayitOgrId);
                kayit.odevBilgi = OdevById(kayit.kayitOdevId);
                kayit.ogretmenBilgi = OgretmenById(kayit.kayitOgretmenId);

            }
            return liste;

        }



        #endregion

        #region Kayıt
        [HttpPost]
        [Route("api/kayitekle")]
        public SonucModel KayitEkle(KayitModel model)
        {
            if (db.Kayit.Count(c => c.kayitOdevId == model.kayitOdevId & c.kayitOgrId == model.kayitOgrId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Öğrenci Odev Kaydı Önceden Yapılmıştır!";
                return sonuc;
            }

            Kayit yeni = new Kayit();
            yeni.kayitId = Guid.NewGuid().ToString();
            yeni.kayitOgrId = model.kayitOgrId;
            yeni.kayitOdevId = model.kayitOdevId;
            yeni.kayitOgretmenId = model.kayitOgretmenId;
            yeni.kayitTarih = DateTime.Now.ToString();

            db.Kayit.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kayıt Eklendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kayitsil/{kayitId}")]
        public SonucModel KayitSil(string kayitId)
        {
            Kayit kayit = db.Kayit.Where(s => s.kayitId == kayitId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }


            db.Kayit.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kayıt Silindi";

            return sonuc;
        }
        #endregion
    }
}
