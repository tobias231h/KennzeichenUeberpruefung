using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace KennzeichenUeberpruefung
{
    class Program
    {
        static string[] kuerzel = KuerzelLaden("Material.kuerzelliste.txt");
        static List<Autokennzeichen> moeglicheKennzeichen = new List<Autokennzeichen>();
        static List<List<Autokennzeichen>> richtig = new List<List<Autokennzeichen>>();

        static void Main(string[] args)
        {
            string wort = Console.ReadLine().ToUpper();

            moeglicheKennzeichen = MoeglichkeitenBekommen(wort);

            foreach (var y in moeglicheKennzeichen)
            {
                AddKennzeichen(y);
            }

            Console.WriteLine("\nMögliche Kennzeichen: ");
            foreach (var x in richtig)
            {
                bool geht = true;
                foreach (var y in x)
                {
                    if (!kuerzel.Contains(y.Ortskennung))
                    {
                        geht = false;
                        break;
                    }
                }

                if (geht)
                {
                    foreach (var y in x)
                    {
                        Console.Write(y.Ortskennung + "|" + y.Buchstaben + " ");
                    }
                    Console.Write('\n');
                }
            }

            Console.ReadKey();
        }

        static void AddKennzeichen(Autokennzeichen autokennzeichen)
        {
            if (autokennzeichen.NaechsteKennzeichen.Count == 0)
            {
                RichtigAdden(autokennzeichen);
            }
            else
            {
                foreach (var x in autokennzeichen.NaechsteKennzeichen)
                {
                    AddKennzeichen(x);
                }
            }
        }

        static void RichtigAdden(Autokennzeichen autokennzeichen)
        {
            List<Autokennzeichen> kennzInsg = new List<Autokennzeichen>();

            Autokennzeichen kennzTemp = autokennzeichen;

            kennzInsg.Insert(0, kennzTemp);

            while (kennzTemp.KennzeichenDavor != null)
            {
                kennzTemp = kennzTemp.KennzeichenDavor;
                kennzInsg.Insert(0, kennzTemp);
            }

            if (kennzInsg[kennzInsg.Count - 1].Wort == "")
                richtig.Add(kennzInsg);
        }

        static List<Autokennzeichen> MoeglichkeitenBekommen(string wort, Autokennzeichen davor = null, bool rekursiv = false)
        {
            List<Autokennzeichen> kennzTemp = new List<Autokennzeichen>();

            if (wort.Length < 2)
            {
                return null;
            }

            if (wort.Length >= 2)
            {
                Autokennzeichen autokennzeichen = new Autokennzeichen();
                autokennzeichen.Ortskennung = wort.Substring(0, 1);
                autokennzeichen.Buchstaben = wort.Substring(1, 1);
                autokennzeichen.Wort = wort.Substring(2, (wort.Length - 2));
                autokennzeichen.KennzeichenDavor = davor;

                kennzTemp.Add(autokennzeichen);
            }

            if (wort.Length >= 3)
            {
                Autokennzeichen autokennzeichen = new Autokennzeichen();
                autokennzeichen.Ortskennung = wort.Substring(0, 1);
                autokennzeichen.Buchstaben = wort.Substring(1, 2);
                autokennzeichen.Wort = wort.Substring(3, (wort.Length - 3));
                autokennzeichen.KennzeichenDavor = davor;

                kennzTemp.Add(autokennzeichen);

                Autokennzeichen autokennzeichen2 = new Autokennzeichen();
                autokennzeichen2.Ortskennung = wort.Substring(0, 2);
                autokennzeichen2.Buchstaben = wort.Substring(2, 1);
                autokennzeichen2.Wort = wort.Substring(3, (wort.Length - 3));
                autokennzeichen2.KennzeichenDavor = davor;

                kennzTemp.Add(autokennzeichen2);
            }

            if (wort.Length >= 4)
            {
                Autokennzeichen autokennzeichen = new Autokennzeichen();
                autokennzeichen.Ortskennung = wort.Substring(0, 2);
                autokennzeichen.Buchstaben = wort.Substring(2, 2);
                autokennzeichen.Wort = wort.Substring(4, (wort.Length - 4));
                autokennzeichen.KennzeichenDavor = davor;

                kennzTemp.Add(autokennzeichen);

                Autokennzeichen autokennzeichen2 = new Autokennzeichen();
                autokennzeichen2.Ortskennung = wort.Substring(0, 3);
                autokennzeichen2.Buchstaben = wort.Substring(3, 1);
                autokennzeichen2.Wort = wort.Substring(4, (wort.Length - 4));
                autokennzeichen2.KennzeichenDavor = davor;

                kennzTemp.Add(autokennzeichen2);
            }

            if (wort.Length >= 5)
            {
                Autokennzeichen autokennzeichen = new Autokennzeichen();
                autokennzeichen.Ortskennung = wort.Substring(0, 3);
                autokennzeichen.Buchstaben = wort.Substring(3, 2);
                autokennzeichen.Wort = wort.Substring(5, (wort.Length - 5));
                autokennzeichen.KennzeichenDavor = davor;

                kennzTemp.Add(autokennzeichen);
            }

            for (int i = 0; i < kennzTemp.Count; i++)
            {
                var nextKennzeichen = MoeglichkeitenBekommen(kennzTemp[i].Wort, kennzTemp[i], true);

                if (nextKennzeichen != null)
                {
                    kennzTemp[i].NaechsteKennzeichen.AddRange(nextKennzeichen);
                }
                else if (!rekursiv)
                {
                    moeglicheKennzeichen.Add(kennzTemp[i]);
                }
            }

            return kennzTemp;
        }

        static string[] KuerzelLaden(string dateiName)
        {
            Encoding ev = Encoding.Default;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "KennzeichenUeberpruefung." + dateiName;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }

    class Autokennzeichen
    {
        private string ortskennung;

        public string Ortskennung
        {
            get { return ortskennung; }
            set { ortskennung = value; }
        }

        private string buchstaben;

        public string Buchstaben
        {
            get { return buchstaben; }
            set { buchstaben = value; }
        }

        public Autokennzeichen(string ort, string buchstaben)
        {
            this.Ortskennung = ort;
            this.Buchstaben = buchstaben;
        }

        private string wort;

        public string Wort
        {
            get { return wort; }
            set { wort = value; }
        }

        private List<Autokennzeichen> autokennzeichen = new List<Autokennzeichen>();

        public List<Autokennzeichen> NaechsteKennzeichen
        {
            get { return autokennzeichen; }
            set { autokennzeichen = value; }
        }

        private Autokennzeichen kennzeichenDavor;

        public Autokennzeichen KennzeichenDavor
        {
            get { return kennzeichenDavor; }
            set { kennzeichenDavor = value; }
        }


        public Autokennzeichen()
        {
        }

    }
}
