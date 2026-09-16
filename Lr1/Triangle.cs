
namespace Lr1
{
    internal class Triangle
    {
        public int a;
        public int b;
        public int c;

        public Triangle(int a, int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public void GetInformation()
        {
            if (!IsPossible())
            {
                Console.WriteLine("Трикутник не існує.");
                return;
            }

            string triangleType = GetTypeString();
            int perimeter = GetPerimeter();
            double area = GetArea();
            Console.WriteLine($"Трикутник існує.\nПериметр: {perimeter}\nПлоща:{area}\nТип трикутника: {triangleType}");

        }

        private bool IsPossible()
        {
            if (a + b > c && a + c > b && b + c > a)
            {
                return true;
            }
            return false;
        }

        private string GetTypeString()
        {
            if (a == b && b == c)
            {
                return "Рівносторонній трикутник";
            }
            if (a == b || a == c || b == c)
            {
                return "Рівнобедрений трикутник";
            }
            if (a * a + b * b == c * c || a * a + c * c == b * b || b * b + c * c == a * a)
            {
                return "Прямокутний трикутник";
            }
            return "Довільний трикутник";
        }

        private int GetPerimeter()
        {
            return a + b + c;
        }

        private double GetArea()
        {
            double p = (a + b + c) / 2.0;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
}