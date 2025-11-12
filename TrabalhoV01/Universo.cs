using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrabalhoV01
{
    public class Universo
    {
        public List<Corpo> Corpos { get; private set; } = new List<Corpo>();
        private double G = 25.0;
        private double scaleFactor = 1.0;
        private int proximoId = 0;
        private readonly object lockObj = new object(); // Para thread-safety

        public void AdicionarCorpo(Corpo c)
        {
            lock (lockObj)
            {
                if (c.Id == 0)
                {
                    c.Id = proximoId++;
                }
                else
                {
                    proximoId = Math.Max(proximoId, c.Id + 1);
                }
                Corpos.Add(c);
            }
        }

        public void Limpar()
        {
            lock (lockObj)
            {
                Corpos.Clear();
                proximoId = 0;
            }
        }

        public void Atualizar()
        {
            int n = Corpos.Count;
            if (n < 2) return;

            var aceleracoes = new (double ax, double ay)[n];

            Parallel.For(0, n, i =>
            {
                double ax = 0.0, ay = 0.0;
                var ci = Corpos[i];

                for (int j = 0; j < n; j++)
                {
                    if (i == j) continue;
                    var cj = Corpos[j];

                    double dx = cj.PosX - ci.PosX;
                    double dy = cj.PosY - ci.PosY;
                    double dist2 = dx * dx + dy * dy + 1e-6;
                    double dist = Math.Sqrt(dist2);

                    double F = G * ci.Massa * cj.Massa / dist2;

                    double fx = F * dx / dist;
                    double fy = F * dy / dist;

                    ax += fx / ci.Massa;
                    ay += fy / ci.Massa;
                }
                aceleracoes[i] = (ax, ay);
            });

            // Atualiza velocidades e posições
            for (int i = 0; i < n; i++)
            {
                Corpos[i].VelX += aceleracoes[i].ax;
                Corpos[i].VelY += aceleracoes[i].ay;
                Corpos[i].PosX += Corpos[i].VelX;
                Corpos[i].PosY += Corpos[i].VelY;
            }

            TratarColisoes();
        }

        private void TratarColisoes()
        {
            lock (lockObj)
            {
                for (int i = 0; i < Corpos.Count; i++)
                {
                    int j = i + 1;
                    while (j < Corpos.Count)
                    {
                        var c1 = Corpos[i];
                        var c2 = Corpos[j];

                        double dx = c2.PosX - c1.PosX;
                        double dy = c2.PosY - c1.PosY;
                        double dist = Math.Sqrt(dx * dx + dy * dy);
                        double rsum = c1.Raio + c2.Raio;

                        if (dist <= rsum)
                        {
                            double massaTotal = c1.Massa + c2.Massa;
                            double xCM = (c1.PosX * c1.Massa + c2.PosX * c2.Massa) / massaTotal;
                            double yCM = (c1.PosY * c1.Massa + c2.PosY * c2.Massa) / massaTotal;
                            double vx = (c1.VelX * c1.Massa + c2.VelX * c2.Massa) / massaTotal;
                            double vy = (c1.VelY * c1.Massa + c2.VelY * c2.Massa) / massaTotal;
                            double dens = (c1.Densidade * c1.Massa + c2.Densidade * c2.Massa) / massaTotal;
                            string nomeNovo = c1.Nome + "+" + c2.Nome;

                            Corpos[i] = new Corpo(proximoId++, nomeNovo, massaTotal, dens, xCM, yCM, vx, vy);
                            Corpos.RemoveAt(j);
                        }
                        else j++;
                    }
                }
            }
        }
    }
}