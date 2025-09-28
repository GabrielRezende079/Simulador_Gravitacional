using System;
using System.Drawing;
using System.Windows.Forms;

namespace TrabalhoV01
{
    public partial class Form1 : Form
    {
        private Universo universo = new Universo();
        private Persistencia persistencia = new PersistenciaTxt();
        private int contadorIteracoes = 0;
        private string? caminhoIteracoes = null; // nullable

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
            foreach (var c in universo.Corpos)
            {
                e.Graphics.FillEllipse(Brushes.White,
                    (float)(c.PosX - c.Raio),  // centralizar
                    (float)(c.PosY - c.Raio),
                    (float)(c.Raio * 5),       // diâmetro
                    (float)(c.Raio * 5));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            universo.Limpar();
            Random rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                double massa = rnd.Next(5, 20);
                double densidade = rnd.Next(1, 5); // corpos maiores se densidade for menor
                double posX = rnd.Next(50, panel1.Width - 50);
                double posY = rnd.Next(50, panel1.Height - 50);
                double velX = rnd.Next(-2, 3);
                double velY = rnd.Next(-2, 3);

                universo.AdicionarCorpo(new Corpo(
                    $"Corpo{i}", massa, densidade, posX, posY, velX, velY
                ));
            }

            panel1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Arquivo Texto|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                persistencia.SalvarConfiguracao(universo.Corpos, sfd.FileName, iteracoes: 1000, tempo: timer1.Interval);
                MessageBox.Show("Configuração inicial salva com sucesso!");
            }
        }

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

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arquivo Texto|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                universo.Limpar();
                foreach (var corpo in persistencia.CarregarConfiguracao(ofd.FileName))
                {
                    universo.AdicionarCorpo(corpo);
                }
                panel1.Invalidate();
                MessageBox.Show("Configuração carregada com sucesso!");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            universo.Atualizar();
            panel1.Invalidate();

            contadorIteracoes++;
            if (!string.IsNullOrEmpty(caminhoIteracoes))
            {
                persistencia.SalvarIteracao(universo.Corpos, caminhoIteracoes, contadorIteracoes);
            }
        }
    }
}
