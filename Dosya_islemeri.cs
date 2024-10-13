using System;
using System.IO;

namespace Math_Game
{
    public class Dosya_islemleri
    {
        private const string filePath = @"Dosya yolunu \Math_Game.txt";// Dosya yolunu buraya yazınız


        // Oyun verilerini kaydet
        public void SaveGameData(int dogruS, int yanlisS, int kalanS, int seviye, int bolum)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(dogruS);
                writer.WriteLine(yanlisS);
                writer.WriteLine(kalanS);
                writer.WriteLine(seviye);
                writer.WriteLine(bolum);
            }
        }

        // Oyun verilerini yükle
        public (int, int, int, int, int) LoadGameData()
        {
            if (!File.Exists(filePath))
            {
                // Dosya yoksa varsayılan değerleri döndür
                return (0, 0, 20, 1, 1);
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < 5)
            {
                // Eksik veri varsa varsayılan değerleri döndür
                return (0, 0, 20, 1, 1);
            }

            int dogruS = int.Parse(lines[0]);
            int yanlisS = int.Parse(lines[1]);
            int kalanS = int.Parse(lines[2]);
            int seviye = int.Parse(lines[3]);
            int bolum = int.Parse(lines[4]);

            return (dogruS, yanlisS, kalanS, seviye, bolum);
        }
    }
}
