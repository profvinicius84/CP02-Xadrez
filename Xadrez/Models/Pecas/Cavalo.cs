namespace Xadrez.Models.Pecas;

/// <summary>
/// Representa a peça Cavalo no xadrez.
/// </summary>
/// <param name="eBranca">Indica se a peça é branca ou preta.</param>
public class Cavalo(bool eBranca) : Peca(eBranca), ICavalo
{
    /// <summary>
    /// Devolve os movimentos possíveis para o Cavalo.
    /// </summary>
    /// <param name="tabuleiro">O tabuleiro atual do jogo.</param>
    /// <returns>Uma lista de movimentos possíveis para o cavalo.</returns>
    public override List<Movimento> MovimentosPossiveis(Tabuleiro tabuleiro)
    {
        var movimentos = new List<Movimento>();

        Casa origem = tabuleiro.ObtemCasaPeca(this);
        if (origem == null) return movimentos;

        int[,] deslocamentos = new int[,]
        {
            {-2, -1}, {-2, 1}, {-1, -2}, {-1, 2},
            {1, -2}, {1, 2}, {2, -1}, {2, 1}
        };

        for (int i = 0; i < deslocamentos.GetLength(0); i++)
        {
            int novaLinha = origem.Linha + deslocamentos[i, 0];
            int novaColuna = origem.Coluna + deslocamentos[i, 1];

            if (tabuleiro.PosicaoValida(novaLinha, novaColuna))
            {
                Casa destino = tabuleiro.ObtemCasa(novaLinha, novaColuna);

                if (destino.Peca == null || destino.Peca.EBranca != this.EBranca)
                {
                    movimentos.Add(new Movimento(this, origem, destino, destino.Peca));
                }
            }
        }

        return movimentos;
    }
}
