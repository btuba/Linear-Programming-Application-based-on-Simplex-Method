using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace mp
{
    class Islem
    {
        bool found, flag1;
        public void KisitEkle(string[] txt, List<Kisit> kisitlar)
        {
            Kisit kisit;
            for (int i = 0; i < txt.Length; i++)
            {
                switch (txt[i])
                {
                    case "N":
                        kisit = new Kisit(txt[i + 1], txt[i]);
                        kisitlar.Add(kisit);
                        break;
                    case "L":
                        kisit = new Kisit(txt[i + 1], txt[i]);
                        kisitlar.Add(kisit);
                        break;
                    case "G":
                        kisit = new Kisit(txt[i + 1], txt[i]);
                        kisitlar.Add(kisit);
                        break;
                    case "E":
                        kisit = new Kisit(txt[i + 1], txt[i]);
                        kisitlar.Add(kisit);
                        break;
                    case "UP":
                        kisit = new Kisit(txt[i + 1] + txt[i + 2], txt[i]);
                        kisitlar.Add(kisit);
                        break;
                    case "LO":
                        kisit = new Kisit(txt[i + 1] + txt[i + 2], txt[i]);
                        kisitlar.Add(kisit);
                        break;
                }
            }
        }
        public void DegiskenEkle(string[] txt, List<Kisit> kisitlar, string table)
        {
            found = false;
            for (int i = 1; i < txt.Length; i++)
            {
                foreach (Kisit k in kisitlar)
                {
                    if (txt[i] == k.ad)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false && txt[i] != table)
                {
                    try
                    {
                        Convert.ToDouble(txt[i]);
                    }
                    catch
                    {
                        foreach (Kisit k in kisitlar)
                        {
                            Degisken d = new Degisken();
                            d.ad = txt[i];
                            foreach (Degisken degisken in k.degiskenler)
                            {
                                if (degisken.ad == txt[i])
                                {
                                    flag1 = true;
                                    break;
                                }

                            }
                            if (!flag1 && d.ad != "UP" && d.ad != "LO" && d.ad != "FR" && d.ad.Contains("BND") == false && d.ad.Contains("BV1") == false && d.ad.Contains("BOUND") == false)
                            {
                                if (table == "BOUNDS" && k.ad.Contains(txt[i]) && k.tip == "UP")
                                    d.katsayi = 1;
                                else if (table == "BOUNDS" && k.ad.Contains(txt[i]) && k.tip == "LO")
                                    d.katsayi = -1;
                                k.degiskenler.Add(d);
                            }
                            flag1 = false;
                        }
                    }
                }
                found = false;
            }
            foreach (Kisit kisit in kisitlar)
            {
                if (kisit.tip == "UP" || kisit.tip == "LO")
                {
                    for (int i = 0; i < kisitlar[0].degiskenler.Count; i++)
                    {
                        if (((Degisken)(kisit.degiskenler[i])).ad != ((Degisken)(kisitlar[0].degiskenler[i])).ad)
                        {
                            Degisken yeni = new Degisken();
                            yeni.ad = ((Degisken)(kisitlar[0].degiskenler[i])).ad;
                            yeni.katsayi = 0;
                            kisit.degiskenler.Insert(i, yeni);
                        }
                    }
                }
            }
        }
        public void KatsayiEkle(string[] txt, List<Kisit> kisitlar, string table)
        {
            found = false;
            int kinx = 0;
            string degiskenadi = "";
            double degiskenkatsayi = 0;

            foreach (string x in txt)
            {
                foreach (Kisit k in kisitlar)
                {
                    if (k.ad == x)
                    {
                        found = true;
                        kinx = kisitlar.IndexOf(k);
                        break;
                    }
                }
                if (!found)
                {
                    try
                    {
                        degiskenkatsayi = Convert.ToDouble(x);
                        foreach (Degisken d in ((Kisit)(kisitlar[kinx])).degiskenler)
                        {
                            if (degiskenadi == d.ad)
                            {
                                d.katsayi = degiskenkatsayi;
                                break;
                            }
                        }
                    }
                    catch
                    {
                        if ((string)x != table)
                            degiskenadi = (string)x;
                    }
                }
                found = false;
            }
        }
        public void BoundsRHS(string[] txt, List<Kisit> kisitlar)
        {
            for (int i = 1; i < txt.Length; i += 4)
            {
                foreach (Kisit k in kisitlar)
                {
                    if (k.tip == txt[i] && k.ad == txt[i + 1] + txt[i + 2])
                        ((Degisken)k.degiskenler[k.degiskenler.Count - 1]).katsayi = Convert.ToDouble(txt[i + 3]);

                }
            }
        }
        public void Objsense(string[] txt, Kisit amac)
        {
            if (txt[txt.Length - 1] == "MAX")
                amac.objmax = true;
        }
        public void Free(string[] txt, List<Kisit> kisitlar)
        {
            for (int i = 0; i < txt.Length; i++)
            {
                if (txt[i] == "FR" || txt[i] == "LO")
                {
                    foreach (Kisit k in kisitlar)
                    {
                        Degisken yeni = new Degisken();
                        yeni.ad = "-" + txt[i + 2];
                        yeni.isFR = true;
                        k.degiskenler.Insert(k.degiskenler.Count, yeni);
                    }
                }
            }
            foreach (Kisit k in kisitlar)
            {
                foreach (Degisken d in k.degiskenler)
                {
                    if (d.isFR == true)
                    {
                        foreach (Degisken r in k.degiskenler)
                        {
                            if (d.ad.Contains(r.ad))
                                d.katsayi = r.katsayi;
                        }
                        d.katsayi *= -1;
                    }
                }
            }
        }
        public void Print(List<Kisit> kisitlar, string filename, int sayac)
        {
            FileStream file = new FileStream(filename, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine("....................................................................................................................................");
            writer.Write(sayac + ")");
            foreach (Degisken d in ((Kisit)(kisitlar[0])).degiskenler)
                writer.Write("\t" + d.ad);
            writer.WriteLine("\n");
            foreach (Kisit k in kisitlar)
            {
                writer.Write(k.girendeg);
                foreach (Degisken d in k.degiskenler)
                    writer.Write("\t" + (d.katsayi).ToString("#,#0.0"));
                writer.WriteLine("\n");
            }
            writer.WriteLine("\n");
            foreach (Kisit k in kisitlar)
            {
                if (k.girendegYapay != true)
                {
                    if (k.tip != "N")
                        writer.Write(k.girendeg + "\t");
                    else if (k.tip == "N" || k.tip == "R")
                        writer.Write(k.ad + "\t");
                    writer.Write((((Degisken)k.degiskenler[k.degiskenler.Count - 1]).katsayi).ToString("#,##0.00"));
                    writer.WriteLine("\n");
                }
            }
            writer.Close();
            file.Close();
        }
        public void ConsolePrintf(List<Kisit> kisitlar, int sayac)
        {
            Console.Write(sayac + ")");
            foreach (Degisken d in ((Kisit)(kisitlar[0])).degiskenler)
                Console.Write("\t" + d.ad);
            Console.WriteLine("\n");
            foreach (Kisit k in kisitlar)
            {
                Console.Write(k.girendeg);
                foreach (Degisken d in k.degiskenler)
                    Console.Write("\t" + (d.katsayi).ToString("#,#0.0"));
                Console.WriteLine("\n");
            }
            Console.WriteLine("\n");
            foreach (Kisit k in kisitlar)
            {
                if (k.girendegYapay != true)
                {
                    if (k.tip != "N")
                        Console.Write(k.girendeg+"\t");
                    else if (k.tip == "N" || k.tip == "R")
                        Console.Write(k.ad + "\t");
                    Console.Write((((Degisken)k.degiskenler[k.degiskenler.Count - 1]).katsayi).ToString("#,##0.00"));
                    Console.WriteLine("\n");
                }
            }
        }
    }
}
