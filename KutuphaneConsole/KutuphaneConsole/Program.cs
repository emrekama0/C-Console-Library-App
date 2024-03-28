using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

class Kitap
{
    public string Baslik { get; set; }
    public string Yazar { get; set; }
    public string ISBN { get; set; }
    public int KopyaSayisi { get; set; }
    public int OduncAlinanKopyalar { get; set; }
}

class Kutuphane
{
    private List<Kitap> kitaplar;

    public Kutuphane()
    {
        kitaplar = new List<Kitap>();
    }

    public void KitapEkle(Kitap kitap)
    {
        kitaplar.Add(kitap);
    }



    //kitap listeleme methodu
    public void KitaplariListele()
    {
        foreach (var kitap in kitaplar)
        {
            Console.WriteLine("Başlık: " + kitap.Baslik);
            Console.WriteLine("Yazar: " + kitap.Yazar);
            Console.WriteLine("ISBN: " + kitap.ISBN);
            Console.WriteLine("Mevcut Kopya Sayısı: " + kitap.KopyaSayisi);
            Console.WriteLine("Ödünç Alınan Kopyalar: " + kitap.OduncAlinanKopyalar);
            Console.WriteLine();
        }
    }
    //kitap arama methodu
    public void KitapAra(string arama)
    {
        bool kitapBulundu = false;

        foreach (var kitap in kitaplar)
        {
            if (kitap.Baslik.Contains(arama) || kitap.Yazar.Contains(arama))
            {
                Console.WriteLine("Başlık: " + kitap.Baslik);
                Console.WriteLine("Yazar: " + kitap.Yazar);
                Console.WriteLine("ISBN: " + kitap.ISBN);
                Console.WriteLine("Mevcut Kopya Sayısı: " + kitap.KopyaSayisi);
                Console.WriteLine("Ödünç Alınan Kopyalar: " + kitap.OduncAlinanKopyalar);
                Console.WriteLine();

                kitapBulundu = true;
            }
        }

        if (!kitapBulundu)
        {
            Console.WriteLine("Aradığınız kriterlere uygun kitap bulunamadı.");
        }
    }
    //ödünç alma methodu
    public void KitapOduncAl(string baslik)
    {
        Kitap kitap = kitaplar.Find(k => k.Baslik.Equals(baslik));

        if (kitap != null)
        {
            if (kitap.KopyaSayisi > kitap.OduncAlinanKopyalar)
            {
                DateTime simdikiTarih = DateTime.Now;
                DateTime iadeTarihi = simdikiTarih.AddDays(15);
                kitap.OduncAlinanKopyalar++;
                kitap.KopyaSayisi--;
                Console.WriteLine("Kitap ödünç alındı: " + kitap.Baslik);
                KaydetOduncKitap(kitap.Baslik);
            }
            else
            {
                Console.WriteLine("Ödünç almak için yeterli kopya bulunmamaktadır.");
            }
        }
        else
        {
            Console.WriteLine("Kitap bulunamadı.");
        }
    }

    public void KaydetOduncKitap(string baslik)
    {
        Kitap kitap = kitaplar.Find(k => k.Baslik.Equals(baslik));

        if (kitap != null)
        {
            using (StreamWriter writer = new StreamWriter("odunc.txt", true))
            {
                writer.WriteLine("Başlık: " + kitap.Baslik);
                writer.WriteLine("Yazar: " + kitap.Yazar);
                writer.WriteLine("ISBN: " + kitap.ISBN);
                writer.WriteLine("Mevcut Kopya Sayısı: " + kitap.KopyaSayisi);
                writer.WriteLine("Ödünç Alınan Tarih: " + DateTime.Now.ToString("dd/MM/yyyy")); // Ödünç alınan tarihi uygun biçimde kaydet
                writer.WriteLine();

                Console.WriteLine("Ödünç alınan kitap bilgileri 'odunc.txt' dosyasına kaydedildi.");
            }
        }
        else
        {
            Console.WriteLine("Kitap bulunamadı.");
        }
    }

