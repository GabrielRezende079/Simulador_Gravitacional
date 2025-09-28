using System;

namespace TrabalhoV01
{
    public class Corpo
    {
        public string Nome { get; set; }
        public double Massa { get; set; }
        public double Densidade { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double VelX { get; set; }
        public double VelY { get; set; }

        // Raio calculado dinamicamente a partir da massa e densidade
        public double Raio
        {
            get
            {
                double dens = Math.Max(Densidade, 1e-6); // evita divisão por zero
                double area = Massa / dens;
                double raw = Math.Sqrt(area / Math.PI);

                double scaleFactor = 3.0; // fator de escala visual/física
                return raw * scaleFactor;
            }
        }

        public Corpo(string nome, double massa, double densidade,
                     double posX, double posY, double velX, double velY)
        {
            Nome = nome;
            Massa = massa;
            Densidade = densidade;
            PosX = posX;
            PosY = posY;
            VelX = velX;
            VelY = velY;
        }
    }
}
