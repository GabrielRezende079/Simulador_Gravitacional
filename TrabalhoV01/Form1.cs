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
                e.Graphics.FillEllipse(Brushes.Blue,
                    (float)c.PosX, (float)c.PosY,
                    c.Raio * 2, c.Raio * 2);
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
            double G = 0.10; // constante gravitacional "ajustada" para simulação 2D

            // Lista para armazenar acelerações de cada corpo
            var aceleracoes = new (double ax, double ay)[corpos.Count];

            // Calcula forças gravitacionais (O(n²))
            for (int i = 0; i < corpos.Count; i++)
            {
                double ax = 0, ay = 0;
                for (int j = 0; j < corpos.Count; j++)
                {
                    if (i == j) continue;

                    double dx = corpos[j].PosX - corpos[i].PosX;
                    double dy = corpos[j].PosY - corpos[i].PosY;
                    double dist2 = dx * dx + dy * dy + 1; // +1 evita divisão por zero
                    double dist = Math.Sqrt(dist2);

                    // Força gravitacional
                    double F = G * corpos[i].Massa * corpos[j].Massa / dist2;

                    // Direção normalizada
                    double fx = F * dx / dist;
                    double fy = F * dy / dist;

                    // a = F / m
                    ax += fx / corpos[i].Massa;
                    ay += fy / corpos[i].Massa;
                }
                aceleracoes[i] = (ax, ay);
            }

            // Atualiza velocidade e posição
            for (int i = 0; i < corpos.Count; i++)
            {
                corpos[i].VelX += aceleracoes[i].ax;
                corpos[i].VelY += aceleracoes[i].ay;
                corpos[i].PosX += corpos[i].VelX;
                corpos[i].PosY += corpos[i].VelY;
            }

            panel1.Invalidate(); // força redesenho
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
