using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Numerics;
using System.Windows.Forms.DataVisualization.Charting;
using AutoItX3Lib;





namespace SesAnalizi
{

    public partial class SesAnalizi : Form
    {

        //SES İLK DEĞER ATAMALARI
        public const int seskalan_uzunluk = 512;
        public const int sesilk44_uzunluk = 44;
        public string sesyolu = "";
        public byte[] ses;
        public byte[] sesilk44 = new byte[sesilk44_uzunluk];
        public byte[] seskalan = new byte[seskalan_uzunluk];

        public byte[] temp = new byte[seskalan_uzunluk];
        public byte[] hammed_signal = new byte[seskalan_uzunluk];
        public Complex[] buffer = new Complex[seskalan_uzunluk];


        //MFCC
        public static double PI = 3.1415926536;
        public static int MfccAdedi = 13;
        public double[] Cep = new double[MfccAdedi];
        public static List<double> MfccKatsayı = new List<double>();
        public static List<double> VqKadınListe = new List<double>();
        public static List<double> VqErkekListe = new List<double>();


        //KADIN VE ERKEK MERKEZ MFCC KATSAYILARI
        public double[] merkezkadın = new double[MfccAdedi];
        public double[] merkezerkek = new double[MfccAdedi];


        public double[] FiltSol = new double[seskalan_uzunluk / 2 + 1];  //Sol katsayısı
        public double[] FiltSag = new double[seskalan_uzunluk / 2 + 1];  //Doğru katsayısı
        public double[] BantGenisligi = new double[FiltNumara + 1];
        public int[] Num = new int[seskalan_uzunluk / 2 + 1];     //Genel olarak her nokta iki bitişik filtreye dahil edilir, burada bu noktayla ilişkili ikinci filtre bulunur
        public double[] Enerji = new double[FiltNumara + 1];
        public double[] yeniEnerji = new double[FiltNumara + 1];
        //Bant enerjisi
        public static int FiltNumara = 40;
        public static int FS = 16;
        public static long FrameUzunluk = 1024;
        //double[] Cep = new double[MfccAdedi]

        public double[] zerocrossing = new double[seskalan_uzunluk];

        //epsilon değeri    
        public double epsi = 0.05;
        //threshold
        public double threshold = 5.0;


        //ortalamalar için örnek sayısı
        public int örneksayisi = 20;


       public double ortkadın = 0.0;
       public double orterkek = 0.0;
       public double eps=0.0;
       public double sorgu = 0.0;
       public int[] sayacdizi = new int[20];
       public int sayac = 0;

        public SesAnalizi()
        {
            InitializeComponent();

        }

        private void SesAnalizi_Load(object sender, EventArgs e)
        {

            panel2.Visible = false;


        }

        private void btn_Sec_Click(object sender, EventArgs e)
        {
            OpenFileDialog oku = new OpenFileDialog();
            if (oku.ShowDialog() == DialogResult.OK)
            {
                FileInfo dosyabilgisi = new FileInfo(oku.FileName);
                sesyolu = dosyabilgisi.FullName;
            }
            panel2.Visible = true;
        }

        private void ByteCevir()
        {
            FileStream fs = new FileStream(sesyolu, FileMode.Open, FileAccess.Read);
            BinaryReader oku = new BinaryReader(fs);
            int chunkID = oku.ReadInt32();
            int fileSize = oku.ReadInt32();
            int riffType = oku.ReadInt32();
            int fmtID = oku.ReadInt32();
            int fmtSize = oku.ReadInt32();
            int fmtCode = oku.ReadInt16();
            int channels = oku.ReadInt16();
            int sampleRate = oku.ReadInt32();
            int fmtAvgBPS = oku.ReadInt32();
            int fmtBlockAlign = oku.ReadInt16();
            int bitDepth = oku.ReadInt16();

            if (fmtSize == 18) {
                int fmtExtraSize = oku.ReadInt16();
                oku.ReadBytes(fmtExtraSize);
            }

            int dataID = oku.ReadInt32();
            int dataSize = oku.ReadInt32();
            ses = oku.ReadBytes(dataSize);

            int s = 1;                   //VERİLERİN LİSTBOX'A EKLENMESİ
            for (int k = 0; k < ses.Length; k++){
                listBox_bytecevir.Items.Add(s + "->   " + ses[k]);
                s++;
            }

            fs.Position = 0;
            fs.Read(sesilk44, 0, sesilk44_uzunluk);
            fs.Position = 44;
            fs.Read(seskalan, 0, seskalan_uzunluk);

            fs.Close();



            foreach (var series in gra.Series)  //GRAFİK ÇİZDİRME İŞLEMİ
            {
                series.Points.Clear();
            }

            for (int i = 0; i < ses.Length; i++)
            {
                gra.Series["deger"].Points.Add(ses[i]);
            }

            int xx = Convert.ToInt32(ses.Length * 0.1);
            gra.ChartAreas[0].AxisX.ScaleView.Size = xx;

        }


