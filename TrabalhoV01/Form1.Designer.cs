namespace TrabalhoV01
{
    partial class Form1
    {
        private Label qtdCorposLabel;
private Label massaLabel;
private NumericUpDown numQtdCorpos;
private NumericUpDown numMassa;

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            btnGerar = new Button();
            btnIniciar = new Button();
            btnSalvarAtual = new Button();
            btnSalvarConfig = new Button();
            btnCarregar = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            qtdCorposLabel = new Label();
            massaLabel = new Label();
            numQtdCorpos = new NumericUpDown();
            numMassa = new NumericUpDown();
            SuspendLayout();
                // 
// qtdCorposLabel
// 
qtdCorposLabel.AutoSize = true;
qtdCorposLabel.Location = new Point(1163, 80);
qtdCorposLabel.Name = "qtdCorposLabel";
qtdCorposLabel.Size = new Size(125, 20);
qtdCorposLabel.Text = "Quantidade corpos:";
// 
// numQtdCorpos
// 
numQtdCorpos.Location = new Point(1163, 105);
numQtdCorpos.Minimum = 1;
numQtdCorpos.Maximum = 1000;
numQtdCorpos.Value = 100;
numQtdCorpos.Name = "numQtdCorpos";
numQtdCorpos.Size = new Size(117, 27);
// 
// massaLabel
// 
massaLabel.AutoSize = true;
massaLabel.Location = new Point(1163, 150);
massaLabel.Name = "massaLabel";
massaLabel.Size = new Size(110, 20);
massaLabel.Text = "Massa média:";
// 
// numMassa
// 
numMassa.Location = new Point(1163, 175);
numMassa.Minimum = 1;
numMassa.Maximum = 100;
numMassa.Value = 10;
numMassa.Name = "numMassa";
numMassa.Size = new Size(117, 27);
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaptionText;
            panel1.Location = new Point(15, 19);
            panel1.Name = "panel1";
            panel1.Size = new Size(1119, 693);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // btnGerar
            // 
            btnGerar.Location = new Point(1163, 462);
            btnGerar.Name = "btnGerar";
            btnGerar.Size = new Size(117, 46);
            btnGerar.TabIndex = 1;
            btnGerar.Text = "Gerar";
            btnGerar.UseVisualStyleBackColor = true;
            btnGerar.Click += button1_Click;
            // 
            // btnIniciar
            // 
            btnIniciar.Location = new Point(1163, 390);
            btnIniciar.Name = "btnIniciar";
            btnIniciar.Size = new Size(117, 46);
            btnIniciar.TabIndex = 2;
            btnIniciar.Text = "Iniciar";
            btnIniciar.UseVisualStyleBackColor = true;
            btnIniciar.Click += button2_Click;
            // 
            // btnSalvarAtual
            // 
            btnSalvarAtual.Location = new Point(1163, 595);
            btnSalvarAtual.Name = "btnSalvarAtual";
            btnSalvarAtual.Size = new Size(117, 46);
            btnSalvarAtual.TabIndex = 4;
            btnSalvarAtual.Text = "Salvar Atual";
            btnSalvarAtual.UseVisualStyleBackColor = true;
            btnSalvarAtual.Click += button3_Click;
            // 
            // btnSalvarConfig
            // 
            btnSalvarConfig.Location = new Point(1163, 526);
            btnSalvarConfig.Name = "btnSalvarConfig";
            btnSalvarConfig.Size = new Size(117, 46);
            btnSalvarConfig.TabIndex = 3;
            btnSalvarConfig.Text = "Salvar Config";
            btnSalvarConfig.UseVisualStyleBackColor = true;
            btnSalvarConfig.Click += button4_Click;
            // 
            // btnCarregar
            // 
            btnCarregar.Location = new Point(1163, 666);
            btnCarregar.Name = "btnCarregar";
            btnCarregar.Size = new Size(117, 46);
            btnCarregar.TabIndex = 5;
            btnCarregar.Text = "Carregar";
            btnCarregar.UseVisualStyleBackColor = true;
            btnCarregar.Click += button5_Click;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1292, 749);
            Controls.Add(btnCarregar);
            Controls.Add(btnSalvarAtual);
            Controls.Add(btnSalvarConfig);
            Controls.Add(btnIniciar);
            Controls.Add(btnGerar);
            Controls.Add(panel1);
            Controls.Add(qtdCorposLabel);
            Controls.Add(numQtdCorpos);
            Controls.Add(massaLabel);
            Controls.Add(numMassa);
// numQtdCorpos
numQtdCorpos.Minimum = 1;
numQtdCorpos.Maximum = 10000;
numQtdCorpos.Value = 100;

// numMassa
numMassa.Minimum = 1;
numMassa.Maximum = 10000;
numMassa.DecimalPlaces = 1;
numMassa.Value = 10;

            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnGerar;
        private Button btnIniciar;
        private Button btnSalvarAtual;
        private Button btnSalvarConfig;
        private Button btnCarregar;
        private System.Windows.Forms.Timer timer1;
    }
}
