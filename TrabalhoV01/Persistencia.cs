using System.Collections.Generic;

namespace TrabalhoV01
{
    public abstract class Persistencia
    {
        public abstract void SalvarConfiguracao(List<Corpo> corpos, string caminho, int iteracoes, int tempo);
        public abstract List<Corpo> CarregarConfiguracao(string caminho);
        public abstract void SalvarIteracao(List<Corpo> corpos, string caminho, int passo);
    }
}
