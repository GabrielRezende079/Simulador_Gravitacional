using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TrabalhoV01
{
    public partial class Form1 : Form
    {
        private List<Corpo> corpos = new List<Corpo>();
        private Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Nada por enquanto
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var c in corpos)
            {
                e.Graphics.FillEllipse(Brushes.White,
                    (float)c.PosX, (float)c.PosY,
                    c.Raio * 3, c.Raio * 3);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            corpos.Clear();
            for (int i = 0; i < 10; i++) // exemplo: 5 corpos aleatórios
            {
                corpos.Add(new Corpo(
                    nome: "Corpo" + i,
                    massa: rnd.Next(1, 10),
                    raio: rnd.Next(5, 15),
                    posX: rnd.Next(50, panel1.Width - 50),
                    posY: rnd.Next(50, panel1.Height - 50),
                    velX: rnd.Next(-2, 3), // inclui -2, -1, 0, 1, 2
                    velY: rnd.Next(-2, 3)
                ));
            }
            panel1.Invalidate(); // redesenha
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start(); // usa o Timer do Designer
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // salvar configuração inicial (vamos implementar depois)
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // salvar iteração (vamos implementar depois)
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // carregar configuração (vamos implementar depois)
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double G = 50; // constante gravitacional "ajustada" para simulação 2D

            var aceleracoes = new (double ax, double ay)[corpos.Count];

            // 1) Gravidade entre todos os corpos
            for (int i = 0; i < corpos.Count; i++)
            {
                double ax = 0, ay = 0;
                for (int j = 0; j < corpos.Count; j++)
                {
                    if (i == j) continue;

                    double dx = corpos[j].PosX - corpos[i].PosX;
                    double dy = corpos[j].PosY - corpos[i].PosY;
                    double dist2 = dx * dx + dy * dy + 1;
                    double dist = Math.Sqrt(dist2);

                    double F = G * corpos[i].Massa * corpos[j].Massa / dist2;

                    double fx = F * dx / dist;
                    double fy = F * dy / dist;

                    ax += fx / corpos[i].Massa;
                    ay += fy / corpos[i].Massa;
                }
                aceleracoes[i] = (ax, ay);
            }

            // 2) Atualiza velocidades e posições
            for (int i = 0; i < corpos.Count; i++)
            {
                corpos[i].VelX += aceleracoes[i].ax;
                corpos[i].VelY += aceleracoes[i].ay;
                corpos[i].PosX += corpos[i].VelX;
                corpos[i].PosY += corpos[i].VelY;
            }

            // 3) Tratamento de colisões
            for (int i = 0; i < corpos.Count; i++)
            {
                for (int j = i + 1; j < corpos.Count; j++)
                {
                    double dx = corpos[j].PosX - corpos[i].PosX;
                    double dy = corpos[j].PosY - corpos[i].PosY;
                    double dist = Math.Sqrt(dx * dx + dy * dy);

                    if (dist < corpos[i].Raio + corpos[j].Raio) // colisão detectada
                    {
                        Corpo maior = corpos[i].Massa >= corpos[j].Massa ? corpos[i] : corpos[j];
                        Corpo menor = maior == corpos[i] ? corpos[j] : corpos[i];

                        // Conservação do momento
                        double massaTotal = maior.Massa + menor.Massa;
                        double velXfinal = (maior.Massa * maior.VelX + menor.Massa * menor.VelX) / massaTotal;
                        double velYfinal = (maior.Massa * maior.VelY + menor.Massa * menor.VelY) / massaTotal;

                        // Atualiza o corpo maior
                        maior.Massa = massaTotal;
                        maior.VelX = velXfinal;
                        maior.VelY = velYfinal;

                        // Raio cresce com a raiz da massa
                        maior.Raio = (int)(Math.Sqrt(maior.Massa) * 2);

                        // Remove o corpo menor
                        corpos.Remove(menor);

                        break; // evita problemas na lista
                    }
                }
            }

            panel1.Invalidate();
        }

    }

    // Classe Corpo (simplificada por enquanto)
    public class Corpo
    {
        public string Nome { get; set; }
        public double Massa { get; set; }
        public int Raio { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double VelX { get; set; }
        public double VelY { get; set; }
        public double Densidade { get; set; }

        public Corpo(string nome, double massa, int raio,
                     double posX, double posY, double velX, double velY)
        {
            Nome = nome;
            Massa = massa;
            Raio = raio;
            PosX = posX;
            PosY = posY;
            VelX = velX;
            VelY = velY;
            Densidade = massa / (Math.PI * raio * raio); // simplificação 2D
        }
    }
}
