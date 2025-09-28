using System;
using System.Collections.Generic;
using System.IO;

namespace TrabalhoV01
{
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
                        densidade: 5, // valor padrão (não estava no arquivo)
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
            using (StreamWriter sw = new StreamWriter(caminho, true))
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
