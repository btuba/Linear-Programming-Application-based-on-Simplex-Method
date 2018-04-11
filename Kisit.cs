using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace mp
{
    class Kisit
    {
        public string ad;
        public string girendeg;
        public bool girendegYapay=false;
        public string tip;
        public bool objmax = false;
        public ArrayList degiskenler=new ArrayList();
        char yapaydegadi = 'A';
        public Kisit(string ad,string tip)
        {
            this.ad = ad;
            this.tip = tip; 
        }
        public void YapayDegiskenEkle(List<Kisit> kisitlar)
        {
            char temp;
            switch(this.tip)
            {
                case "L":
                    temp = this.yapaydegadi;
                    foreach(Kisit k in kisitlar)
                    {
                        Degisken yeni = new Degisken();
                        yeni.isS = true;
                        yeni.ad = temp.ToString();
                        k.degiskenler.Add(yeni);
                        k.yapaydegadi++;
                    }
                    ((Degisken)(this.degiskenler[degiskenler.Count - 1])).katsayi = 1;
                    girendeg = temp.ToString();
                    girendegYapay = true;
                    break;
                case "G":
                    temp = this.yapaydegadi;
                    foreach(Kisit k in kisitlar)
                    {
                        Degisken yeni = new Degisken();
                        yeni.isS = true;
                        yeni.ad = temp.ToString();
                        k.degiskenler.Add(yeni);
                        k.yapaydegadi++;
                    }
                    ((Degisken)(this.degiskenler[degiskenler.Count - 1])).katsayi = -1;
                    temp = this.yapaydegadi;
                    girendegYapay = true;
                    foreach(Kisit k in kisitlar)
                    {
                        Degisken yeni = new Degisken();
                        yeni.isR = true;
                        if (k.tip == "N")
                            yeni.katsayi = 19;
                        yeni.ad = temp.ToString();
                        k.degiskenler.Add(yeni);
                        k.yapaydegadi++;
                    }
                    ((Degisken)(this.degiskenler[degiskenler.Count - 1])).katsayi = 1;
                    girendeg = temp.ToString();                  
                    girendegYapay = true;
                    break;
                case "E":
                    temp = this.yapaydegadi;
                    foreach(Kisit k in kisitlar)
                    {
                        Degisken yeni = new Degisken();
                        yeni.isR = true;
                        if (k.tip == "N")
                            yeni.katsayi = 19;
                        yeni.ad = temp.ToString();
                        k.degiskenler.Add(yeni);
                        k.yapaydegadi++;
                    }
                    ((Degisken)(this.degiskenler[degiskenler.Count - 1])).katsayi = 1;
                    girendeg = temp.ToString();
                    girendegYapay = true;
                    break;
                case "UP":
                    temp = this.yapaydegadi;
                    foreach (Kisit k in kisitlar)
                    {
                        Degisken yeni = new Degisken();
                        yeni.isS = true;
                        yeni.ad = temp.ToString();
                        k.degiskenler.Add(yeni);
                        k.yapaydegadi++;
                    }
                    ((Degisken)(this.degiskenler[degiskenler.Count - 1])).katsayi = 1;
                    girendeg = temp.ToString();
                    girendegYapay = true;
                    break;
                case "LO":
                    temp = this.yapaydegadi;
                    foreach (Kisit k in kisitlar)
                    {
                        Degisken yeni = new Degisken();
                        yeni.isS = true;
                        yeni.ad = temp.ToString();
                        k.degiskenler.Add(yeni);
                        k.yapaydegadi++;
                    }
                    ((Degisken)(this.degiskenler[degiskenler.Count - 1])).katsayi = -1;
                    temp = this.yapaydegadi;
                    girendegYapay = true;

                    foreach (Kisit k in kisitlar)
                    {
                        Degisken yeni = new Degisken();
                        yeni.isR = true;
                        if (k.tip == "N")
                            yeni.katsayi = 666;
                        yeni.ad = temp.ToString();
                        k.degiskenler.Add(yeni);
                        k.yapaydegadi++;
                    }
                    ((Degisken)(this.degiskenler[degiskenler.Count - 1])).katsayi = 1;
                    girendeg = temp.ToString();
                    girendegYapay = true;
                    break;
            }
        }
    }
}
