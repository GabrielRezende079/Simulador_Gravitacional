using System.Collections.Generic;

namespace TrabalhoV01
{
    // Classe abstrata para persistência de dados dos corpos 
    public abstract class Persistencia
    {
        public abstract void SalvarConfiguracao(List<Corpo> corpos, string caminho, int iteracoes, int tempo); // salva a configuração inicial
        public abstract List<Corpo> CarregarConfiguracao(string caminho); // carrega a configuração inicial
        public abstract void SalvarIteracao(List<Corpo> corpos, string caminho, int passo); // salva o estado dos corpos em cada iteração
    }
}