        private void btn_bytecevir_Click(object sender, EventArgs e)
        {


            ByteCevir();
            gra.Visible = true;
        }


        private void PreEmphasis()
        {
            byte[] bt1 = seskalan;
            byte[] headerfile = sesilk44;
            byte[] preEmphasis = new byte[seskalan.Length + sesilk44.Length];

            for (int i = 0; i < seskalan.Length; i++)
            {
                if (i == 0)
                    temp[0] = (byte)bt1[0];
                else
                    temp[i] = (byte)(bt1[i] - 0.95 * bt1[i - 1]);  }

            int s = 1;
            for (int k = 0; k < temp.Length; k++)   {
                listBox_preemphasis.Items.Add(s + "->  " + temp[k]);
                s++;
            }

            foreach (var series in gra.Series)
            {
                series.Points.Clear();
            }

            for (int i = 0; i < temp.Length; i++)
            {
                gra.Series["deger"].Points.Add(temp[i]);
            }

            int xx = Convert.ToInt32(temp.Length * 0.1);
            gra.ChartAreas[0].AxisX.ScaleView.Size = xx;

        }
        private void btn_preemphasis_Click(object sender, EventArgs e)
        {
            PreEmphasis();
        }


        private void hamming()//HAMMING ÇERÇEVELEME
        {
            double window = 0;
            for (int j = 0; j < temp.Length; j++){//HAMMING MATEMATİKSEL FORMÜL
                window = 0.54 - 0.46 * Math.Cos((2 * Math.PI * j) / (temp.Length - 1));
                hammed_signal[j] = (byte)(temp[j] * window);
            }
            int s = 1;
            for (int k = 0; k < temp.Length; k++){//LISTBIX3 E EKLENEN YAZILAN KISIMI
                listBox_hamming.Items.Add(s + "->  " + hammed_signal[k]);
                s++;
            }

            double[] A = new double[seskalan_uzunluk];
            for (int i = 0; i < seskalan_uzunluk; i++){
                A[i] = System.Convert.ToDouble(hammed_signal[i]);
            }
            for (int k = 0; k < seskalan_uzunluk; k++) {
                buffer[k] = A[k];
            }
            //Complex[] input = { 5, 0, 20, 0, 0, 0, 0, 0 ,0};
            //FFT(input);
            foreach (var series in gra.Series) {
                series.Points.Clear();
            }
            for (int i = 0; i < A.Length; i++) {
                gra.Series["deger"].Points.Add(hammed_signal[i]);
            }
            int xx = Convert.ToInt32(hammed_signal.Length * 0.1);
            gra.ChartAreas[0].AxisX.ScaleView.Size = xx;
        }

        private void btn_hamming_Click(object sender, EventArgs e)
        {
            hamming();
        }

        private int BitReverse(int n, int bits) {

            int reversedN = n;
            int count = bits - 1;

            n >>= 1;
            while (n > 0) {
                reversedN = (reversedN << 1) | (n & 1);
                count--;
                n >>= 1;
            }
            return ((reversedN << count) & ((1 << bits) - 1));
        }
        