    public void ListeleOduncKitaplar()
    {
        Console.WriteLine("Ödünç Alınan Kitaplar:");
        Console.WriteLine("----------------------");

        string[] lines = File.ReadAllLines("odunc.txt");

        foreach (string line in lines)
        {
            Console.WriteLine(line);
        }
    }
    //iade etme methodu
    public void KitapIadeEt(string baslik)
    {
        Kitap kitap = kitaplar.Find(k => k.Baslik.Equals(baslik));

        if (kitap != null)
        {
            if (kitap.OduncAlinanKopyalar > 0)
            {
                kitap.OduncAlinanKopyalar--;
                kitap.KopyaSayisi++;
                Console.WriteLine("Kitap iade edildi: " + kitap.Baslik);
            }
            else
            {
                Console.WriteLine("Ödünç alınmış bir kopya bulunmamaktadır.");
            }
        }
        else
        {
            Console.WriteLine("Kitap bulunamadı.");
        }
    }

   
    //verileri kaydet
    public void VerileriKaydet(string dosyaAdi)
    {
        using (StreamWriter writer = new StreamWriter(dosyaAdi))
        {
            foreach (var kitap in kitaplar)
            {
                writer.WriteLine(kitap.Baslik);
                writer.WriteLine(kitap.Yazar);
                writer.WriteLine(kitap.ISBN);
                writer.WriteLine(kitap.KopyaSayisi);
                writer.WriteLine(kitap.OduncAlinanKopyalar);
                writer.WriteLine();
            }
        }
    }
    // verileri yükle
    public void VerileriYukle(string dosyaAdi)
    {
        if (File.Exists(dosyaAdi))
        {
            using (StreamReader reader = new StreamReader(dosyaAdi))
            {
                string baslik, yazar, isbn, kopyaSayisiStr, oduncAlinanKopyalarStr;
                int kopyaSayisi, oduncAlinanKopyalar;

                while ((baslik = reader.ReadLine()) != null)
                {
                    yazar = reader.ReadLine();
                    isbn = reader.ReadLine();
                    kopyaSayisiStr = reader.ReadLine();
                    oduncAlinanKopyalarStr = reader.ReadLine();

                    if (int.TryParse(kopyaSayisiStr, out kopyaSayisi) && int.TryParse(oduncAlinanKopyalarStr, out oduncAlinanKopyalar))
                    {
                        Kitap kitap = new Kitap
                        {
                            Baslik = baslik,
                            Yazar = yazar,
                            ISBN = isbn,
                            KopyaSayisi = kopyaSayisi,
                            OduncAlinanKopyalar = oduncAlinanKopyalar
                        };

                        kitaplar.Add(kitap);
                    }

                    reader.ReadLine(); 
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Kutuphane kutuphane = new Kutuphane();
        kutuphane.VerileriYukle("kutuphane.txt");

        bool devamEt = true;

        while (devamEt)
        {
            Console.WriteLine("1. Kitap Ekle");
            Console.WriteLine("2. Kitapları Listele");
            Console.WriteLine("3. Kitap Ara");
            Console.WriteLine("4. Kitap Ödünç Al");
            Console.WriteLine("5. Kitap İade Et");
            Console.WriteLine("6. Süresi Geçmiş Kitapları Listele");
            Console.WriteLine("7. Çıkış");
            Console.Write("Seçiminizi yapın (1-7): ");

            string secim = Console.ReadLine();
            Console.WriteLine();

            switch (secim)
            {
                case "1":
                    Console.Clear();
                    Kitap kitap = new Kitap();

                    Console.Write("Kitap Başlığı: ");
                    kitap.Baslik = Console.ReadLine();

                    Console.Write("Yazar: ");
                    kitap.Yazar = Console.ReadLine();

                    Console.Write("ISBN: ");
                    kitap.ISBN = Console.ReadLine();

                    Console.Write("Mevcut Kopya Sayısı: ");
                    int kopyaSayisi;
                    if (int.TryParse(Console.ReadLine(), out kopyaSayisi))
                    {
                        kitap.KopyaSayisi = kopyaSayisi;
                    }

                    kitap.OduncAlinanKopyalar = 0;

                    kutuphane.KitapEkle(kitap);
                    Console.WriteLine("Kitap başarıyla eklendi.\n");
                    break;

                case "2":
                    Console.Clear();
                    kutuphane.KitaplariListele();
                    break;

                case "3":
                    Console.Clear();
                    Console.Write("Arama Terimini Girin: ");
                    string aramaTerimi = Console.ReadLine();
                    Console.WriteLine();
                    kutuphane.KitapAra(aramaTerimi);
                    break;

                case "4":
                    Console.Clear();
                    Console.Write("Ödünç Alınacak Kitabın Başlığını Girin: ");
                    string oduncAlinacakKitap = Console.ReadLine();
                    Console.WriteLine();
                    kutuphane.KitapOduncAl(oduncAlinacakKitap);
                    break;

                case "5":
                    Console.Clear();
                    Console.Write("İade Edilecek Kitabın Başlığını Girin: ");
                    string iadeEdilecekKitap = Console.ReadLine();
                    Console.WriteLine();
                    kutuphane.KitapIadeEt(iadeEdilecekKitap);
                    break;
                case "6":
                    Console.Clear();
                    kutuphane.ListeleOduncKitaplar();
                    break;

                case "7":
                    Console.Clear();
                    kutuphane.VerileriKaydet("kutuphane.txt");
                    devamEt = false;
                    break;

                default:
                    Console.WriteLine("Geçersiz seçim. Tekrar deneyin.\n");
                    break;
            }
        }
    }
}