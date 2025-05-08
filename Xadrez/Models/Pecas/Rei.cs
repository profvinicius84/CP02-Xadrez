
namespace Xadrez.Models.Pecas;

/// <summary>
/// Representa o rei no jogo de xadrez.
/// </summary>
public class Rei : Peca, IRei
{
    /// <summary>
    /// Indica se o rei está em xeque.
    /// </summary>
    public bool EmCheque { get; set; }

    /// <summary>
    /// Construtor da classe Rei.
    /// </summary>
    /// <param name="eBranca">Indica se a peça é branca.</param>
    public Rei(bool eBranca): base(eBranca)
    {
        FoiMovimentada = false;
        EmCheque = false;
    }

    /// <summary>
    /// Obtém os movimentos possíveis para o rei.
    /// </summary>
    /// <param name="tabuleiro">Tabuleiro onde o rei está.</param>
    /// <returns>Retorna uma lista de movimentos possíveis.</returns>
    public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
    {
        var movimentos = new List<Movimento>();
        var casaAtual = tabuleiro.ObtemCasaPeca(this);
        if (casaAtual == null)
            return movimentos;

        int[] direcoesLinha = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] direcoesColuna = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int i = 0; i < 8; i++)
        {
            int novaLinha = casaAtual.Linha + direcoesLinha[i];
            int novaColuna = casaAtual.Coluna + direcoesColuna[i];

            if (novaLinha >= 0 && novaLinha < 8 && novaColuna >= 0 && novaColuna < 8)
            {
                var casaDestino = tabuleiro.Casas.First(c => c.Linha == novaLinha && c.Coluna == novaColuna);
                if (casaDestino.Peca == null || casaDestino.Peca.EBranca != EBranca)
                {
                    movimentos.Add(new Movimento(this, casaAtual, casaDestino));
                }
            }
        }

        return movimentos;
    }

    // <summary>
    /// Método que verifica se o movimento de roque é possível.
    /// </summary>
    /// <param name="tabuleiro"> Instância atual do tabuleiro.</param>
    /// <param name="roquePequeno"> Indica se é um roque pequeno (lado do rei) ou grande (lado da dama). </param>
    /// <returns>Retorna true se o roque é possível; false caso contrário. </returns>
    public bool VerificaRoque(Tabuleiro tabuleiro, bool roquePequeno = false)
    {
        // Verifica se o rei já se moveu ou está em cheque; se sim, não é possível fazer o roque
        if (FoiMovimentada || EmCheque)
            return false;

        // Define a linha onde o rei e torre estão baseados na cor da peça
        int linha = EBranca ? 1 : 8;

        // Define a coluna inicial do rei (posição fixa)
        int colunaRei = 5;

        //Define a coluna onde a torre está, dependendo se o roque é pequeno ou grande
        int colunaTorre = roquePequeno ? 8 : 1;

        // Define a direção do movimento (+1 para direita, -1 para esquerda)
        int direcao = roquePequeno ? 1 : -1;

        //Busca a casa atual do rei no tabuleiro
        var casaRei = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == colunaRei);

        //Busca a casa atual da torre no tabuleiro
        var casaTorre = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == colunaTorre);

        // Verifica se as peças nas casas são respectivamente um Rei e uma Torre válidos e da mesma cor
        if (casaRei?.Peca is not Rei || casaTorre?.Peca is not Torre torre || torre.EBranca != EBranca || torre.FoiMovimentada)
            return false;

        // Verifica se existem peças entre o rei e a torre
        for (int i = 1; i < Math.Abs(colunaRei - colunaTorre); i++)
        {
            //Pega a casa intermediária entre o rei e a torre
            var casaEntre = tabuleiro.Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == colunaRei + direcao * i);

            // Se existir qualquer peça no caminho, o roque não é possível
            if (casaEntre?.Peca != null)
                return false;
        }

        //Verifica se o rei atravessa alguma casa sob ataque durante o roque
        for (int i = 0; i <= 2; i++) //  Inclui a posição final
        {
            int coluna = colunaRei + direcao * i; // Calcula a coluna da casa atual no movimento

            // Obtém a casa atual para simular o movimento do rei
            var casa = tabuleiro.Casas.First(c => c.Linha == linha && c.Coluna == coluna);

            // Cria um movimento simulado do rei até a casa
            var movimentoSimulado = new Movimento(this, casaRei, casa);

            // Executa o movimento simulado
            tabuleiro.ExecutaMovimento(movimentoSimulado);

            //Verifica se, após o movimento, o rei está em cheque
            bool emCheque = tabuleiro.VerificaXeque(EBranca);

            // Reverte o movimento simulado
            tabuleiro.ReverteMovimento(movimentoSimulado);

            //Se o rei estiver em cheque em alguma posição intermediária, o roque é inválido
            if (emCheque)
                return false;
        }

        // Se passou todas as verificações, o roque é possível
        return true;
    }

    /// <summary>
    /// Método que executa efetivamente o roque no tabuleiro.
    /// </summary>
    /// <param name="tabuleiro">Instância atual do tabuleiro.</param>
    /// <param name="roquePequeno">Indica se é um roque pequeno ou grande.</param>
    /// <returns> Retorna o movimento do rei após realizar o roque.</returns>
    public Movimento ExecutaRoque(Tabuleiro tabuleiro, bool roquePequeno = false)
    {
        // Primeiro, verifica se o roque é permitido
        if (!VerificaRoque(tabuleiro, roquePequeno))
            throw new InvalidOperationException("Roque inválido");

        // Define a linha base do rei e da torre (0 para brancas, 7 para pretas)
        int linha = EBranca ? 0 : 7;

        // Define a posição inicial do rei e da torre baseado se é roque pequeno ou grande
        int colunaRei = 4;
        int colunaTorre = roquePequeno ? 7 : 0;

        // Define a direção de movimento (+1 para direita, -1 para esquerda)
        int direcao = roquePequeno ? 1 : -1;

        // Obtém a casa atual do rei
        var casaRei = tabuleiro.Casas.First(c => c.Linha == linha && c.Coluna == colunaRei);

        // Obtém a casa atual da torre
        var casaTorre = tabuleiro.Casas.First(c => c.Linha == linha && c.Coluna == colunaTorre);

        // Define a nova posição para o rei após o roque
        var novaCasaRei = tabuleiro.Casas.First(c => c.Linha == linha && c.Coluna == colunaRei + 2 * direcao);

        // Define a nova posição para a torre após o roque
        var novaCasaTorre = tabuleiro.Casas.First(c => c.Linha == linha && c.Coluna == colunaRei + direcao);

        // Obtém a peça rei da casa original
        var rei = casaRei.Peca as Rei;

        // Obtém a peça torre da casa original
        var torre = casaTorre.Peca as Torre;

        // Move o rei para a nova casa
        novaCasaRei.Peca = rei;
        casaRei.Peca = null;
        FoiMovimentada = true; // Marca que o rei já moveu

        // Move a torre para a nova casa
        novaCasaTorre.Peca = torre;
        casaTorre.Peca = null;
        torre!.FoiMovimentada = true; // Marca que a torre já moveu

        // Retorna o movimento realizado pelo rei
        return new Movimento(rei, casaRei, novaCasaRei);
    }    
} 