        private void FFT()
        {
            int bits = (int)Math.Log(buffer.Length, 2);
            for (int j = 1; j < buffer.Length / 2; j++) {

                int swapPos = BitReverse(j, bits);
                var temp = buffer[j];
                buffer[j] = buffer[swapPos];
                buffer[swapPos] = temp;
            }

            for (int N = 2; N <= buffer.Length; N <<= 1)
            {          
                for (int i = 0; i < buffer.Length; i += N)
                {
                    for (int k = 0; k < N / 2; k++)
                    {
                        int evenIndex = i + k;
                        int oddIndex = i + k + (N / 2);
                        var even = buffer[evenIndex];
                        var odd = buffer[oddIndex];

                        double term = -2 * Math.PI * k / (double)N;
                        Complex exp = new Complex(Math.Cos(term), Math.Sin(term)) * odd;

                        buffer[evenIndex] = even + exp;
                        buffer[oddIndex] = even - exp;

                    }
                }
            }
            int s = 1;
            for (int k = 0; k < buffer.Length; k++) {

                listBox_fftdonustur.Items.Add(s + "->  " + String.Format("{0:0.00}", buffer[k]));
                s++;
            }
            foreach (var series in gra.Series)
            {
                series.Points.Clear();
                //for(int i = 0; i < buffer.Length; i++) { 
                //series.Points.Clear(buffer[i].Real);
                //series.Points.Clear(buffer[i].Imaginary);
                //}

            }
            for (int i = 0; i < buffer.Length; i++)
            {
                gra.Series["deger"].Points.Add(buffer[i].Real, buffer[i].Imaginary);

            }
            int xx = Convert.ToInt32(buffer.Length * 0.1);
            gra.ChartAreas[0].AxisX.ScaleView.Size = xx;


        }
        private void btn_fftdonustur_Click(object sender, EventArgs e)
        {
            FFT();
        }
        private void btn_dosyasil_Click(object sender, EventArgs e)
        {

            for (int i=0;i<sesilk44_uzunluk;i++)
            {
                sesilk44[i]= 0;
            }

            for (int i = 0; i < seskalan_uzunluk; i++)
            {
                seskalan[i] = 0;
                temp[i]= 0;
                hammed_signal[i] = 0;
                buffer[i] = 0;
                zerocrossing[i] = 0;
            }

            for (int j=0;j<MfccAdedi;j++)
            {
                Cep[j]= 0;
                merkezkadın[j] = 0;
                merkezerkek[j] = 0;
            }

            MfccKatsayı.Clear();
            VqErkekListe.Clear();
            VqKadınListe.Clear();

            for (int j = 0; j < seskalan_uzunluk / 2 + 1; j++)
            {
                FiltSol[j] = 0;
                FiltSag[j] = 0;
                Num[j] = 0;


            }

            for (int j = 0; j < FiltNumara; j++)
            {
                BantGenisligi[j] = 0;
               yeniEnerji[j] = 0;
                Enerji[j] = 0;


            }

            for(int i = 0; i < 20; i++)
            {
                sayacdizi[i] = 0;
            }


            listBox_bytecevir.Items.Clear();
            listBox_preemphasis.Items.Clear();
            listBox_hamming.Items.Clear();
            listBox_fftdonustur.Items.Clear();
            listBox_filterfrekans.Items.Clear();
            listBox_FFFark.Items.Clear();
            listBox_MFCC.Items.Clear();
            listBox_zeroc.Items.Clear();
            label2.Visible = false;
            label3.Visible = false;
            listBox_fftdonustur.Visible = true;
            listBox_zeroc.Visible = false;
            listBox_mindistanceerkek.Items.Clear();
            listBox_mindistancekadın.Items.Clear();
            textBox1.Text = "";
            text_erkekenerjiort.Text = "";
            text_kadınenerjiort.Text = "";
            foreach (var series in gra.Series)
            {
                series.Points.Clear();

            }

            orterkek = 0;
            ortkadın = 0;
            eps = 0;
            sorgu = 0;
            sayac = 0;
       

    }



        private void gra_MouseMove(object sender, MouseEventArgs e)
        {
            label2.Visible = true;
            label3.Visible = true;
            gra.Focus();

            Point mousePoint = new Point(e.X, e.Y);

            gra.ChartAreas[0].CursorX.Interval = 0;
            gra.ChartAreas[0].CursorY.Interval = 0;

            gra.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, true);
            gra.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, true);

            label2.Text = "X pozisyon:" + String.Format("{0:0.00}", gra.ChartAreas[0].AxisX.PixelPositionToValue(e.X));
            label3.Text = "Y pozisyon:" + String.Format("{0:0.00}", gra.ChartAreas[0].AxisY.PixelPositionToValue(e.Y));

