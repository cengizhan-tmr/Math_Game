using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Math_Game
{
    public partial class Math_Game : Form
    {
        int DogruS = 0;
        int YanlisS = 0;
        int KalanS = 20; // Toplam sorular
        Timer KalanZaman = new Timer();
        int ZamanS;
        int Seviye = 1; // Başlangıç seviyesi
        int Bolum = 1;
        int BlokSoruSayisi = 5; // Her blokta 5 soru var
        int pasHakki = 1; // Her soru için bir pas hakkı var
        Dictionary<int, bool> PasSorular = new Dictionary<int, bool>(); // Pas geçilen soruları takip eden dictionary

        Dosya_islemleri d_Islemleri = new Dosya_islemleri(); // DataProcessor sınıfı örneği

        public Math_Game(string[] args = null) // string[] türünde bir argüman alacak şekilde güncelliyoruz
        {
            InitializeComponent();
            KalanZaman.Tick += KalanZaman_Tick;
            KalanZaman.Interval = 1000;
            KalanZaman.Start();
            ZamanS = 15 + (Bolum * 5); // Bölüme göre zaman ayarlaması



            // Eğer argümanlar varsa, onları kullanıyoruz
            if (args != null && args.Length > 1 && args[0].ToLower() == "open")
            {
                string cheatCode = args[1].ToLower(); // İkinci argüman hile kodu olarak kullanılıyor
                ApplyCheatCode(cheatCode); // Hile kodunu uygula
            }
            else
            {
                LoadGameData(); // Normal oyun verilerini yükle
            }


            random(); // İlk rastgele sorular oluşturuluyor
        }


        private void ApplyCheatCode(string cheatCode)
        {
            switch (cheatCode)
            {
                case "2":
                    Seviye = 2;
                    Bolum = 1;
                    MessageBox.Show("2. seviyenin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                case "3":
                    Seviye = 3;
                    Bolum = 1;
                    MessageBox.Show("3. seviyenin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                case "4":
                    Seviye = 4;
                    Bolum = 1;
                    MessageBox.Show("4. seviyenin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                case "5":
                    Seviye = 5;
                    Bolum = 1;
                    MessageBox.Show("5. seviyenin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                case "all":
                    Seviye = 5;
                    Bolum = 4;

                    MessageBox.Show("Tüm seviyelerin kilidi açıldı!", "Hile Etkin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisableTimer();
                    break;
                default:
                    MessageBox.Show("Geçersiz hile kodu!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }

            // Güncel bilgileri kullanıcı arayüzüne yansıt
            lblSeviye.Text = $"Seviye= {Seviye}";
            lblBolum.Text = $"Bölüm= {Bolum}";
        }

        private void DisableTimer()
        {
            KalanZaman.Stop(); // Timer'ı durdur
            ZamanS = 0; // Zamanı sıfırla veya istediğin bir süreyi ayarla
            Zaman.Text = $"Kalan Süre: {ZamanS} Saniye"; // Zamanı güncelle
            KalanZaman.Interval = int.MaxValue; // Zamanlayıcıyı işlevsiz hale getir

        }



        private void LoadGameData()
        {

            var (dogru, yanlis, kalan, seviye, bolum) = d_Islemleri.LoadGameData();
            DogruS = dogru;
            YanlisS = yanlis;
            KalanS = kalan;
            Seviye = seviye;
            Bolum = bolum;



            Dogru.Text = DogruS.ToString();
            Yanlis.Text = YanlisS.ToString();
            Kalan.Text = KalanS.ToString();
            lblSeviye.Text = $"Seviye= {Seviye}";
            lblBolum.Text = $"Bölüm= {Bolum}";

            Yildiz();
        }

        private void KalanZaman_Tick(object sender, EventArgs e)
        {
            // Zamanlayıcı durdurulmuşsa, hiçbir işlem yapma
            if (ZamanS <= 0)
            {
                return;
            }

            ZamanS--;
            Zaman.Text = $"Kalan Süre: {ZamanS} Saniye";
            if (ZamanS <= 0)
            {
                KalanZaman.Stop();
                MessageBox.Show($"Süre doldu!\nDoğru sayısı: {DogruS}\nYanlış sayısı: {YanlisS}", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GameOver();
            }

            // Her saniye verileri kaydet
            d_Islemleri.SaveGameData(DogruS, YanlisS, KalanS, Seviye, Bolum);
        }

        private void GameOver()
        {
            DialogResult result = MessageBox.Show("Tekrar oynamak ister misiniz?", "Oyun Bitti", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ResetGame();
            }
            else
            {
                Application.Exit();
            }
        }
        private void ResetGame()
        {
            Seviye = 1;
            DogruS = 0;
            YanlisS = 0;
            PasSorular.Clear();
            KalanS = 20; // Toplam soru sayısı
            ZamanS = 15; // Başlangıç zamanı
            KalanZaman.Start();
            random(); // Yeni sorular oluştur
        }
        public void random()
        {
            // Her yeni sayfa açıldığında PasSorular sıfırlanır.
            PasSorular.Clear();
            Random rnd = new Random();
            char[] sembol = { '+', '-', '*', '/' };

            int maxSayi = 100; // Başlangıçta maksimum sayı 100
            int minSayi = 0;
            // Seviyeye göre maksimum sayıyı ayarlıyoruz
            if (Seviye == 2)
            {
                minSayi = 100;
                maxSayi = 250; // 2. seviyede maksimum 200
            }
            else if (Seviye == 3)
            {
                minSayi = 250;
                maxSayi = 500; // 3. seviyede maksimum 300
            }
            else if (Seviye == 4)
            {
                minSayi = 500;
                maxSayi = 1000; // 4. seviyede maksimum 400
            }
            else if (Seviye == 5)
            {
                minSayi = 1000;
                maxSayi = 2000; // 5. seviyede maksimum 500
            }

            for (int i = 1; i <= BlokSoruSayisi; i++)
            {
                TextBox Sonuclar = (TextBox)this.Controls[$"Sonuc{i}"];
                Sonuclar.Clear();
                Sonuclar.Enabled = true; // Yeni sorular aktif hale getirilir

                int s1 = rnd.Next(minSayi, maxSayi); // Seviye bazlı maksimum sayı aralığı
                int s2 = rnd.Next(minSayi + 1, maxSayi); // 0'dan büyük olması için 1 ile başlıyoruz

                int islemIndex = rnd.Next(0, sembol.Length);
                char islemS = sembol[islemIndex];

                // Bölme işlemi için, sıfır bölünmemesi için s2'yi 1 ile maxSayi arasında alıyoruz.
                if (islemS == '/')
                {
                    // s1'in s2'ye tam bölünebilmesi için, s1'i s2 ile çarpıyoruz.
                    s1 = s2 * rnd.Next(1, maxSayi / s2);
                }

                Label degisken1 = (Label)this.Controls[$"degisken{i}"];
                Label degisken1_1 = (Label)this.Controls[$"degisken{i}_{i}"];
                Label islemLabel = (Label)this.Controls[$"islem{i}"];

                degisken1.Text = s1.ToString();
                degisken1_1.Text = s2.ToString();
                islemLabel.Text = islemS.ToString();
            }

        }
        private void Test_Et_Click(object sender, EventArgs e)
        {

            int i = 1;

            for (i = 1; i <= BlokSoruSayisi; i++)
            {
                Label degisken1 = (Label)this.Controls[$"degisken{i}"];
                Label degisken1_1 = (Label)this.Controls[$"degisken{i}_{i}"];
                Label islemLabel = (Label)this.Controls[$"islem{i}"];
                TextBox cevapTextBox = (TextBox)this.Controls[$"Sonuc{i}"];

                if (cevapTextBox == null)
                {
                    MessageBox.Show($"Cevap için TextBox {i} bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                // Eğer soru daha önce cevaplandıysa, bu soruyu atla
                if (PasSorular.ContainsKey(i) && !PasSorular[i])
                {
                    continue; // Zaten cevaplanmış bir soru, tekrar kontrol edilmez
                }

                if (int.TryParse(cevapTextBox.Text, out int kullaniciCevabi))
                {
                    int sonuc = Hesapla(degisken1.Text, degisken1_1.Text, islemLabel.Text);

                    if (kullaniciCevabi == sonuc)
                    {
                        DogruS++;
                        KalanS--;
                        PasSorular[i] = false; // Soru doğru çözülmüş, pas değil
                    }
                    else
                    {
                        YanlisS++;
                        KalanS--;
                        PasSorular[i] = false; // Soru yanlış çözülmüş, pas değil
                    }

                    // Doğru veya yanlış olsa bile bu TextBox'ı pasif yapıyoruz
                    cevapTextBox.Enabled = false;
                }
                else
                {
                    // Cevaplanmayan sorular varsa pas işlemi kontrolü
                    Pasİslemleri(i, cevapTextBox);
                }
            }

            Dogru.Text = DogruS.ToString();
            Yanlis.Text = YanlisS.ToString();
            Kalan.Text = KalanS.ToString();
            SonrakiSeviye();
        }
        public int Hesapla(string s1Str, string s2Str, string islemStr)
        {
            int s1 = int.Parse(s1Str);
            int s2 = int.Parse(s2Str);
            char islem = islemStr[0];

            int sonuc = 0;
            switch (islem)
            {
                case '+':
                    sonuc = s1 + s2;
                    break;
                case '-':
                    sonuc = s1 - s2;
                    break;
                case '*':
                    sonuc = s1 * s2;
                    break;
                case '/':
                    sonuc = s2 != 0 ? s1 / s2 : 0;
                    break;
            }
            return sonuc;
        }
        public void Pasİslemleri(int i, TextBox cevapTextBox)
        {
            if (!PasSorular.ContainsKey(i))
            {
                PasSorular[i] = true; // Soruyu pas olarak işaretle
                MessageBox.Show($"{i}. soruyu pas geçtiniz. Bu soruya tekrar döneceksiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cevapTextBox.Enabled = true; // Pas geçilen sorunun aktif kalması için
            }
            else
            {
                if (PasSorular[i]) // Pas hakkı bitmişse
                {
                    YanlisS++; // Pas hakkı bittiğinde yanlış kabul edilir
                    MessageBox.Show($"{i}. soruyu pas geçtiniz ve yanlış kabul edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PasSorular[i] = false; // Yanlış olarak kabul edilen pas sorusu çözülmüş sayılır
                    cevapTextBox.Enabled = false; // Yanlış olarak işaretlenen soru artık pasif olur
                }
                else
                {
                    MessageBox.Show($"{i}. soruyu zaten pas geçtiniz ve yanlış kabul edildi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void SonrakiSeviye()
        {
            if (PasSorular.ContainsValue(true))
            {
                TekrarSor(); // Pas geçilen sorular yeniden soruluyor
            }
            else
            {
                // Seviye arttırılıyor
                if (Seviye <= 5)
                {
                    if (Bolum < 4)
                    {
                        Bolum++;
                        ZamanS += 5; // Yeni sorular için süre ekleniyor
                        lblSeviye.Text = $"Seviye= {Seviye}";
                        lblBolum.Text = $"Bölüm= {Bolum}";
                        MessageBox.Show($"Tebrikler! {Seviye}.Seviye {Bolum}.Bölüm'e geçtiniz!", "Seviye Atladı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        random(); // Yeni seviyedeki sorular oluşturuluyor
                        ZamanS = 15 + (Bolum * 5); // Bölüme göre zaman ayarlaması


                    }
                    else
                    {
                        if (DogruS >= 11)
                        {
                            ZamanS += (Bolum * 3) + 15; // Yeni sorular için süre ekleniyor
                            KalanS = 20;
                            DogruS = 0;
                            YanlisS = 0;
                            Seviye++;
                            Bolum = 0;

                        }
                        else
                        {

                            MessageBox.Show($"Yeterli Sayıda Doğrunuz Yok", "Seviye Tekrarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            KalanS = 20;
                            DogruS = 0;
                            YanlisS = 0;
                            Bolum = 0;

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Oyun bitti! Tebrikler tüm seviyeleri tamamladınız!", "Oyun Bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GameOver(); // Oyun bittiğinde
                }
            }
        }
        private void TekrarSor()
        {
            foreach (var soruNo in PasSorular.Keys)
            {
                if (PasSorular[soruNo] && pasHakki > 0) // Sadece pas geçilen sorular ve pas hakkı bitmemişse tekrar sorulur
                {
                    TextBox cevapTextBox = (TextBox)this.Controls[$"Sonuc{soruNo}"];
                    cevapTextBox.Clear(); // Cevapları temizle
                    cevapTextBox.Enabled = true; // Pas geçilen sorular aktif hale getiriliyor
                }
            }
        }
        public void Yildiz()
        {
            if (DogruS >= 11 && DogruS <= 15)
            {
                YildizGoster.Text = "Yıldız= *";
            }
            else if (DogruS >= 16 && DogruS <= 18)
            {
                YildizGoster.Text = "Yıldız= **";
            }
            else if (DogruS >= 19 && DogruS <= 20)
            {
                YildizGoster.Text = "Yıldız= ***";
            }
        }

       
    }
}
