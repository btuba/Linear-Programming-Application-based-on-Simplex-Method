using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace mp
{
    class Simplex
    {
        public int AnahtarSutun(ArrayList degiskenler, bool minmi, List<Kisit> kisitlar)
        {
            int index = 0;
            if (minmi)
            {
                Degisken pivotdeg = (Degisken)degiskenler[0];
                for (int i = 1; i < degiskenler.Count - 2; i++)
                {
                    if (pivotdeg.katsayi < ((Degisken)degiskenler[i]).katsayi)
                    {
                        if (AnahtarSutunElemanlari(i, kisitlar))
                        {
                            pivotdeg = (Degisken)degiskenler[i];
                            index = i;
                        }
                    }
                }
                return index;
            }
            else
            {
                Degisken pivotdeg = (Degisken)degiskenler[0];
                for (int i = 1; i < degiskenler.Count - 2; i++)
                {
                    if (pivotdeg.katsayi > ((Degisken)degiskenler[i]).katsayi)
                    {
                        if (AnahtarSutunElemanlari(i, kisitlar))
                        {
                            pivotdeg = (Degisken)degiskenler[i];
                            index = i;
                        }
                    }
                }
                return index;
            }
        }
        public int AnahtarSatirElemanlari(List<Kisit> kisitlar, int sutun)
        {
            double min = 1000000000;
            int index = 0;
            double sayi;
            foreach (Kisit k in kisitlar)
            {
                if (k.tip != "N" || k.tip != "R")
                {
                    if ((((Degisken)k.degiskenler[k.degiskenler.Count - 1]).katsayi * ((Degisken)k.degiskenler[sutun]).katsayi) <= 0)
                        continue;
                    else
                    {
                        if (min > ((Degisken)k.degiskenler[k.degiskenler.Count - 1]).katsayi / ((Degisken)k.degiskenler[sutun]).katsayi)
                        {
                            min = ((Degisken)k.degiskenler[k.degiskenler.Count - 1]).katsayi / ((Degisken)k.degiskenler[sutun]).katsayi;
                            index = kisitlar.IndexOf(k);
                        }
                    }
                }
            }
            ((Kisit)(kisitlar[index])).girendeg = ((Degisken)(((Kisit)(kisitlar[index])).degiskenler[sutun])).ad;

            if (((Degisken)(((Kisit)(kisitlar[index])).degiskenler[sutun])).isR == true || ((Degisken)(((Kisit)(kisitlar[index])).degiskenler[sutun])).isS == true)
                ((Kisit)(kisitlar[index])).girendegYapay = true;
            else
                ((Kisit)(kisitlar[index])).girendegYapay = false;

            sayi = (((Degisken)(((Kisit)(kisitlar[index])).degiskenler[sutun])).katsayi);
            foreach (Degisken d in ((Kisit)(kisitlar[index])).degiskenler)
                d.katsayi /= sayi;
            return index;
        }
        public void Azalt(Kisit kısıt, double[] anahtarsatirelemanlari, int sutun)
        {
            double[] temp = new double[anahtarsatirelemanlari.Length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = anahtarsatirelemanlari[i];
            for (int i = 0; i < temp.Length; i++)
                temp[i] *= ((Degisken)(kısıt.degiskenler[sutun])).katsayi;
            for (int i = 0; i < kısıt.degiskenler.Count; i++)
                ((Degisken)(kısıt.degiskenler[i])).katsayi -= temp[i];
        }
        public bool Durma(Kisit z, bool max)
        {
            bool flag = false;
            if (max == true)
            {
                for (int i = 0; i < z.degiskenler.Count - 2; i++)
                {
                    if (((Degisken)z.degiskenler[i]).katsayi < 0)
                    {
                        flag = true;
                        break;
                    }
                }
                return flag;
            }
            else
            {

                for (int i = 0; i < z.degiskenler.Count - 2; i++)
                {
                    if (((Degisken)z.degiskenler[i]).katsayi > 0)
                    {
                        flag = true;
                        break;
                    }
                }
                return flag;
            }
        }
        public void Negatif(Kisit z)
        {
            foreach (Degisken d in z.degiskenler)
                d.katsayi *= -1;
        }
        public void RemoveR(List<Kisit> kisitlar)
        {
            foreach (Kisit kisit in kisitlar)
            {
                for (int i = kisit.degiskenler.Count - 1; i >= 0; i--)
                {
                    if (((Degisken)(kisit.degiskenler[i])).isR == true)
                        kisit.degiskenler.RemoveAt(i);
                }
                foreach (Degisken degisken in kisit.degiskenler)
                {
                    if (degisken.isR == true)
                        kisit.degiskenler.Remove(degisken);
                }
            }
        }
        public void TutarliZ(Kisit z, Kisit lim, string girenDegAd, bool girenDegYapay)
        {
            double sayi = 0;
            foreach (Degisken d in z.degiskenler)
            {
                if (d.ad == girenDegAd && !girenDegYapay)
                {
                    sayi = d.katsayi;
                    break;
                }
            }
            for (int i = 0; i < z.degiskenler.Count; i++)
                ((Degisken)z.degiskenler[i]).katsayi -= ((Degisken)lim.degiskenler[i]).katsayi * sayi;
        }
        public bool AnahtarSutunElemanlari(int sutun, List<Kisit> kisitlar)
        {
            bool flag = false;
            for (int i = 0; i < kisitlar.Count - 1; i++)
            {
                if ((((Degisken)(kisitlar[i].degiskenler[kisitlar[i].degiskenler.Count - 1])).katsayi * ((Degisken)(kisitlar[i].degiskenler[sutun])).katsayi) > 0)
                {
                    flag = true;
                    break;
                }
                else
                    flag = false;
            }
            return flag;
        }

    }
}
