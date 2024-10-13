using System;
using System.Windows.Forms;

namespace Math_Game
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Komut satırından argümanları kontrol etmek için konsola yazdıralım

            foreach (var arg in args)
            {
                Console.WriteLine(arg); // Tüm argümanları sırayla yazdırıyoruz
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Math_Game(args)); // form2'ye komut satırı argümanlarını geçiriyoruz
        }
    }
}
