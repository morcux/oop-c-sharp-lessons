namespace Lr1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a, b, c;
            Console.WriteLine("Введіть довжину сторони a:");
            do
            {
                
            } while (!int.TryParse(Console.ReadLine(), out a) || a <= 0);
            Console.WriteLine("Введіть довжину сторони b:");
            do
            {
                
            } while (!int.TryParse(Console.ReadLine(), out b) || b <= 0);
            Console.WriteLine("Введіть довжину сторони c:");
            do
            {
                
            } while (!int.TryParse(Console.ReadLine(), out c) || c <= 0);
            

            Triangle triangle = new Triangle(a, b, c);
            triangle.GetInformation();   
        }
    }
}