using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace TrabalhoV01
{
    public partial class Form1 : Form
    {
        private Universo universo = new Universo();
        private PersistenciaMySql persistencia = new PersistenciaMySql();
        private int contadorIteracoes = 0;
        private bool simulando = false;
        private const int INTERVALO_SALVAMENTO = 50; // Salva a cada 50 iterações

public Form1()
{
    InitializeComponent();
    this.DoubleBuffered = true; // ativa no próprio formulário
    EnableDoubleBuffering(panel1); // ativa via reflexão no painel
}
private void EnableDoubleBuffering(Panel panel)
{
    typeof(Panel).InvokeMember("DoubleBuffered",
        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
        null, panel, new object[] { true });
}
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Cria uma cópia thread-safe da lista
            List<Corpo> corposCopia = universo.Corpos.ToList();

            foreach (var c in corposCopia)
            {
                e.Graphics.FillEllipse(Brushes.LightBlue,
                    (float)(c.PosX - c.Raio),
                    (float)(c.PosY - c.Raio),
                    (float)(c.Raio * 2),
                    (float)(c.Raio * 2));
            }
        }

  private void button1_Click(object sender, EventArgs e)
{
    universo.Limpar();
    Random rnd = new Random();

    // usa os valores vindos da interface
    int qtd = (int)numQtdCorpos.Value;
    double massaBase = (double)numMassa.Value;

    for (int i = 0; i < qtd; i++)
    {
        // pequenas variações aleatórias de massa e densidade
        double massa = massaBase + rnd.NextDouble() * massaBase * 0.5; // ±50%
        double densidade = rnd.Next(1, 5);

        double posX = rnd.Next(50, panel1.Width - 50);
        double posY = rnd.Next(50, panel1.Height - 50);
        double velX = rnd.Next(-2, 3);
        double velY = rnd.Next(-2, 3);

        universo.AdicionarCorpo(new Corpo(
            0, $"Corpo{i}", massa, densidade, posX, posY, velX, velY
        ));
    }

    contadorIteracoes = 0;
    panel1.Invalidate(false);

    MessageBox.Show($"{qtd} corpos gerados com massa média ~{massaBase:F1}", 
        "Geração concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
}

        private void button2_Click(object sender, EventArgs e)
        {
            if (!simulando)
            {
                timer1.Start();
                simulando = true;
            }
            else
            {
                timer1.Stop();
                simulando = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (universo.Corpos.Count == 0)
            {
                MessageBox.Show("Não há corpos para salvar! Gere corpos primeiro.");
                return;
            }

            try
            {
                persistencia.SalvarConfiguracao(universo.Corpos, contadorIteracoes, timer1.Interval);
                MessageBox.Show("Configuração inicial salva no MySQL!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar configuração: {ex.Message}");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"As iterações são salvas automaticamente a cada {INTERVALO_SALVAMENTO} passos!\n\n" +
                          $"Iterações totais: {contadorIteracoes}\n" +
                          $"Iterações salvas: {contadorIteracoes / INTERVALO_SALVAMENTO}\n" +
                          $"Corpos ativos: {universo.Corpos.Count}");
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() => universo.Atualizar());
                panel1.Invalidate();
                contadorIteracoes++;

                // Salva apenas a cada 50 iterações
                if (contadorIteracoes % INTERVALO_SALVAMENTO == 0)
                {
                    await Task.Run(() => persistencia.SalvarIteracao(universo.Corpos, contadorIteracoes));
                    // Opcional: mostrar no título ou em um label
                    this.Text = $"Simulador - Iteração: {contadorIteracoes} (Salvo)";
                }
                else
                {
                    this.Text = $"Simulador - Iteração: {contadorIteracoes}";
                }
            }
            catch (Exception ex)
            {
                timer1.Stop();
                simulando = false;
                MessageBox.Show($"Erro durante a simulação: {ex.Message}");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (simulando)
                {
                    timer1.Stop();
                    simulando = false;
                }

                universo.Limpar();
                var corpos = persistencia.CarregarUltimaConfiguracao();

                if (corpos.Count == 0)
                {
                    MessageBox.Show("Não há configurações salvas no banco de dados!");
                    return;
                }

                foreach (var corpo in corpos)
                    universo.AdicionarCorpo(corpo);

                contadorIteracoes = 0;
                panel1.Invalidate();
                MessageBox.Show("Última configuração carregada do banco MySQL!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar configuração: {ex.Message}");
            }
        }
    }
}