            HitTestResult result = gra.HitTest(e.X, e.Y);



        }



        private void InitFltre()
        {
            int i, k;
            double Frekans;
            double[] FiltFrekans = new double[FiltNumara + 2];                                               //40 filtre, 42 filtre bitiş noktası var. Her bir filtrenin sol ve sağ uç noktaları sırasıyla bir önceki ve bir sonraki süzgeçlerin merkez frekansındaki noktalardır
                                                      // Bant genişliği, her bir bitiş noktası arasındaki frekans aralığı

            double endüsük = (double)(400.0 / 3.0);                                                             /*Filtre bankasının en düşük frekansı, ilk bitiş noktası değeri */
            short lineer = 13;                                                                                 /* 1000 Hz'den önceki 13 filtre doğrusal olarak dağıtılır */
            double lineer_uzaklık = (double)(200.0 / 3.0);                                                    /* Bitişik filtre merkezleri arasındaki mesafe 66.6Hz */
            short log = 27;                                                                                    /* 1000Hz'den sonra logaritma doğrusal olarak dağıtılan bir filtre */
            double log_uzaklık = 1.0711703f;                                                                     /* Bitişik filtre sol yarım genişlik oranı */

            for (i = 0; i < lineer; i++)
            {
                FiltFrekans[i] = endüsük + i * lineer_uzaklık;
            }
            for (i = lineer; i < lineer + log + 2; i++)
            {
                FiltFrekans[i] = FiltFrekans[lineer - 1] * (double)Math.Pow(log_uzaklık, i - lineer + 1);
            }
            for (i = 0; i < FiltNumara + 1; i++)
            {
                BantGenisligi[i] = FiltFrekans[i + 1] - FiltFrekans[i];
            }

            int s = 1;
            for (int z = 0; z < FiltFrekans.Length; z++)
            {

                listBox_filterfrekans.Items.Add(s + "->  " + String.Format("{0:0.00}", FiltFrekans[z]));
                s++;
            }

           


            //GRAFİK ÇİZME
            foreach (var series in gra.Series)
            {
                series.Points.Clear();
            }

            for (int W = 0; W < FiltFrekans.Length; W++)
            {
                gra.Series["deger"].Points.Add(FiltFrekans[W]);
            }
            int xx = Convert.ToInt32(FiltFrekans.Length * 0.1);
            gra.ChartAreas[0].AxisX.ScaleView.Size = xx;

            
            
            for (i = 0; i <= (int)seskalan_uzunluk / 2; i++)
            {
                Num[i] = 0;
            }


            // FİLTRELERİN SAĞ VE SOL AYAKLARININ YERLEŞTİRİLMESİ
            bool bBulFilt = false;
            for (i = 0; i <= (int)seskalan_uzunluk / 2; i++)
            {
                Frekans = FS * 1000.0F * i / (double)(seskalan_uzunluk);
                bBulFilt = false;

                for (k = 0; k <= FiltNumara; k++)
                {
                    if (Frekans >= FiltFrekans[k] && Frekans <= FiltFrekans[k + 1])
                    {
                        bBulFilt = true;
                        if (k == FiltNumara)
                        {
                            FiltSol[i] = 0.0F;
                        }
                        else
                        {
                            FiltSol[i] = (Frekans - FiltFrekans[k]) / (double)(BantGenisligi[k]) * 2.0f / (BantGenisligi[k] + BantGenisligi[k + 1]);
                        }

                        if (k == 0)
                        {
                            FiltSag[i] = 0.0F;
                        }
                        else
                        {
                            FiltSag[i] = (FiltFrekans[k + 1] - Frekans) / (double)(BantGenisligi[k]) * 2.0f / (BantGenisligi[k] + BantGenisligi[k - 1]);
                        }

                        Num[i] = k;     // K == FiltNumara olduğunda, ilk FiltNumara filtresi, aslında yoktur. Bu sadece kolaylık sağlamak için, FiltNumara filtreleri bulunduğunu varsayalım.
                                        //Ancak aslında sonuç etkilenmez
                        break;

                    }

                }

                if (!bBulFilt)
                {
                    Num[i] = 0;    //Bu noktada, nokta herhangi bir filtreye ait değildir, çünkü sol ve sağ katsayıların hepsi sıfırdır, bu yüzden sonucu etkilemeden bir filtreye ait olduğu varsayılabilir.Işte buradayım
                                   // İlk filtre olarak ayarlayın.
                    FiltSol[i] = 0.0F;
                    FiltSag[i] = 0.0F;
                }
            }
        }
        /*
         Bant enerjisi, filtre parametrelerinden hesaplanır

         Giriş Parametreleri: * spdata --- önişlemden sonra konuşma sinyali

         * FiltSol --- katsayının solundaki üçgen filtre
         * FiltSag --- sağ katsayılı üçgen filtre
         * Num --- Her filtrenin hangi süzgecin ait olduğuna karar verin


         Çıkış parametreleri: * Enerji --- logaritmik bant enerjisi çıktı
         */
        //Belirli bir frekans bandına ait tüm enerjiyi ekleyin
        // CFiltre(data, FiltSol, FiltSag, Num, Enerji, vecList); veclist ： FFT hesaplanan sonucu Sayı: Her bir sının hangi sınınca ait olduğunu belirler

        private void CFiltre()
        {
            double temp = 0;
            int id, id1, id2;

            for (id = 0; id < FiltNumara; id++)
            {
                Enerji[id] = 0.0F;
            }
            for (id = 0; id <= (int)seskalan_uzunluk / 2; id++)
            {
                temp = buffer[id].Real * buffer[id].Real + buffer[id].Imaginary * buffer[id].Imaginary; //HER FİLTRENİN AYRI AYRI ENERJİSİ ALINIP TOPLANIR
                temp = temp / ((FrameUzunluk / 2) * (FrameUzunluk / 2));
                id1 = Num[id];
                if (id1 == 0)
                    Enerji[id1] = Enerji[id1] + FiltSol[id] * temp;
                if (id1 == FiltNumara)
                    Enerji[id1 - 1] = Enerji[id1 - 1] + FiltSag[id] * temp;
                if ((id1 > 0) && (id1 < FiltNumara))
                {
                    id2 = id1 - 1;
                    Enerji[id1] = Enerji[id1] + FiltSol[id] * temp;
                    Enerji[id2] = Enerji[id2] + FiltSag[id] * temp;
                }
            }
            for (int z1 = 0; z1 < FiltNumara; z1++) {
                yeniEnerji[z1] = Enerji[z1];

            }


            ////ENEERJİ//

            btn_erkekenerjiekle.Visible = true;
            btn_kadınenerjiekle.Visible = true;
            btn_erkekenerjiort.Visible = true;
            btn_kadınenerjiort.Visible = true;
            text_erkekenerjiort.Visible = true;
            text_kadınenerjiort.Visible = true;


            ////////////////////////////////

            listBox_filterfrekans.Visible = true;

            for (id = 0; id < FiltNumara; id++)
            {
                if (Enerji[id] != 0)
                    Enerji[id] = (double)Math.Log10(Enerji[id]);
            }
        }



        private void btn_melfilter_Click(object sender, EventArgs e)
        {
            listBox_FFFark.Visible = false;
            InitFltre();
            CFiltre();
        }



        /*
        MFCC katsayılarını hesapla
        Giriş parametreleri: * Enerji --- Logaritmik bant enerjisi
        */

        private void MFCC()
        {
            int i, j;
            //	double Cep[13];

            for (i = 0; i < MfccAdedi; i++)
            {
                Cep[i] = 0.0f;

                for (j = 0; j < FiltNumara; j++)   //Ayrık kosinüs dönüşümü (DCT)
                {
                    if (j == 0)
                        Cep[i] = Cep[i] + Enerji[j] * (double)Math.Cos(i * (j + 0.5f) * PI / (FiltNumara)) * 10.0f * Math.Sqrt(1 / (double)FiltNumara);
                    else
                        Cep[i] = Cep[i] + Enerji[j] * (double)Math.Cos(i * (j + 0.5f) * PI / (FiltNumara)) * 10.0f * Math.Sqrt(2 / (double)FiltNumara);

                }
                MfccKatsayı.Add(Cep[i]);
            }


            int y = 1;
            for (int z = 0; z < Cep.Length; z++)
            {

                listBox_MFCC.Items.Add(y + "->  " + String.Format("{0:0.00}", Cep[z]));
                y++;
            }



            //GRAFİK ÇİZME
            foreach (var series in gra.Series)
            {
                series.Points.Clear();
            }

            for (int W = 0; W < Cep.Length; W++)
            {
                gra.Series["deger"].Points.Add(Cep[W]);
            }
            int xx = Convert.ToInt32(Cep.Length * 0.1);
            gra.ChartAreas[0].AxisX.ScaleView.Size = xx;



        }

        private void btn_MFCC_Click(object sender, EventArgs e)
        {
            MFCC();
            label4.Visible = true;
            btn_veri_erkek.Visible = true;
            btn_veri_kadın.Visible = true;
        }

        private void btn_zeroc_Click(object sender, EventArgs e)
        {
            listBox_fftdonustur.Visible = false;
            listBox_zeroc.Visible = true;

            for (int i = 0; i < buffer.Length - 1; i++)
            {
                zerocrossing[i] = 0;
                if ((buffer[i].Real * buffer[i + 1].Real) < 0)
                {
                    zerocrossing[i] = buffer[i].Real;
                    listBox_zeroc.Items.Add(i + 1 + "->  " + String.Format("{0:0.00}", zerocrossing[i]));


                }


            }
            //fft
            foreach (var series in gra.Series)
            {

                series.Points.Clear();

            }

            for (int i = 0; i < zerocrossing.Length; i++)
            {
                gra.Series["deger"].Points.Add(buffer[i].Real);
                if (zerocrossing[i] != 0)
                    gra.Series["nokta"].Points.Add(zerocrossing[i]);

            }

            int xx = Convert.ToInt32(buffer.Length * 0.1);
            gra.ChartAreas[0].AxisX.ScaleView.Size = xx;



        }

        private void btn_veri_erkek_Click(object sender, EventArgs e)
        {


            string dosya_yolu = @"C:\Users\burak\Desktop\SES VERİTABANI\erkek\erkek.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
           

            for (int i = 0; i < Cep.Length; i++)
            {
                sw.Write(String.Format("{0:0.00}", Cep[i]));
                sw.Write(" ");
            }
                       
            sw.Flush();            
            sw.Close();
            fs.Close();


            MessageBox.Show("Veritabanı başarıyla eklendi.");

        }

        private void btn_veri_kadın_Click(object sender, EventArgs e)
        {

            string dosya_yolu = @"C:\Users\burak\Desktop\SES VERİTABANI\kadın\kadın.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            


            for (int i = 0; i < Cep.Length; i++)
            {
                sw.Write(String.Format("{0:0.00}", Cep[i]));
                sw.Write(" ");
            }


            sw.Flush();           
            sw.Close();
            fs.Close();
            
            MessageBox.Show("Veritabanı başarıyla eklendi.");
        }

        private void btn_kadın_merkez_Click(object sender, EventArgs e)
        {
            List<Double> line_list = new List<double>();
            foreach (string line in File.ReadLines(@"C:\Users\burak\Desktop\SES VERİTABANI\kadın\kadın.txt"))
            {
                string[] rows = line.Trim().Split(' ');

                foreach (string el in rows)
                {
                    line_list.Add(double.Parse(el.Trim()));
                }
            }
            
 

            for(int i=0;i< MfccAdedi; i++)
            {
                for (int k = i; k < örneksayisi * MfccAdedi; k++)
                {
                   merkezkadın[i] += line_list[k] ;
                    k = k + 12;
                }
                merkezkadın[i] = merkezkadın[i] / örneksayisi;
                

            }

            string dosya_yolu = @"C:\Users\burak\Desktop\SES VERİTABANI\kadın\kadınmerkez.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            

            for (int i = 0; i <merkezkadın.Length; i++)
            {
                sw.Write(String.Format("{0:0.00}", merkezkadın[i]));
                sw.Write(" ");
            }


            //Dosyaya ekleyeceğimiz iki satırlık yazıyı WriteLine() metodu ile yazacağız.
            sw.Flush();
            //Veriyi tampon bölgeden dosyaya aktardık.
            sw.Close();
            fs.Close();
            //İşimiz bitince kullandığımız nesneleri iade ettik.
            MessageBox.Show("kadın ortalaması başarıyla eklendi.");


        }

        private void btn_erkek_merkez_Click(object sender, EventArgs e)
        {
            
            List<Double> line_list = new List<double>();

            foreach (string line in File.ReadLines(@"C:\Users\burak\Desktop\SES VERİTABANI\erkek\erkek.txt"))
            {
                string[] rows = line.Trim().Split(' ');

                foreach (string el in rows)
                {
                    line_list.Add(double.Parse(el.Trim()));
                }
            }


            for (int i = 0; i < MfccAdedi; i++)
            {
                for (int k = i; k < örneksayisi*MfccAdedi; k++)
                {
                    merkezerkek[i] += line_list[k];
                    k = k + 12;
                }
                merkezerkek[i] = merkezerkek[i] / örneksayisi;
                

            }
            string dosya_yolu = @"C:\Users\burak\Desktop\SES VERİTABANI\erkek\erkekmerkez.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            

            for (int i = 0; i < merkezerkek.Length; i++)
            {
                sw.Write(String.Format("{0:0.00}", merkezerkek[i]));
                sw.Write(" ");
            }


            
            sw.Flush();            
            sw.Close();
            fs.Close();

            
            MessageBox.Show("erkek ortalaması başarıyla eklendi.");

        }

         
        //SWAP
        static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }
        private double LevenshteinDistance_Kadın()
        {
            int length1 = merkezkadın.Length;
            int length2 = Cep.Length;
            // Önemsiz durum döndürür - dize uzunluklarındaki fark eşiği aşar
            if (Math.Abs(length1 - length2) > threshold) { return double.MaxValue; }
                        
            if (length1 > length2) {
                Swap(ref Cep, ref merkezkadın);
                Swap(ref length1, ref length2);
            }
            int maxi = length1;
            int maxj = length2;

            double[] dCurrent = new double[maxi + 1];
            double[] dMinus1 = new double[maxi + 1];
            double[] dMinus2 = new double[maxi + 1];
            double[] dSwap;

            for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

            int jm1 = 0, im1 = 0, im2 = -1;
            for (int j = 1; j <= maxj; j++)
            {
                // Döndür
                dSwap = dMinus2;
                dMinus2 = dMinus1;
                dMinus1 = dCurrent;
                dCurrent = dSwap;
                // Başlatma
                double minDistance = double.MaxValue;
                dCurrent[0] = j;
                im1 = 0;
                im2 = -1;

                for (int i = 1; i <= maxi; i++)
                {
                    double cost = ((merkezkadın[im1] - Cep[jm1]) <= epsi) ? 0 : 1; //STRING KARŞILAŞTIRMA FARKLI 
                                                                                 //NUMERIC TE FARKLARA BAKIYOR.
                    double del = dCurrent[im1] + 1;
                    double ins = dMinus1[i] + 1;
                    double sub = dMinus1[im1] + cost;

                    // Minimum değer için 3 tamsayı için en hızlı yürütme
                    double min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

                    if (i > 1 && j > 1 && merkezkadın[im2] == Cep[jm1] && merkezkadın[im1] == Cep[j - 2])
                        min = Math.Min(min, dMinus2[im2] + cost);

                    VqKadınListe.Add(min);

                    listBox_mindistancekadın.Items.Add(min);
                                                           
                    dCurrent[i] = min;
                    if (min < minDistance) { minDistance = min; }
                    im1++;
                    im2++;

                    
                    
                }
                
                jm1++;
                
                if (minDistance > threshold) { return double.MaxValue; }
                
            }
            
            double result = dCurrent[maxi];
            return (result > threshold) ? double.MaxValue: result;
            

        }
        private double LevenshteinDistance_Erkek()
        {

            int length1 = merkezerkek.Length;
            int length2 = Cep.Length;


            // Önemsiz durum döndürür - dize uzunluklarındaki fark eşiği aşar
            if (Math.Abs(length1 - length2) > threshold) { return double.MaxValue; }


            // Dizilerin olduğundan emin olun [i] / length1 daha kısa bir uzunluk kullanın
            if (length1 > length2)
            {
                Swap(ref Cep, ref merkezerkek);
                Swap(ref length1, ref length2);
            }

            int maxi = length1;
            int maxj = length2;

            double[] dCurrent = new double[maxi + 1];
            double[] dMinus1 = new double[maxi + 1];
            double[] dMinus2 = new double[maxi + 1];
            double[] dSwap;

            for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

            int jm1 = 0, im1 = 0, im2 = -1;

            for (int j = 1; j <= maxj; j++)
            {

                // Döndür
                dSwap = dMinus2;
                dMinus2 = dMinus1;
                dMinus1 = dCurrent;
                dCurrent = dSwap;

                // Başlatma
                double minDistance = double.MaxValue;
                dCurrent[0] = j;
                im1 = 0;
                im2 = -1;

                for (int i = 1; i <= maxi; i++)
                {

                    double cost = ((merkezerkek[im1] - Cep[jm1]) <= epsi) ? 0 : 1;

                    
                    double del = dCurrent[im1] + 1;
                    double ins = dMinus1[i] + 1;
                    double sub = dMinus1[im1] + cost;

                    // Minimum değer için 3 tamsayı için en hızlı yürütme
                    double min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);


                    if (i > 1 && j > 1 && merkezerkek[im2] == Cep[jm1] && merkezerkek[im1] == Cep[j - 2])
                        min = Math.Min(min, dMinus2[im2] + cost);

                    VqErkekListe.Add(min);
                    listBox_mindistanceerkek.Items.Add(min);


                    dCurrent[i] = min;
                    if (min < minDistance) { minDistance = min; }
                    im1++;
                    im2++;

                    
                    

                }

                jm1++;

                if (minDistance > threshold) { return double.MaxValue; }

            }

            double result = dCurrent[maxi];
            return (result > threshold) ? double.MaxValue : result;
            


        }
        private void btn_Vq_Click(object sender, EventArgs e)
        {
            LevenshteinDistance_Kadın();
            LevenshteinDistance_Erkek();

            if (VqErkekListe[VqErkekListe.Count - 1] < VqKadınListe[VqKadınListe.Count - 1])
            {
                MessageBox.Show("ANALİZİ YAPILAN SES ERKEK VERİ UZAYINA" +"*"+ VqErkekListe[VqErkekListe.Count - 1]+"*" + "br. uzaklığındadır." +
                    "KADIN VERİ UZAYINA " + "*"+VqKadınListe[VqKadınListe.Count - 1]+"*" + "br. uzaklığındadır. ");
                MessageBox.Show("ERKEK");
                textBox1.Text = VqErkekListe[VqErkekListe.Count - 1].ToString();
            }
            else if (VqKadınListe[VqKadınListe.Count - 1] < VqErkekListe[VqErkekListe.Count - 1])
            {
                MessageBox.Show("ANALİZİ YAPILAN SES ERKEK VERİ UZAYINA" + "*" + VqErkekListe[VqErkekListe.Count - 1] + "*" + "br. uzaklığındadır." +
                     "KADIN VERİ UZAYINA " + "*" + VqKadınListe[VqKadınListe.Count - 1] + "*" + "br. uzaklığındadır. ");
                MessageBox.Show("KADIN");
                textBox1.Text = VqKadınListe[VqKadınListe.Count - 1].ToString();
            }

            else MessageBox.Show("Eşit uzaklıktalar"); 

        }

      

        private void btn_bantgenlisligi_Click(object sender, EventArgs e)
        {


            listBox_filterfrekans.Visible = false;
            listBox_FFFark.Visible = true;
            int y = 1;
            for (int z = 0; z < BantGenisligi.Length; z++)
            {

                listBox_FFFark.Items.Add(y + "->  " + String.Format("{0:0.00}", BantGenisligi[z]));
                y++;
            }
        }

        private void btn_erkekenerjiekle_Click(object sender, EventArgs e)
        {
            double toplam = 0.0;
            foreach (var series in gra.Series)
            {
                series.Points.Clear();
            }

            for (int c = 0; c < yeniEnerji.Length; c++)
            {
                toplam = toplam + yeniEnerji[c];

            }

            string dosya_yolu = @"C:\Users\burak\Desktop\SES VERİTABANI\erkek\erkekenerji.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);



            sw.Write(String.Format("{0:0.00}", toplam));
            sw.Write(" ");


            sw.Flush();
            sw.Close();
            fs.Close();
            MessageBox.Show("Enerji toplamı" + toplam + ".Enerji veritabanına eklendi.");






            for (int W = 0; W < yeniEnerji.Length; W++)
            {
                gra.Series["deger"].Points.Add(yeniEnerji[W]);
            }

            int xx = Convert.ToInt32(Enerji.Length * 0.1);
            gra.ChartAreas[0].AxisX.ScaleView.Size = xx;
        }

        private void btn_kadınenerjiekle_Click(object sender, EventArgs e)
        {
            double toplam = 0.0;
            foreach (var series in gra.Series)
            {
                series.Points.Clear();
            }

            for (int c = 0; c < yeniEnerji.Length; c++)
            {
                toplam = toplam + yeniEnerji[c];

            }

            string dosya_yolu = @"C:\Users\burak\Desktop\SES VERİTABANI\kadın\kadınenerji.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);



            sw.Write(String.Format("{0:0.00}", toplam));
            sw.Write(" ");


            sw.Flush();
            sw.Close();
            fs.Close();
            MessageBox.Show("Enerji toplamı" + toplam + ".Enerji veritabanına eklendi.");






            for (int W = 0; W < yeniEnerji.Length; W++)
            {
                gra.Series["deger"].Points.Add(yeniEnerji[W]);
            }

            int xx = Convert.ToInt32(Enerji.Length * 0.1);
            gra.ChartAreas[0].AxisX.ScaleView.Size = xx;
        }

        private void btn_erkekenerjiort_Click(object sender, EventArgs e)
        {
            List<Double> line_listerkek = new List<double>();

            foreach (string line in File.ReadLines(@"C:\Users\burak\Desktop\SES VERİTABANI\erkek\erkekenerji.txt"))
            {
                string[] rows = line.Trim().Split(' ');

                foreach (string el in rows)
                {
                    line_listerkek.Add(double.Parse(el.Trim()));
                }
            }

            

            for (int i = 0; i < örneksayisi; i++)
            {               
                    orterkek += line_listerkek[i];
                 
            }


            orterkek = orterkek / örneksayisi;

            text_erkekenerjiort.Text = orterkek.ToString();

            
  

            string dosya_yolu = @"C:\Users\burak\Desktop\SES VERİTABANI\erkek\erkekenerjiortalama.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

                sw.Write(String.Format("{0:0.00}", orterkek));
                sw.Write(" ");

            sw.Flush();
            sw.Close();
            fs.Close();


            MessageBox.Show("erkek enerji ortalaması hesaplandı.");
            line_listerkek.Clear();
            
        }

        private void btn_kadınenerjiort_Click(object sender, EventArgs e)
        {
            List<Double> line_listkadın = new List<double>();

            foreach (string line in File.ReadLines(@"C:\Users\burak\Desktop\SES VERİTABANI\kadın\kadınenerji.txt"))
            {
                string[] rows = line.Trim().Split(' ');

                foreach (string el in rows)
                {
                    line_listkadın.Add(double.Parse(el.Trim()));
                }
            }

            

            for (int i = 0; i < örneksayisi; i++)
            {
                ortkadın += line_listkadın[i];

            }

            ortkadın = ortkadın / örneksayisi;

            text_kadınenerjiort.Text = ortkadın.ToString();


            
          

            string dosya_yolu = @"C:\Users\burak\Desktop\SES VERİTABANI\kadın\kadınenerjiortalama.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(String.Format("{0:0.00}", ortkadın));
            sw.Write(" ");

            sw.Flush();
            sw.Close();
            fs.Close();


            MessageBox.Show("kadın enerji ortalaması hesaplandı.");
            line_listkadın.Clear();
            
           
        }

        private void btn_eps_Click(object sender, EventArgs e)
        {

            List<Double> line_listkadın = new List<double>();

            foreach (string line in File.ReadLines(@"C:\Users\burak\Desktop\SES VERİTABANI\kadın\kadınenerji.txt"))
            {
                string[] rows = line.Trim().Split(' ');

                foreach (string el in rows)
                {
                    line_listkadın.Add(double.Parse(el.Trim()));
                }
            }
            List<Double> line_listerkek = new List<double>();

            foreach (string line in File.ReadLines(@"C:\Users\burak\Desktop\SES VERİTABANI\erkek\erkekenerji.txt"))
            {
                string[] rows = line.Trim().Split(' ');

                foreach (string el in rows)
                {
                    line_listerkek.Add(double.Parse(el.Trim()));
                }
            }
            sorgu = (ortkadın + orterkek) / 2;


            sorgu = sorgu - 1;                              
            for (int i=0;i<20;i++)
            {
                sayac = 0;
                for (int k = 0; k < örneksayisi; k++)
            {
                if (line_listerkek[k] > sorgu)
                {
                    sayac++;
                }

                if (line_listkadın[k] < sorgu)
                {
                    sayac++;
                }

            }
                sayacdizi[i] = sayac;
                sorgu = sorgu + 0.1;
            }
            double max=0;
            int adım = 0;
            for(int z=0;z<20;z++)
            {
                 if(sayacdizi[z]>max){
                    max = sayacdizi[z];
                    adım = z;
                 }

            }
             sorgu = (sorgu-(0.1*örneksayisi)) + ((adım+1) * 0.1);
                       
            MessageBox.Show("EN BÜYÜK EŞLEŞME "+sorgu+" DEĞERİNDE OLMUŞTUR."
                    +örneksayisi*2+"'de "+max+" kadardır");
            line_listerkek.Clear();
            line_listkadın.Clear();          
        }

        private void btn_enerjisonuc_Click(object sender, EventArgs e)
        {
            double toplam = 0.0;
            foreach (var series in gra.Series)
            {
                series.Points.Clear();
            }

            for (int c = 0; c < yeniEnerji.Length; c++)
            {
                toplam = toplam + yeniEnerji[c];

            }

            if(sorgu>toplam)
            {
                MessageBox.Show("CEVAP:KADIN " + "Enerjisi: "+toplam);
            }
            else if(toplam>sorgu)
            {
                MessageBox.Show("CEVAP:ERKEK " + "Enerjisi: " + toplam);
            }
            else
            {
                MessageBox.Show("CEVAP:yok " + "Enerjisi eşit: " + toplam);
            }

        }
    }
}


