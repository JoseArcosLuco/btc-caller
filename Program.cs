using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        string previousHash = "6794ff4510f818785e38c793a51a70d54edea5a3a8f55a4c4fb108316e16d4fe";
        string transactions = "Transacción1;Transacción2;Transacción3";
        int nonce = 0;
        string hash;
        DateTime inicio = DateTime.Now;

        Console.WriteLine($"Iniciando minería simulada...{inicio}");

        do
        {
            nonce++;
            string data = previousHash + transactions + nonce.ToString();
            hash = CalculateSHA256(data);
            Console.WriteLine($"¡Minando! Nonce: {nonce}, Hash: {hash}");

            if (nonce % 100000 == 0)
            {
                Console.WriteLine($"Intentos: {nonce}, Hash actual: {hash}");
            }

        } while (!hash.StartsWith("0000")); // Dificultad simulada
        DateTime fin = DateTime.Now;
        Int64 diff = inicio.Minute - fin.Minute;
        
        TimeSpan ts = fin.Subtract(inicio);
        Console.WriteLine("Tiempo trancurrido {0} Days, {1} Hours, {2} Minutes, {3} Seconds, {4} Milliseconds",
                        ts.Days, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        Console.WriteLine($"¡Bloque minado! Nonce: {nonce}, Hash: {hash} , fechahorainicio: {inicio} , fechahorafin: {fin}");

        // Informar que el hash fue resuelto
        //await InformarHashResuelto(hash, nonce);
    }

    static string CalculateSHA256(string data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    static async Task InformarHashResuelto(string hash, int nonce)
    {
        string url = "https://api.ejemplo.com/submit-block"; // URL ficticia
        var datos = new
        {
            hash = hash,
            nonce = nonce,
            minero = "MiIdentificador"
        };

        using (var client = new HttpClient())
        {
            var contenido = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "application/json");

            try
            {
                var respuesta = await client.PostAsync(url, contenido);
                if (respuesta.IsSuccessStatusCode)
                {
                    Console.WriteLine("Hash resuelto informado con éxito a la red.");
                }
                else
                {
                    Console.WriteLine($"Error al informar el hash resuelto. Código: {respuesta.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la red: {ex.Message}");
            }
        }
    }
}
