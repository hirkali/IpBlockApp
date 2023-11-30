using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Dosya yolu girin: ");
        var settingsFilePath = "ayarlar.txt";
        var filePath = "";
        if (File.Exists(settingsFilePath))
        {
            var lastUsedFilePath = File.ReadAllText(settingsFilePath).Trim();
            if (!string.IsNullOrEmpty(lastUsedFilePath) && File.Exists(lastUsedFilePath))
            {
                Console.Write("Son kullanılan dosya yolu olan '" + lastUsedFilePath + "' kullanmak ister misiniz? (y/n): ");
                var response = Console.ReadLine();
                if (response.ToUpper() == "Y")
                {
                    filePath = lastUsedFilePath;
                }
            }
        }
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = Console.ReadLine().Trim();
            File.WriteAllText(settingsFilePath, filePath);
        }

        string lastDate = string.Empty;
        while (true)
        {
            Console.Write("Eklemek istediğiniz IP adresini girin: ");
            var ipAddress = Console.ReadLine();

            // IP adresine /32 eklenir
            ipAddress += "/32";

            try
            {
                var lines = File.ReadAllLines(filePath);
                var date = DateTime.Now.ToString("yyyy-MM-dd");

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    if (!lines.Contains(ipAddress))
                    {
                        // yeni gün başladıysa, tarih başlığını ekle
                        if (lastDate != date)
                        {
                            lastDate = date;
                            sw.WriteLine();
                            sw.WriteLine($"##{date}");
                        }
                        sw.WriteLine(ipAddress);
                    }
                }

                if (!lines.Contains(ipAddress))
                {
                    Console.WriteLine("IP adresi başarıyla eklendi.");
                }
                else
                {
                    Console.WriteLine("Bu IP adresi zaten listede.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
            }

            Console.Write("Başka bir IP adresi eklemek ister misiniz? (y/n): ");
            var response = Console.ReadLine();
            if (response.ToUpper() == "N")
            {
                break;
            }
        }
    }
}