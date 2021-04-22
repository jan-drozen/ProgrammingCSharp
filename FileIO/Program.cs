using System;
using System.IO;
using System.Text;

namespace FileIO
{
    class Program
    {
        static void Main(string[] args)
        {
            using var sr1 = new StreamReader("A1.txt");
            using var sr2 = new StreamReader("A2.txt");
            using var sw = new StreamWriter("output.txt");
            var outputSb = new StringBuilder();
            var tri1 = TryReadInt(sr1, out int number1);
            var tri2 = TryReadInt(sr2, out int number2);            
            do
            {   
                while (number1 <= number2 && tri1)
                {
                    WriteToOutput(number1, outputSb);
                    tri1 = TryReadInt(sr1, out number1);
                }                
                while (number2 <= number1 && tri2)
                {
                    WriteToOutput(number2, outputSb);
                    tri2 = TryReadInt(sr2, out number2);
                }
                if (!(tri1 || tri2))
                    break;
            }
            while (true);
            outputSb.Remove(outputSb.Length - 1, 1);
            sw.Write(outputSb.ToString());
        }

        private static void WriteToOutput(int number, StringBuilder sb)
        {
            sb.Append($"{number} ");
        }

        private static bool TryReadInt(StreamReader sr, out int number)
        {
            number = int.MaxValue;
            if (sr.EndOfStream)
                return false;
            char charRead = (char)sr.Read();
            StringBuilder sb = new StringBuilder();
            do
            {
                sb.Append(charRead);
                charRead = (char)sr.Read();
            }
            while (charRead != ' ' && !sr.EndOfStream);
            if (sr.EndOfStream && char.IsDigit(charRead))
                sb.Append(charRead);
            number = Int32.Parse(sb.ToString().Trim());
            return true;
        }

        static void Task01()
        {
            using var streamReader = new StreamReader("testFile01.txt");
            string lineRead = streamReader.ReadLine();
            var sloupcu = Convert.ToInt32(lineRead);
            Console.WriteLine(sloupcu);
            lineRead = streamReader.ReadLine();
            var radku = Convert.ToInt32(lineRead);
            Console.WriteLine(radku);

            //1
            for (int i = 0; i < radku; i++)
            {
                var radek = streamReader.ReadLine();
                var poziceNaRadku = radek.IndexOfAny(new[] { '>', '<', 'v', '^' });
                Console.WriteLine(radek);
            }

            ////2
            //var bludiste = streamReader.ReadToEnd();
            //Console.WriteLine(bludiste);            

            ////3
            //while (!streamReader.EndOfStream)
            //{
            //    var radek = streamReader.ReadLine();
            //    Console.WriteLine(radek);
            //}

            using (var sw = new StreamWriter("out.txt"))
            {
                sw.Write("asda");
                sw.WriteLine();
                sw.WriteLine("asdasd");
            }
        }
    }
}
