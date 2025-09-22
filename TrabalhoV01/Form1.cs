using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TrabalhoV01
{
    public partial class Form1 : Form
    {
        private List<Corpo> corpos = new List<Corpo>();
        private Random rnd = new Random();
        private Persistencia persistencia = new PersistenciaTxt();
        private int contadorIteracoes = 0;
        private string caminhoIteracoes = null;

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

        // Botão 1 → Gerar corpos aleatórios
        private void button1_Click(object sender, EventArgs e)
        {
            corpos.Clear();
            for (int i = 0; i < 10; i++)
            {
                corpos.Add(new Corpo(
                    nome: "Corpo" + i,
                    massa: rnd.Next(1, 10),
                    raio: rnd.Next(5, 15),
                    posX: rnd.Next(50, panel1.Width - 50),
                    posY: rnd.Next(50, panel1.Height - 50),
                    velX: rnd.Next(-2, 3),
                    velY: rnd.Next(-2, 3)
                ));
            }
            panel1.Invalidate();
        }

        // Botão 2 → Iniciar simulação
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        // Botão 3 → Salvar configuração inicial
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Arquivo Texto|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                persistencia.SalvarConfiguracao(corpos, sfd.FileName, iteracoes: 1000, tempo: timer1.Interval);
                MessageBox.Show("Configuração inicial salva com sucesso!");
            }
        }

        // Botão 4 → Escolher arquivo para salvar iterações
        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Arquivo Texto|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                contadorIteracoes = 0;
                caminhoIteracoes = sfd.FileName;
                MessageBox.Show("Arquivo para salvar iterações definido!");
            }
        }

        // Botão 5 → Carregar configuração
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arquivo Texto|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                corpos = persistencia.CarregarConfiguracao(ofd.FileName);
                panel1.Invalidate();
                MessageBox.Show("Configuração carregada com sucesso!");
            }
        }

        // Atualização da simulação
        private void timer1_Tick(object sender, EventArgs e)
        {
            double G = 25; // constante gravitacional ajustada

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

                    if (dist < corpos[i].Raio + corpos[j].Raio)
                    {
                        Corpo maior = corpos[i].Massa >= corpos[j].Massa ? corpos[i] : corpos[j];
                        Corpo menor = maior == corpos[i] ? corpos[j] : corpos[i];

                        double massaTotal = maior.Massa + menor.Massa;
                        double velXfinal = (maior.Massa * maior.VelX + menor.Massa * menor.VelX) / massaTotal;
                        double velYfinal = (maior.Massa * maior.VelY + menor.Massa * menor.VelY) / massaTotal;

                        maior.Massa = massaTotal;
                        maior.VelX = velXfinal;
                        maior.VelY = velYfinal;
                        maior.Raio = (int)(Math.Sqrt(maior.Massa) * 2);

                        corpos.Remove(menor);
                        break;
                    }
                }
            }

            panel1.Invalidate();

            // 4) Salvar iterações se arquivo definido
            contadorIteracoes++;
            if (!string.IsNullOrEmpty(caminhoIteracoes))
            {
                persistencia.SalvarIteracao(corpos, caminhoIteracoes, contadorIteracoes);
            }
        }
    }

    // ==============================
    // Classe Corpo
    // ==============================
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

    // ==============================
    // Classe abstrata Persistencia
    // ==============================
    public abstract class Persistencia
    {
        public abstract void SalvarConfiguracao(List<Corpo> corpos, string caminho, int iteracoes, int tempo);
        public abstract List<Corpo> CarregarConfiguracao(string caminho);
        public abstract void SalvarIteracao(List<Corpo> corpos, string caminho, int passo);
    }

    // ==============================
    // Persistência em TXT
    // ==============================
    public class PersistenciaTxt : Persistencia
    {
        public override void SalvarConfiguracao(List<Corpo> corpos, string caminho, int iteracoes, int tempo)
        {
            using (StreamWriter sw = new StreamWriter(caminho))
            {
                sw.WriteLine($"{corpos.Count};{iteracoes};{tempo}");
                foreach (var c in corpos)
                {
                    sw.WriteLine($"{c.Nome};{c.Massa};{c.Raio};{c.PosX};{c.PosY};{c.VelX};{c.VelY}");
                }
            }
        }

        public override List<Corpo> CarregarConfiguracao(string caminho)
        {
            List<Corpo> corpos = new List<Corpo>();
            var linhas = File.ReadAllLines(caminho);
            if (linhas.Length == 0) return corpos;

            string[] cabecalho = linhas[0].Split(';');
            int qtdCorpos = int.Parse(cabecalho[0]);

            for (int i = 1; i <= qtdCorpos && i < linhas.Length; i++)
            {
                string[] dados = linhas[i].Split(';');
                if (dados.Length >= 7)
                {
                    corpos.Add(new Corpo(
                        nome: dados[0],
                        massa: double.Parse(dados[1]),
                        raio: int.Parse(dados[2]),
                        posX: double.Parse(dados[3]),
                        posY: double.Parse(dados[4]),
                        velX: double.Parse(dados[5]),
                        velY: double.Parse(dados[6])
                    ));
                }
            }
            return corpos;
        }

        public override void SalvarIteracao(List<Corpo> corpos, string caminho, int passo)
        {
            using (StreamWriter sw = new StreamWriter(caminho, true)) // append = true
            {
                sw.WriteLine($"Iteração {passo}");
                foreach (var c in corpos)
                {
                    sw.WriteLine($"{c.Nome};{c.Massa};{c.Raio};{c.PosX};{c.PosY};{c.VelX};{c.VelY}");
                }
                sw.WriteLine();
            }
        }
    }
}
