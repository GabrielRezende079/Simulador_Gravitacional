using System;
using System.Collections.Generic;

namespace TrabalhoV01
{
    public class Universo
    {
        public List<Corpo> Corpos { get; private set; } = new List<Corpo>();

        private double G = 25; // constante gravitacional "ajustada" para simulação 2D

        public void AdicionarCorpo(Corpo c) => Corpos.Add(c);
        public void Limpar() => Corpos.Clear();

        public void Atualizar()
        {
            if (Corpos.Count < 2) return;

            var aceleracoes = new (double ax, double ay)[Corpos.Count];

            // 1) Calcular forças gravitacionais
            for (int i = 0; i < Corpos.Count; i++)
            {
                double ax = 0, ay = 0;
                for (int j = 0; j < Corpos.Count; j++)
                {
                    if (i == j) continue;

                    double dx = Corpos[j].PosX - Corpos[i].PosX;
                    double dy = Corpos[j].PosY - Corpos[i].PosY;
                    double dist2 = dx * dx + dy * dy + 1;
                    double dist = Math.Sqrt(dist2);

                    double F = G * Corpos[i].Massa * Corpos[j].Massa / dist2;

                    double fx = F * dx / dist;
                    double fy = F * dy / dist;

                    ax += fx / Corpos[i].Massa;
                    ay += fy / Corpos[i].Massa;
                }
                aceleracoes[i] = (ax, ay);
            }

            // 2) Atualizar velocidades e posições
            for (int i = 0; i < Corpos.Count; i++)
            {
                Corpos[i].VelX += aceleracoes[i].ax;
                Corpos[i].VelY += aceleracoes[i].ay;
                Corpos[i].PosX += Corpos[i].VelX;
                Corpos[i].PosY += Corpos[i].VelY;
            }

            // 3) Detectar e tratar colisões com absorção
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
                        // --- centro de massa (posição resultante) ---
                        double massaTotal = c1.Massa + c2.Massa;
                        double xCM = (c1.PosX * c1.Massa + c2.PosX * c2.Massa) / massaTotal;
                        double yCM = (c1.PosY * c1.Massa + c2.PosY * c2.Massa) / massaTotal;

                        // --- velocidade pela conservação do momento linear ---
                        double vx = (c1.VelX * c1.Massa + c2.VelX * c2.Massa) / massaTotal;
                        double vy = (c1.VelY * c1.Massa + c2.VelY * c2.Massa) / massaTotal;

                        // --- densidade média ponderada ---
                        double densidade = (c1.Densidade * c1.Massa + c2.Densidade * c2.Massa) / massaTotal;

                        string nomeNovo = c1.Nome + "+" + c2.Nome;

                        // Substitui o corpo i pelo novo corpo fundido
                        Corpos[i] = new Corpo(nomeNovo, massaTotal, densidade, xCM, yCM, vx, vy);

                        // Remove corpo j
                        Corpos.RemoveAt(j);

                        Console.WriteLine($"Colisão: {c1.Nome} + {c2.Nome} -> {nomeNovo}, massa={massaTotal:0.0}");
                    }
                    else
                    {
                        j++;
                    }
                }
            }
        }
    }
}
