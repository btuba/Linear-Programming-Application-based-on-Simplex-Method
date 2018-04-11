using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace mp
{
    class Program
    {
        static void Main(string[] args)
        {
            File.Delete(System.IO.Directory.GetCurrentDirectory() + "\\" + "R.txt");
            File.Delete(System.IO.Directory.GetCurrentDirectory() + "\\" + "S.txt");
            bool flag1 = true;
            string filename = "";
            FileStream fs;
            StreamReader sr;
            string data = "";
            string text = "", text1 = "", text2 = "", text3 = "", text4 = "", text5 = "";
            Stopwatch watch = new Stopwatch();
            watch.Start();
            
            while (flag1)
            {
                try
                { 
                    Console.WriteLine("Dosya adi:");
                    filename = Console.ReadLine();
                    filename = System.IO.Directory.GetCurrentDirectory() +"\\"+filename + ".mps";
                    fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);
                    while (data != null)
                    {
                        text += data;
                        data = sr.ReadLine();
                    }
                    sr.Close();
                    fs.Close();
                    flag1 = false;
                }
                catch
                {
                    Console.WriteLine("Dosya bulunamadi.");
                    flag1 = true;
                }
            }

            int ix = 0;
            try
            {
                ix = text.IndexOf("OBJSENSE");
                text = text.Remove(0, ix);

                ix = text.IndexOf("ROWS");
                text1 = text.Substring(0, ix);
                text = text.Remove(0, ix);
            }
            catch
            {
                ix = text.IndexOf("ROWS");
                text = text.Remove(0, ix);
            }

            ix = text.IndexOf("COLUMNS");
            text2 = text.Substring(0, ix);
            text = text.Remove(0, ix);

            ix = text.IndexOf("RHS");
            text3 = text.Substring(0, ix);
            text = text.Remove(0, ix);

            bool bayrak = false;
            try
            {
                ix = text.IndexOf("BOUNDS");
                text4 = text.Substring(0, ix);
                text = text.Remove(0, ix);
            }
            catch
            {
                ix = text.IndexOf("ENDATA");
                text4 = text.Substring(0, ix);
                text = text.Remove(0, ix);
                bayrak = true;
            }

            if (bayrak == false)
            {
                ix = text.IndexOf("ENDATA");
                text5 = text.Substring(0, ix);
                text = text.Remove(0, ix);
            }

            string[] txt1, txt2, txt3, txt4, txt5;
            txt1 = text1.Split(new[] { '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            txt2 = text2.Split(new[] { '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            txt3 = text3.Split(new[] { '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            txt4 = text4.Split(new[] { '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            txt5 = text5.Split(new[] { '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            List<Kisit> kisitlar = new List<Kisit>();

            Islem islemci = new Islem();
            islemci.KisitEkle(txt2, kisitlar);
            islemci.DegiskenEkle(txt3, kisitlar, "COLUMNS");
            islemci.KatsayiEkle(txt3, kisitlar, "COLUMNS");
            if (txt5.Length != 0)
            {
                islemci.KisitEkle(txt5, kisitlar);
                islemci.DegiskenEkle(txt5, kisitlar, "BOUNDS");
                islemci.Free(txt5, kisitlar);
            }

            foreach (Kisit k in kisitlar)
                k.YapayDegiskenEkle(kisitlar);

            islemci.DegiskenEkle(txt4, kisitlar, "RHS");
            islemci.KatsayiEkle(txt4, kisitlar, "RHS");

            if (txt5.Length != 0)
                islemci.BoundsRHS(txt5, kisitlar);

            foreach (Kisit k in kisitlar)
            {
                if (((Degisken)k.degiskenler[k.degiskenler.Count - 1]).katsayi < 0)
                    ((Degisken)k.degiskenler[k.degiskenler.Count - 1]).katsayi *= -1;
            }

            Kisit r = new Kisit("R", "R");
            r.objmax = false;
            for (int i = 0; i < ((Kisit)(kisitlar[0])).degiskenler.Count; i++)
            {
                Degisken yeni = new Degisken();
                yeni.ad = ((Degisken)(((Kisit)(kisitlar[0])).degiskenler[i])).ad;
                yeni.isR = ((Degisken)(((Kisit)(kisitlar[0])).degiskenler[i])).isR;
                yeni.isS = ((Degisken)(((Kisit)(kisitlar[0])).degiskenler[i])).isS;
                yeni.isFR = ((Degisken)(((Kisit)(kisitlar[0])).degiskenler[i])).isFR;
                yeni.katsayi = 0;

                foreach (Kisit k in kisitlar)
                {
                    if (k.tip == "G" || k.tip == "E" || k.tip == "LO")
                        yeni.katsayi += ((Degisken)(k.degiskenler[i])).katsayi;
                }
                if (yeni.isR == true)
                    yeni.katsayi = 0;
                r.degiskenler.Add(yeni);
            }

            kisitlar.Add(r);

            Kisit z = new Kisit("z", "N");
            Simplex s = new Simplex();
            z = kisitlar[0];
            islemci.Objsense(txt1, z);
            kisitlar.Remove(kisitlar[0]);

            bool flag = false;
            foreach (Degisken d in kisitlar[0].degiskenler)
            {
                if (d.isR == true)
                {
                    flag = true;
                    break;
                }
            }

            int secim = -1, secim1 = 0;
            bool flag2 = true;
            while (flag2)
            {
                try
                {
                    Console.WriteLine("1.Asama -1\n2.Asama -2\nSonuc   -3");
                    secim = Convert.ToInt16(Console.ReadLine());
                    if (secim == 1 || secim == 2 || secim == 3)
                        flag2 = false;
                    else
                        Console.WriteLine("Gecersiz giriş.");
                }
                catch
                {
                    Console.WriteLine("Gecersiz giriş.");
                    flag2 = true;
                }
            }
            if (!flag2 && secim != 3)
            {
                flag2 = true;
                while (flag2)
                {
                    try
                    {
                        Console.WriteLine("Bir iterasyona gitmek için iterasyon sayisini girin.\nSonucu görmek için -  -1");
                        secim1 = Convert.ToInt16(Console.ReadLine());
                        flag2 = false;
                    }
                    catch
                    {
                        Console.WriteLine("Gecersiz giriş.");
                        flag2 = true;
                    }
                }
            }

            //1.asama
            int sayac = 0;
            int keysutun = -10;
            while (flag)
            {
                int test;
                double[] temp = new double[kisitlar[kisitlar.Count - 1].degiskenler.Count];
                int anahtar;

                test = s.AnahtarSutun(kisitlar[kisitlar.Count - 1].degiskenler, !r.objmax, kisitlar);
                if (s.AnahtarSutunElemanlari(test, kisitlar))
                {
                    anahtar = s.AnahtarSatirElemanlari(kisitlar, test);
                    for (int i = 0; i < kisitlar[anahtar].degiskenler.Count; i++)
                        temp[i] = ((Degisken)(kisitlar[anahtar].degiskenler[i])).katsayi;
                    for (int i = 0; i < kisitlar.Count; i++)
                        s.Azalt(kisitlar[i], temp, test);
                    for (int i = 0; i < kisitlar[anahtar].degiskenler.Count; i++)
                        ((Degisken)(kisitlar[anahtar].degiskenler[i])).katsayi = temp[i];
                    sayac++;
                    flag = s.Durma(kisitlar[kisitlar.Count - 1], r.objmax);
                    if (secim == 1 && sayac == secim1)
                        islemci.ConsolePrintf(kisitlar, sayac);
                    islemci.Print(kisitlar, "R.txt", sayac);
                    if (keysutun != anahtar)
                        keysutun = anahtar;
                    else
                        flag = false;
                }
                else
                    flag = false;
            }
 
            kisitlar.Remove(kisitlar[kisitlar.Count - 1]);
            s.Negatif(z);
            kisitlar.Add(z);
            s.RemoveR(kisitlar);

            foreach (Kisit kisit in kisitlar)
            {
                if (kisit.girendegYapay == false)
                    s.TutarliZ(kisitlar[kisitlar.Count - 1], kisit, kisit.girendeg, kisit.girendegYapay);
            }
            flag = true;

            //2.asama
            sayac = 0;
            keysutun = -10;
            while (flag)
            {
                int test;
                double[] temp = new double[kisitlar[kisitlar.Count - 1].degiskenler.Count];
                int anahtar;
                test = s.AnahtarSutun(kisitlar[kisitlar.Count - 1].degiskenler, !z.objmax, kisitlar);
                if (s.AnahtarSutunElemanlari(test, kisitlar))
                {
                    anahtar = s.AnahtarSatirElemanlari(kisitlar, test);
                    for (int i = 0; i < kisitlar[anahtar].degiskenler.Count; i++)
                        temp[i] = ((Degisken)(kisitlar[anahtar].degiskenler[i])).katsayi;
                    for (int i = 0; i < kisitlar.Count; i++)
                        s.Azalt(kisitlar[i], temp, test);
                    for (int i = 0; i < kisitlar[anahtar].degiskenler.Count; i++)
                        ((Degisken)(kisitlar[anahtar].degiskenler[i])).katsayi = temp[i];
                    flag = s.Durma(kisitlar[kisitlar.Count - 1], z.objmax);
                    sayac++;
                    if (secim == 1 && sayac == secim1)
                        islemci.ConsolePrintf(kisitlar, sayac);
                    islemci.Print(kisitlar, "S.txt", sayac);
                    if (keysutun != anahtar)
                        keysutun = anahtar;
                    else
                        flag = false;
                }
                else
                    flag = false;

            }
            if (sayac < secim1)
                Console.WriteLine("Islem daha kisa surdu.");
            Console.WriteLine("\n");
            if (secim1 == -1 || secim == 3)
            {
                watch.Stop();
                foreach (Kisit k in kisitlar)
                {
                    if (k.girendegYapay != true)
                    {
                        if (k.tip != "N")
                            Console.Write(k.girendeg + "\t");
                        else if (k.tip == "N" || k.tip == "R")
                            Console.Write(k.ad + "\t");
                        Console.Write((((Degisken)k.degiskenler[k.degiskenler.Count - 1]).katsayi).ToString("#,##0.00"));
                        Console.WriteLine("\n");
                    }
                }
                Console.WriteLine("Çalisma süresi: " + watch.Elapsed.Hours.ToString() + "." + watch.Elapsed.Minutes.ToString() + "." + watch.Elapsed.Seconds.ToString() + "." + watch.Elapsed.Milliseconds.ToString());

            }        
            Console.ReadKey();
        }
    }
}
